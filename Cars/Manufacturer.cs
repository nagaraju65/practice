using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HeadQuarter { get; set; }
        public int Year { get; set; }

        internal static Manufacturer ConvertLineToManufacturer(string path)
        {
            var results = path.Split(',');
            return new Manufacturer()
            {
                Name = results[0],
                HeadQuarter = results[1],
                Year = int.Parse(results[2])
            };
        }
    }
}
