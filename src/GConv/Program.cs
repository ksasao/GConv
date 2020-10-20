using System;
using System.IO;
using System.Linq;

namespace GConv
{
    class Program
    {
        static int Main(string[] args)
        {
            // Error code https://docs.microsoft.com/ja-jp/windows/win32/debug/system-error-codes--0-499-
            if (args.Length == 0)
            {
                Console.WriteLine("usage: gconv [target_file|dropbox_url]");
                Console.WriteLine("File type will be automatically suggested.");
                Console.ReadKey();
                return 0; // ERROR_SUCCESS
            }

            // export data
            string file = args[0];
            var dm = new DataManager();
            var items = dm.Load(file);
            if(items == null)
            {
                Console.WriteLine($"{file} is not supported format.");
                Console.ReadKey();
                return 50; // ERROR_NOT_SUPPORTED
            }
            if(items.Length == 0)
            {
                Console.WriteLine($"{file} is empty.");
                Console.ReadKey();
                return 1; // ERROR_INVALID_FUNCTION
            }

            var outputName = items.Last().Date.ToString("yyyyMMdd_HHmm")
                + "_" + dm.ParserName
                + ".csv";
            if (File.Exists(outputName))
            {
                Console.WriteLine($"{file} is up to date.");
                return 80; // ERROR_FILE_EXISTS
            }
            dm.Save(outputName, items);
            Console.WriteLine($"Saved: {outputName}");
            return 0; // ERROR_SUCCESS
        }
    }
}
