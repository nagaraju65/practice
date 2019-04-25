using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = ProcessCSV("fuel.csv");
            foreach (var car in source)
            {
                Console.WriteLine(car.Division);
            }
        }

        private static List<Car> ProcessCSV(string path)
        {
            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(line => line.Length > 1)
                            .Select(ParseStringtoCar)
                            .ToList();
            return query;
        }

        private static Car ParseStringtoCar(string path)
        {
            var dr = path.Split(',');
            return new Car()
            {
                 
                Model_Year = dr[0] ,
                Division = dr[1] ,
                Carline = dr[2] ,
                Eng_Displ = double.Parse(dr[3]) ,
                Cyl = int.Parse(dr[4]) ,
                FE_City = int.Parse(dr[5]) ,
                FE_Hwy = int.Parse(dr[6]) ,
                FE_Comb = int.Parse(dr[7])
            };
        }
    }
}
