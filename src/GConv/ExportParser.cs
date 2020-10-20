using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GConv
{
    public class ExportParser : IParser
    {
        public string Name { get; set; } = "Export";

        public Item[] Parse(string filename)

        {
            List<Item> items = new List<Item>();
            if (!File.Exists(filename))
            {
                return items.ToArray();
            }
            try
            {
                string[] list = File.ReadAllLines(filename);
                for(int i=2; i<list.Length; i++)
                {
                    string line = list[i].Replace("\0", "");
                    var p = ParseLine(line);
                    if (p != null)
                    {
                        items.Add(p);
                    }
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return items.OrderBy(c => c.Date).ToArray();
        }

        private Item ParseLine(string data)
        {
            // 154	2020/10/06 10:27	1		94
            string[] s = data.Split('\t');
            if(s.Length != 19)
            {
                return null;
            }
            try
            {
                DateTime date = System.DateTime.ParseExact(s[1],"yyyy/MM/dd HH:mm",
                    System.Globalization.DateTimeFormatInfo.InvariantInfo,
                    System.Globalization.DateTimeStyles.None);
                int glucose = s[4]==""?Convert.ToInt32(s[3]):Convert.ToInt32(s[4]);
                int type = Convert.ToInt32(s[2]);
                if(type == 6)
                {
                    return null;
                }
                return new Item { Date = date, Glucose = glucose, MeasurementType = type };
            }
            catch
            {
                return null;
            }
        }
    }
}
