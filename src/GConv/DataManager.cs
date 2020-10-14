using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GConv
{
    public class DataManager
    {
        /// <summary>
        /// Parser Type
        /// </summary>
        public string ParserName { get; private set; }
        /// <summary>
        /// Parse raw data. It is automatically determined which application the data came from.
        /// </summary>
        /// <param name="filename">target file</param>
        /// <returns>item list sorted by date</returns>

        public Item[] Load(string filename)
        {
            IParser[] parsers = { new GlimpParser(), new ExportParser()};
            foreach(var p in parsers)
            {
                ParserName = p.Name;
                var data = p.Parse(filename);
                if(data.Length > 0)
                {
                    return data;
                }
            }
            ParserName = null;
            return null;
        }
        /// <summary>
        /// Save as CSV file
        /// </summary>
        /// <param name="filename">CSV filename</param>
        /// <param name="items">item list</param>
        public void Save(string filename, Item[] items)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var i in items)
                {
                    string line = $"{i.Date},{i.Glucose},{i.MeasurementType}";
                    sw.WriteLine(line);
                }
            }

        }
    }
}
