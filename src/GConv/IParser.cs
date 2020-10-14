using System;
using System.Collections.Generic;
using System.Text;

namespace GConv
{
    public interface IParser
    {
        public Item[] Parse(string filename);
        public string Name { get; set; }
    }
}
