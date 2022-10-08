using CRUD.Data;
using CRUD.Models;
using CRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly CrudDbContext _context;

        public EmployeesController(CrudDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var employee = await _context.Employees.ToListAsync();

            const int pageSize = 1;
            if (page < 1)
                page = 1;

            int recordsCount = employee.Count();
            var pager = new Pager(recordsCount, page, pageSize);
            int recordSkip = (page - 1) * pageSize;
            var data = employee.Skip(recordSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);

            //return View(employee);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel model)
        {
            var employee = new Employee()
            {
                Name = model.Name,
                Email = model.Email,
                Salary = model.Salary,
                DateOfBirth = model.DateOfBirth,
                Department = model.Department
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                    Salary = employee.Salary
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await _context.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await _context.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
