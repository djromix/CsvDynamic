using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToLinq
{
    public class CsvToLinq
    {
        /// <summary>
        /// Reads a CSV file and outputs dynamic objects.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public dynamic ReadCsv(string filePath)
        {
            return File.ReadAllLines(filePath).ConvertFromCsv();
        }

        /// <summary>
        /// Reads a CSV file, and outputs mapped dynamic objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public T ReadCsv<T>(string filePath, Func<dynamic, T> mapFunction)
        {
            return File.ReadAllLines(filePath).ConvertFromCsv(mapFunction);
        }
    }

    public static class CsvToLinqExtensions
    {
        /// <summary>
        /// Converts a CSV string to dynamic objects.
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        public static dynamic ConvertFromCsv(this string[] csvString)
        {
            
        } 

        /// <summary>
        /// Converts a CSV string to dynamic objects, which then get mapped with a MapFunction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvString"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public static T ConvertFromCsv<T>(this string[] csvString, Func<dynamic, T> mapFunction)
        {
            return ConvertFromCsv(csvString).Select(mapFunction);
        }
    }
}
