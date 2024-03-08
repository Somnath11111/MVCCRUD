using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models;

namespace MVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MyDbContext myDbContext;

        public EmployeesController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = this.myDbContext.Employees.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeModel model)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email
            };

            await myDbContext.Employees.AddAsync(employee);
            await myDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await myDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeModel model)
        {
            var employee = await myDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Id = model.Id;
                employee.Name = model.Name;
                employee.Email = model.Email;
                await myDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeModel model)
        {
            var employee = await myDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                myDbContext.Employees.Remove(employee);
                await myDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
        
}
