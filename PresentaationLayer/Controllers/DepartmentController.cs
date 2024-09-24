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

        public async Task<IActionResult> Index()
        {
            var departments = await _DeptRepo.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   await _DeptRepo.createAsync(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int? id) => await DepartmentControleerHandlerAsync(id, nameof(Details));
        public async Task<IActionResult> Edit(int? id) => await DepartmentControleerHandlerAsync(id, nameof(Edit));

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

        public async Task<IActionResult> Delete(int? id) => await DepartmentControleerHandlerAsync(id, nameof(Delete));
       


        [HttpPost ,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var department =await _DeptRepo.GetAsync(id.Value);
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

        public async Task<IActionResult> DepartmentControleerHandlerAsync(int? id , string ViewName)     //ممكن دي بس اللي اخلي اخرها اسينك لانها برايفت
        {
            if (!id.HasValue) return BadRequest();
            var department = await _DeptRepo.GetAsync(id.Value);
            if (department == null) return NotFound();
            return View(ViewName, department);
        }

    }
}
