using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _repo;

        public EmployeeController(IEmployeeRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            var employees = _repo.GetAll();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.create(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(employee);
        }

        public IActionResult Details(int? id) => EmployeeControleerHandler(id, nameof(Details));
        public IActionResult Edit(int? id) => EmployeeControleerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(employee);
        }

        public IActionResult Delete(int? id) => EmployeeControleerHandler(id, nameof(Delete));



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _repo.Get(id.Value);
            if (employee == null) return NotFound();
            try
            {
                _repo.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(employee);
        }

        public IActionResult EmployeeControleerHandler(int? id, string ViewName)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _repo.Get(id.Value);
            if (employee == null) return NotFound();
            return View(ViewName, employee);
        }

    }
}

