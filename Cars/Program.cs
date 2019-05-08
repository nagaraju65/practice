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
            Aggregations_UsingAggregateMethod(cars);
            //Aggreation_On_Cars(cars);

            /*
            ShowCars(cars);
            ShowManufacturers(manufacturers);
            TestingJoin(cars, manufacturers);
            JoinWithTwoOnConditions(cars, manufacturers);
            GroupByManufacturer(cars);
            GroupbyManufacturer_OrderbyCountry(cars, manufacturers);
            GroupingCarsbyManufacturer_UsingGroupJoin(cars, manufacturers);
            FirstThreeEfficientCars_UsingGroupJoin_and_SelectMany(cars, manufacturers);
             Aggreation_On_Cars(cars);
            */
        }

        private static void Aggregations_UsingAggregateMethod(List<Car> cars)
        {
            var Query = cars.GroupBy(c => c.Division)
                                    .Select(g =>
                                    {
                                        var results = g.Aggregate(new CarStatistics(), (acc, c) => acc.Accumulate(c), acc => acc.Compute());

                                        return new
                                        {
                                            Name = g.Key,
                                            Max = results.Max,
                                            Min = results.Min,
                                            Avg = results.Average

                                        };


                                    })
                                    .OrderByDescending(c => c.Max);
            foreach (var result in Query)
            {
                Console.WriteLine($"Name : {result.Name}");
                Console.WriteLine($"\tMaximum FE : {result.Max}");
                Console.WriteLine($"\tMinimum FE : {result.Min}");
                Console.WriteLine($"\tAverage FE : {result.Avg}");
            }
        }

        private static void Aggreation_On_Cars(List<Car> cars)
        {
            var Query = from car in cars
                        group car by car.Division
                                    into CarGroup
                        select new
                        {
                            name = CarGroup.Key,
                            max = CarGroup.Max(c => c.FE_Comb),
                            min = CarGroup.Min(c => c.FE_Comb),
                            avg = CarGroup.Average(c => c.FE_Comb)
                        } into result
                        orderby result.max descending
                        select result;

            var Query2 = cars.GroupBy(c => c.Division)
                             .Select(g => new
                             {
                                 name = g.Key,
                                 max = g.Max(c => c.FE_Comb),
                                 min = g.Min(c => c.FE_Comb),
                                 avg = g.Average(c => c.FE_Comb)
                             })
                              .OrderByDescending(r => r.max);

            foreach (var result in Query2)
            {
                Console.WriteLine($"Name : {result.name}");
                Console.WriteLine($"\tMaximum FE : {result.max}");
                Console.WriteLine($"\tMinimum FE : {result.min}");
                Console.WriteLine($"\tAverage FE : {result.avg}");
            }
        }

        private static void FirstThreeEfficientCars_UsingGroupJoin_and_SelectMany(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from manufacturer in manufacturers
                        join car in cars on manufacturer.Name.ToUpper() equals car.Division.ToUpper()
                        into CarGroup
                        orderby manufacturer.HeadQuarter
                        select new
                        {
                            manufacturer = manufacturer,
                            cars = CarGroup
                        } into result
                        group result by result.manufacturer.HeadQuarter;

            var Query2 = manufacturers.GroupJoin(cars, m => m.Name.ToUpper(), c => c.Division.ToUpper(),
                                                 (m, g) => new
                                                 {
                                                     manufacturer = m,
                                                     cars = g
                                                 })
                         .OrderBy(g => g.manufacturer.HeadQuarter)
                         .GroupBy(g => g.manufacturer.HeadQuarter);


            foreach (var group in Query)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.SelectMany(c => c.cars).OrderByDescending(c => c.FE_Comb).Take(3))
                {
                    Console.WriteLine($"\t {car.Carline} : {car.FE_Comb}");

                }
            }
        }

        private static void GroupingCarsbyManufacturer_UsingGroupJoin(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from manufacturer in manufacturers
                        join car in cars on manufacturer.Name.ToUpper() equals car.Division.ToUpper()
                        into CarGroup
                        orderby manufacturer.Name
                        select new
                        {
                            manufacturer = manufacturer,
                            cars = CarGroup
                        };

            var Query2 = manufacturers.GroupJoin(cars, m => m.Name.ToUpper(), c => c.Division.ToUpper(),
                                                 (m, g) => new
                                                 {
                                                     manufacturer = m,
                                                     cars = g
                                                 })
                         .OrderBy(c => c.manufacturer.Name);

            foreach (var group in Query)
            {
                Console.WriteLine($"{group.manufacturer.Name} : {group.manufacturer.HeadQuarter}");
                foreach (var car in group.cars.OrderByDescending(c => c.FE_Comb).Take(2))
                {
                    Console.WriteLine($"\t{car.Carline} : {car.FE_Comb}");
                }
            }
        }

        private static void GroupbyManufacturer_OrderbyCountry(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from manufacturer in manufacturers
                        join car in cars on manufacturer.Name.ToUpper() equals car.Division.ToUpper()
                        select new
                        {
                            car = car,
                            manufacturer = manufacturer
                        }
                        into totalCarsInfo
                        orderby totalCarsInfo.manufacturer.HeadQuarter
                        group totalCarsInfo by totalCarsInfo.manufacturer.HeadQuarter;

            var Query2 = cars.Join(manufacturers, c => c.Division.ToUpper(), m => m.Name.ToUpper(),
                                    (c, m) => new
                                    {
                                        car = c,
                                        manufacturer = m
                                    })
                          .OrderBy(g => g.manufacturer.HeadQuarter)
                          .GroupBy(g => g.manufacturer.HeadQuarter);


            foreach (var group in Query2)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.OrderByDescending(c => c.car.FE_Comb).Take(2))
                {
                    Console.WriteLine($"\t {car.car.Carline} : {car.car.FE_Comb}");
                }
            }
        }

        private static void GroupByManufacturer(List<Car> cars)
        {
            var Query = from car in cars
                        group car by car.Division;

            var Query2 = cars.GroupBy(c => c.Division);

            foreach (var group in Query2)
            {
                Console.WriteLine($"{group.Key} : {group.Count()}");
            }
        }

        private static void JoinWithTwoOnConditions(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from car in cars
                        join manufacturer in manufacturers on new { manufacturer = car.Division, year = car.Model_Year }
                                                              equals new { manufacturer = manufacturer.Name, year = manufacturer.Year.ToString() }
                        select new
                        {
                            car = car.Carline,
                            manufacturer = car.Division,
                            country = manufacturer.HeadQuarter,
                            year = car.Model_Year
                        }
                        into result
                        orderby result.country
                        select result;

            var Query2 = cars.Join(manufacturers, c => new { manufacturer = c.Division, year = c.Model_Year },
                                   m => new { manufacturer = m.Name, year = m.Year.ToString() }, (c, m) => new
                                   {
                                       car = c.Carline,
                                       manufacturer = c.Division,
                                       country = m.HeadQuarter,
                                       year = c.Model_Year
                                   })
                                   .OrderBy(c => c.country);

            foreach (var car in Query2)
            {
                Console.WriteLine($"{car.country} : {car.car} : {car.manufacturer} : {car.year}");

            }
        }

        private static void TestingJoin(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var Query = from car in cars
                        join manufacturer in manufacturers on car.Division equals manufacturer.Name
                        select new
                        {
                            country = manufacturer.HeadQuarter,
                            name = car.Division,
                            Combined_FE = car.FE_Comb
                        };

            var Query2 = cars.Join(manufacturers, c => c.Division, m => m.Name, (c, m) => new
            {
                country = m.HeadQuarter,
                name = c.Division,
                Combined_FE = c.FE_Comb
            });

            foreach (var car in Query2)
            {
                Console.WriteLine($"{car.country}:{car.name}:{car.Combined_FE}");
            }
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

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = int.MinValue;
            Min = int.MaxValue;
        }

        public int total { get; set; }
        public int count { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }

        public CarStatistics Accumulate(Car car)
        {
            total += car.FE_Comb;
            count += 1;
            Max = Math.Max(Max, car.FE_Comb);
            Min = Math.Min(Min, car.FE_Comb);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = (double)total / count;
            return this;
        }
    }
}
