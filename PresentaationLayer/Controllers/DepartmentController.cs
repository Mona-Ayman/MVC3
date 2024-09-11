using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class DepartmentController : Controller
    {
        
        private readonly IDepartmentRepository _DeptRepo;

        public DepartmentController(IDepartmentRepository DeptRepo)
        {
            _DeptRepo = DeptRepo;
        }

        public IActionResult Index()
        {
            var departments = _DeptRepo.GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _DeptRepo.create(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public IActionResult Details(int? id) => DepartmentControleerHandler(id, nameof(Details));
        public IActionResult Edit(int? id) => DepartmentControleerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _DeptRepo.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public IActionResult Delete(int? id) => DepartmentControleerHandler(id,nameof(Delete));
       


        [HttpPost ,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department =_DeptRepo.Get(id.Value);
            if (department == null) return NotFound();
            try
            {
                _DeptRepo.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
            }
            return View(department);
        }

        public IActionResult DepartmentControleerHandler(int? id , string ViewName)
        {
            if (!id.HasValue) return BadRequest();
            var department = _DeptRepo.Get(id.Value);
            if (department == null) return NotFound();
            return View(ViewName, department);
        }

    }
}
