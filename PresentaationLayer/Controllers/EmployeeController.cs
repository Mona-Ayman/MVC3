

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchName)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrWhiteSpace(searchName))
                employees =await _unitOfWork.Employees.GetAllWithDepartmentsAsync();
            else employees = await _unitOfWork.Employees.GetAllAsync(searchName);
            var employeesVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeesVM);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            SelectList items = new SelectList(departments, "Id", "Name");
            ViewBag.Departments = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                        employeeVM.ImageName =await DocumentSettings.UploadFileAsync(employeeVM.Image, "Images");
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
					await _unitOfWork.Employees.createAsync(employee);
					await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id) => await EmployeeControleerHandlerAsync(id, nameof(Details));
        public async Task<IActionResult> Edit(int? id) => await EmployeeControleerHandlerAsync(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                        employeeVM.ImageName = await DocumentSettings.UploadFileAsync(employeeVM.Image, "Images");
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.Employees.Update(employee);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                    {
                        TempData["Message"] = $"Employee {employeeVM.Name} Updated Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int? id) =>await EmployeeControleerHandlerAsync(id, nameof(Delete));



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = await _unitOfWork.Employees.GetAsync(id.Value);
            if (employee == null) return NotFound();
            try
            {
                _unitOfWork.Employees.Delete(employee);
                if (await _unitOfWork.SaveChangesAsync() > 0 && employee.ImageName is not null)
                    DocumentSettings.DeleteFile("Images",employee.ImageName);   
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);
            return View(employeeVM);
        }

        public async Task<IActionResult> EmployeeControleerHandlerAsync(int? id, string ViewName)
        {
            if (ViewName == nameof(Edit))
            {
                var departments =await _unitOfWork.Departments.GetAllAsync();
                SelectList items = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = items;
            }
            if (!id.HasValue) return BadRequest();
            var employee = await _unitOfWork.Employees.GetAsync(id.Value);
            //var employeeVM = _mapper.Map<Employee , EmployeeViewModel>(employee);
            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);  //ممكن اكتبها كده عادي
            if (employee == null) return NotFound();
            return View(ViewName, employeeVM);
        }

    }
}

