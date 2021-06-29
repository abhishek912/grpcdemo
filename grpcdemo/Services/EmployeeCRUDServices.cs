using Grpc.Core;
using GRPCService1;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPCDemo
{
    public class EmployeeCRUDServices : EmployeeCRUD.EmployeeCRUDBase
    {
        private readonly ILogger<EmployeeCRUDServices> _logger;

        private AppDbContext db = null;
        public EmployeeCRUDServices(ILogger<EmployeeCRUDServices> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public override Task<GRPCService1.Employees> SelectAll(Empty requestData, ServerCallContext contact)
        {
            Employees responseData = new Employees();
            var employeeData = db.Employees.ToList();

            var query = employeeData.Select(x =>
                            new GRPCService1.Employee()
                            {
                                EmployeeID = x.EmployeeID,
                                FirstName = x.FirstName,
                                LastName = x.LastName
                            }
                        );

            responseData.Items.AddRange(query.ToArray());
            return Task.FromResult(responseData);
        }

        public override Task<GRPCService1.Employee> SelectById(EmployeeFilter requestData, ServerCallContext context)
        {
            var employees = db.Employees.ToList();
            var data = employees.Find(x => x.EmployeeID == requestData.EmployeeID);

            GRPCService1.Employee employee = new GRPCService1.Employee
            {
                EmployeeID = data.EmployeeID,
                FirstName = data.FirstName,
                LastName = data.LastName
            };

            return Task.FromResult(employee);
        }

        public override Task<Empty> Insert(GRPCService1.Employee requestData, ServerCallContext context)
        {
            Employee emp = new Employee
            {
                EmployeeID = requestData.EmployeeID,
                FirstName = requestData.FirstName,
                LastName = requestData.LastName
            };

            db.Employees.Add(emp);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Update(GRPCService1.Employee requestData, ServerCallContext context)
        {
            Employee emp = new Employee
            {
                EmployeeID = requestData.EmployeeID,
                FirstName = requestData.FirstName,
                LastName = requestData.LastName
            };

            db.Employees.Update(emp);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Delete(EmployeeFilter requestData, ServerCallContext contact)
        {
            var emp = db.Employees.ToList().Find(x => x.EmployeeID == requestData.EmployeeID);

            db.Employees.Remove(emp);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }
    }
}
