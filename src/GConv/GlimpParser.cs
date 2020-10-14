using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace GConv
{
    public class GlimpParser : IParser
    {
        public string Name { get; set; }  = "Glimp";

        public Item[] Parse(string filename)
        {
            List<Item> items = new List<Item>();
            try
            {
                string[] list = LoadFile(filename);
                for(int i=0; i<list.Length; i++)
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
            // 5;11/10/2020 16.51.30;1602402690;;;74;74;;5;0M000DF471D;;;;;
            string[] s = data.Split(';');
            if(s.Length != 15)
            {
                return null;
            }
            try
            {
                DateTime date = System.DateTime.ParseExact(s[1],"dd/MM/yyyy HH.mm.ss",
                    System.Globalization.DateTimeFormatInfo.InvariantInfo,
                    System.Globalization.DateTimeStyles.None);
                int glucose = Convert.ToInt32(s[5]);
                int type = Convert.ToInt32(s[8]);
                return new Item { Date = date, Glucose = glucose, MeasurementType = type };
            }
            catch
            {
                return null;
            }
        }

        private string[] LoadFile(string filename)
        {
            if (IsURL(filename))
            {
                return ReadAllLinesFromUrl(filename);
            }
            if (IsGZipped(filename))
            {
                return ReadAllLinesGZipped(filename);
            }
            return File.ReadAllLines(filename);
        }


        private bool IsURL(string filename)
        {
            return filename.Trim().StartsWith("https://");
        }
        private string[] ReadAllLinesFromUrl(string url)
        {
            List<string> lines = new List<string>();
            url = url.Replace("?dl=0", "?dl=1");

            string tempFile = "tmp.gz";
            using (var wc = new System.Net.WebClient())
            {
                wc.DownloadFile(url, tempFile);
            }
            string[] data = LoadFile(tempFile);
            File.Delete(tempFile);
            return data;
        }


        private bool IsGZipped(string filename)
        {
            bool isGZipped = false;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] buf = new byte[2];
                fs.Read(buf, 0, 2);
                if(buf[0] == 0x1f && buf[1] == 0x8B)
                {
                    isGZipped = true;
                }
            }
            return isGZipped;
        }
        private string[] ReadAllLinesGZipped(string filename)
        {
            List<string> lines = new List<string>();
            using(FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using(GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using(StreamReader sr = new StreamReader(gs))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                }
            }
            return lines.ToArray();
        }
    }
}
