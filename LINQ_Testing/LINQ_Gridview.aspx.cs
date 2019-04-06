using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LINQ_Testing
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var Employees = RetrieveAllEmployees();

            //var FilteredEmployees = Employees.Where(e1 => (e1.FirstName.StartsWith("N") && e1.AnnualPackage>500000) );
            
            //var FilteredEmployees = Employees
            //                         .Where(q => q.AnnualPackage > 600000)
            //                         .GroupBy(q => q.Laoction);
            
            //var FilteredEmployees = from Emp in Employees
            //                        where Emp.LastName.Contains("ara")
            //                        select Emp;

            var FilteredEmployees = Employees.Where(c => c.AnnualPackage < 600000);

            
            GridView1.DataSource = FilteredEmployees;
            GridView1.DataBind();
        }

        private List<Employee> RetrieveAllEmployees()
        {
            List<Employee> Employees = new List<Employee>() {
                new Employee() {FirstName="Nagaraju",LastName="Salendra",Age=27,Location="Hyderabad",AnnualPackage=700000 },
                new Employee() {FirstName="Nikesh",LastName="Charabuddi",Age=26,Location="Hyderabad",AnnualPackage=590000 },
                new Employee() {FirstName="Sambeet",LastName="Mohanty",Age=25,Location="Orissa",AnnualPackage=520000 },
                new Employee() {FirstName="Rudra",LastName="Narayan",Age=25,Location="Orissa",AnnualPackage=550000 },
                new Employee() {FirstName="Shameem",LastName="Ahmed",Age=25,Location="Hyderabad",AnnualPackage=560000 },
                new Employee() {FirstName="Sai",LastName="Chand",Age=29,Location="Hyderabad",AnnualPackage=790000 },
                new Employee() {FirstName="Chandrasekhar",LastName="Poshala",Age=35,Location="Hyderabad",AnnualPackage=900000 },
                new Employee() {FirstName="Siva",LastName="Yedula",Age=30,Location="Hyderabad",AnnualPackage=690000 }
            };
            return Employees;
        }
    }

    public class Employee
    {
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public int AnnualPackage { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
    }
}