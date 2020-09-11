using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsDownloader.Helpers;
using AutoPartsDownloader.Models;
using AutoPartsDownloader.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoPartsDownloader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoPartsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public ActionResult Get()
        {
            List<AutoParts> partslist;
            using (var db = new ApplicationContext())
            {
                partslist= db.AutoParts.Select(ap=>ap).ToList();
            }
            return new ObjectResult(partslist);
        }

        [HttpGet("AddValuesFromMail")]
        public ActionResult AddValuesFromMail(string ProviderName)
        {
            //Выгружаем вложения из почты
            MailParser.DownloadAttachmentFromMail();
            //ProviderName = "ООО Запчасти даром";
            Dictionary<string, int> ColumnNumber = new Dictionary<string, int>();
            ColumnNumber = ColumnConfiguration.ReeturnDictionaryWithColumnName(ProviderName);
            List<AutoParts> listAutoParts = new List<AutoParts>();

            foreach (string file in Directory.GetFiles(@"/Users/a777/Downloads/", "*.csv"))
            {//Ищем все загруженные csv файлы
                bool isColumnName = true;
                using (var reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isColumnName)
                        {//Сохраняем наименований колонок и их индексы
                            int i = 0;
                            foreach (var value in line.Split(';'))
                            {
                                if (ColumnNumber.ContainsKey(value))
                                {
                                    ColumnNumber[value] = i;
                                }
                                i++;
                            }
                            if (ColumnNumber.Values.Any(k => k == -1))
                            {
                                return BadRequest($"Не найдено поле {ColumnNumber.Where(cl => cl.Value == -1).First().Key} в файле {file}");
                            }
                            isColumnName = false;
                        }
                        else
                        {
                            try
                            {
                                var listValues = line.Split(';');

                                //ColumnNumber хранит индексы столбцов со значениями
                                //ColumnConfiguration.ReturnProviderColumnName возвращает наименований полей для конкретного поставщика
                                var vendor = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Производитель")]].ToString().ToUpper();
                                var number = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Номер запчасти")]].ToString().ToUpper();
                                var searchVendor = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Производитель для поиска")]].ToString();
                                searchVendor = ValueValidator.ReturnStringWithOnlyLettersAndNumbers(searchVendor);
                                var searchNumber = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Номер для поиска")]].ToString();
                                searchNumber = ValueValidator.ReturnStringWithOnlyLettersAndNumbers(searchNumber);
                                var description = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Наименование")]].ToString();
                                if (description.Length > 512)
                                {
                                    description = description.Remove(512, description.Length - 512);
                                }
                                double price = 0;
                                Double.TryParse(listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Цена")]].ToString().Replace(",","."),
                                    out price);
                                var strCount = listValues[ColumnNumber[ColumnConfiguration.ReturnProviderColumnName(ProviderName, "Количество")]].ToString();
                                strCount = ValueValidator.ReturnStringWithoutComparisonOperators(strCount);
                                var count = Int32.Parse(strCount);

                                if (!listAutoParts.Any(ap => ap.Number == number))
                                {
                                    listAutoParts.Add(new AutoParts(vendor, number, searchVendor, searchNumber, description, price, count));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    foreach (var parts in listAutoParts)
                    {
                        db.AutoParts.Add(parts);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok("Информация успешно добавлена");
        }

        // GET: api/values
        [HttpGet("AddTestValues")]
        public ActionResult AddTestValues()
        {
            using (var db = new ApplicationContext())
            {
                db.AutoParts.Add(new Models.AutoParts("111", "111", "111", "111", "111", 111, 111));
                db.AutoParts.Add(new Models.AutoParts("222", "222", "222", "222", "222", 222, 222));
                db.SaveChanges();
            }
            return Ok("Информация успешно добавлена");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
