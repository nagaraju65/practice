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
            var Cars = ProcessCars("fuel.csv");
            var Manufacturers = ProcessManufacturers("manufacturers.csv");
            ExecuteFunctionality(Cars, Manufacturers);

        }

        private static void ExecuteFunctionality(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from car in cars
                        join manufacturer in manufacturers on car.Division equals manufacturer.Name
                        select new
                        {
                            country = manufacturer.HeadQuarter,
                            name = car.Division,
                            Combined_FE = car.FE_Comb
                        };

            foreach (var car in Query)
            {
                Console.WriteLine($"{car.country}:{car.name}:{car.Combined_FE}");
            }

            /*
            ShowCars(cars);
            ShowManufacturers(manufacturers);
            */
        }

        private static void ShowManufacturers(List<Manufacturer> Manufacturers)
        {
            foreach (var Manufacturer in Manufacturers)
            {
                Console.WriteLine($"{Manufacturer.Name} : {Manufacturer.HeadQuarter}");
            }
        }

        private static void ShowCars(List<Car> Cars)
        {


            foreach (var car in Cars)
            {
                Console.WriteLine(car.Division);
            }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var Query = File.ReadAllLines(path)
                            .Where(line => line.Length > 1)
                            .Select(Manufacturer.ConvertLineToManufacturer)
                            .OrderBy(m => m.Name);

            return Query.ToList();


        }

        private static List<Car> ProcessCars(string path)
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

                Model_Year = dr[0],
                Division = dr[1],
                Carline = dr[2],
                Eng_Displ = double.Parse(dr[3]),
                Cyl = int.Parse(dr[4]),
                FE_City = int.Parse(dr[5]),
                FE_Hwy = int.Parse(dr[6]),
                FE_Comb = int.Parse(dr[7])
            };
        }
    }
}
