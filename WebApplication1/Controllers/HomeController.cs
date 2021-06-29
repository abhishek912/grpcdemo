using Grpc.Net.Client;
using GRPCService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmployeeCRUD.
            EmployeeCRUDClient(channel);

            Employees emps = client.SelectAll(new Empty());

            Empty response1 = client.Insert(new Employee()
            {
                FirstName = "Tom",
                LastName = "Jerry"
            });

            Empty response2 = client.Insert(new Employee()
            {
                FirstName = "Abhishek",
                LastName = "Sharma"
            });

            Employee a = client.SelectById(new EmployeeFilter { EmployeeID = 2 });

            Empty b = client.Update(new Employee { EmployeeID = 2, FirstName = "Simran", LastName = "Sharma" });

            Empty c = client.Delete(new EmployeeFilter { EmployeeID = 3 });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
