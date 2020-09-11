using System;

namespace AutoPartsDownloader.Helpers
{
    public static class ValueValidator
    {
        /// <summary>
        /// Удаляет из строки все не числовые и текстовые значения
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string ReturnStringWithOnlyLettersAndNumbers(string originalString)
        {
            string NewString = "";
            foreach (char c in originalString)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    NewString += c;
                }
            }
            return NewString;
        }

        public static string ReturnStringWithoutComparisonOperators(string originalString)
        {
            string NewString = originalString;
            NewString = NewString.Replace(">", "");
            NewString = NewString.Replace("<", "");
            var indexDash = NewString.LastIndexOf("-");
            if (indexDash > 0)
            {
                NewString= NewString.Remove(0, indexDash + 1);
            }
            return NewString;
        }


    }
}
