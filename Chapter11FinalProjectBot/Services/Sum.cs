using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Chapter11FinalProjectBot.Services
{
    public static class Sum
    {
        public static double[] GetNumbersFromMessage(String message, out string errorMessage )
        {
            string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] numbers = new double[words.Length];
            errorMessage = "";
            for (int i = 0; i < words.Length; i++)     
            {
                try
                {
                    {
                        numbers[i] = Convert.ToDouble(words[i]);
                    }
                }
                catch (Exception e)
                {
                    errorMessage = "Введены неверные данные, повторите ввод";

                }
            }
            return numbers;
           
        }
        public static double SumNumbers(double[] numbersArray)
        {
            double sum = 0;
            foreach(var number in numbersArray)
            { sum += number; }
            return sum;

        }
    }
}
