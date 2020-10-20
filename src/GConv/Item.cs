using System;
using System.Collections.Generic;
using System.Text;

namespace GConv
{
    public class Item
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Glucose (mg/dL)
        /// </summary>
        public int Glucose { get; set; }
        /// <summary>
        /// Measurement type
        /// </summary>
        public int MeasurementType { get; set; }
        public override string ToString()
        {
            string csv = $"{Date.ToString("yyyy/MM/dd HH:mm:ss")},{Glucose},{MeasurementType}";
            return csv;
        }
    }
}
