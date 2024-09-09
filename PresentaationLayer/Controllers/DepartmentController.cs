using BusinessLogicLayer.Repositories;
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

        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _DeptRepo.GetDepartment(id.Value);
            if(department == null) return NotFound();
            return View(department);
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _DeptRepo.GetDepartment(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if (id != department.Id) return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    _DeptRepo.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex) 
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department = _DeptRepo.GetDepartment(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _DeptRepo.Delete(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

    }
}
