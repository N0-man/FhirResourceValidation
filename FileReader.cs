using System;
using System.IO;

namespace fhir_cs_profiling_basic
{
    public static class FileReader
    {
        public static string ReadFhirInputFile(string filename)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data/{filename}");
            Console.WriteLine(path);
            var stream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data/{filename}"), FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            stream.Close();
            return result;
        }
    }
}