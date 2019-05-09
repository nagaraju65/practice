using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class CarDB : DbContext
    {
        public CarDB() : base()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDB>());
        }
        public DbSet<Car> cars { get; set; }
    }
}
