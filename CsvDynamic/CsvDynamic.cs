using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace CsvDynamic
{
    public static class CsvDynamic
    {
        /// <summary>
        /// Reads a CSV file and outputs dynamic objects.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<dynamic> Convert(string filePath)
        {
            return File.ReadAllLines(filePath).Convert();
        }

        /// <summary>
        /// Reads a CSV string array and outputs dynamic objects.
        /// </summary>
        /// <param name="csvLines"></param>
        /// <returns></returns>
        public static List<dynamic> Convert(string[] csvLines)
        {
            return csvLines.Convert();
        }

        /// <summary>
        /// Reads a stream containing CSV and outputs dynamic objects.
        /// </summary>
        /// <param name="csvStream"></param>
        /// <returns></returns>
        public static List<dynamic> Convert(Stream csvStream)
        {
            return csvStream.ToStringArray().Convert();
        }

        /// <summary>
        /// Reads a CSV file and outputs mapped dynamic objects.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public static List<T> Convert<T>(string filePath, Func<dynamic, T> mapFunction)
        {
            return File.ReadAllLines(filePath).Convert(mapFunction);
        }

        /// <summary>
        /// Reads a CSV string array and outputs mapped dynamic objects.
        /// </summary>
        /// <param name="csvLines"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public static List<T> Convert<T>(string[] csvLines, Func<dynamic, T> mapFunction)
        {
            return csvLines.Convert(mapFunction);
        }

        /// <summary>
        /// Reads a stream containing CSV and outputs mapped dynamic objects.
        /// </summary>
        /// <param name="csvStream"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        public static List<T> Convert<T>(Stream csvStream, Func<dynamic, T> mapFunction)
        {
            return csvStream.ToStringArray().Convert(mapFunction);
        }
    }

    internal static class CsvDynamicExtensions
    {
        /// <summary>
        /// Converts a CSV string to dynamic objects.
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        internal static List<dynamic> Convert(this string[] csvString)
        {
            // If no rows, then it won't work.
            if (!csvString.Any()) throw new CsvDynamicException("No rows were found.");

            // If only a header row, got a problem.
            if (csvString.Count() == 1) throw new CsvDynamicException("Only one row was found.");

            // Get all items into rows
            var csvArray = csvString.Select(ConvertStringToArray).ToList();

            // Get header row
            var header = csvArray.First();

            // Ensure that each row has the same count, if not, that's not good
            var fourSided = csvArray.All(row => row.Length == header.Length);
            if (!fourSided) throw new CsvDynamicException("Not all rows had equal cell count.");

            // Sanitize header items
            header = header.Select(SanitizeForProperty).ToArray();

            // If duplicate header items, can't make properties
            if (header.Distinct().Count() < header.Length)
            {
                throw new CsvDynamicException("Some columns have duplicate names.");
            }
            
            // Get the other rows
            var items = csvArray.Skip(1).ToList();

            // Create list of dynamic objects
            var returnList = new List<dynamic>();

            // Populate properties of objects
            foreach (var item in items)
            {
                var listItem = new ExpandoObject();
                var li = listItem as IDictionary<String, object>;
                for (var i = 0; i < header.Count(); i++)
                {
                    li[header[i]] = item[i];
                }
                returnList.Add(listItem);
            }

            return returnList;
        }

        /// <summary>
        /// Converts a string to an array using the Microsft.VisualBasic text field parser.
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        internal static string[] ConvertStringToArray(string csvString)
        {
            var reader = new StringReader(string.Join(Environment.NewLine, csvString));
            using (var parser = new TextFieldParser(reader))
            {
                parser.Delimiters = new[] { "," };
                while (true)
                {
                    var parts = parser.ReadFields();
                    return parts ?? (new List<string>()).ToArray();
                }
            }
        }

        /// <summary>
        /// Sanitizes a string for use as a property name.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string SanitizeForProperty(string input)
        {
            // Remove all non-alphanumeric
            var re1 = new Regex("[^a-zA-Z0-9]");
            input = re1.Replace(input, string.Empty);

            // URL that generated this code (this went over my ability to do RegEx's...)
            // http://txt2re.com/index-csharp.php3?s=1SomethingSomething23Something&2
            const string re2 = ".*?"; // Non-greedy match on filler
            const string re3 = "((?:[a-z][a-z0-9_]*))"; // Variable Name 1

            var r = new Regex(re2 + re3, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var m = r.Match(input.Replace(" ", string.Empty));
            return m.Success ? m.Groups[1].ToString() : string.Empty;
        }

        /// <summary>
        /// Converts a CSV string to dynamic objects, which then get mapped with a MapFunction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvString"></param>
        /// <param name="mapFunction"></param>
        /// <returns></returns>
        internal static List<T> Convert<T>(this string[] csvString, Func<dynamic, T> mapFunction)
        {
            var converted = Convert(csvString);
            return converted.Select(i => (T)mapFunction(i)).ToList();
        }

        /// <summary>
        /// Converts a stream to a string array.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static string[] ToStringArray(this Stream stream)
        {
            var list = new List<string>();
            using (var sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list.ToArray();
        }
    }

    public class CsvDynamicException : Exception
    {
        public CsvDynamicException(string message) : base(message) { }
    }
}
