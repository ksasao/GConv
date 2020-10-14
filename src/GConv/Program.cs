using System;
using System.IO;

namespace GConv
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: gconv [target_file|dropbox_url]");
                Console.WriteLine("File type will be automatically suggested.");
                Console.ReadKey();
                return;
            }

            // export data
            string file = args[0];
            var dm = new DataManager();
            var items = dm.Load(file);
            if(items == null)
            {
                Console.WriteLine($"{file} is not supported format.");
                Console.ReadKey();
                return;
            }
            var outputName = DateTime.Now.ToString("yyyyMMdd_HHmmss")
                + "_" + dm.ParserName
                + ".csv";
            dm.Save(outputName, items);
            Console.WriteLine($"Saved: {outputName}");
        }
    }
}
