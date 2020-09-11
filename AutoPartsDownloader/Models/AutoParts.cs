using System;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsDownloader.Models
{

    public class AutoParts
    {

        public AutoParts(string vendor, string number, string searchVendor, string searchNumber,
            string description, double price, int count)
        {
            Vendor = vendor;
            Number = number;
            SearchVendor = searchVendor;
            SearchNumber = searchNumber;
            Description = description;
            Price = price;
            Count = count;
        }


        /// <summary>
        /// Производитель
        /// </summary>
        public string Vendor { get; set; }

        [Key]
        /// <summary>
        /// Номер запчасти
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Производитель для поиска
        /// </summary>
        public string SearchVendor { get; set; }

        /// <summary>
        /// Номер для поиска
        /// </summary>
        public string SearchNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Стоимость, тип decimal почему то не нашел в моей версии БД
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }



    }
}
