

using PresentationLayer.Utilities;

namespace PresentationLayer.Controllers
{
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
        public IActionResult Index(string? searchName)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrWhiteSpace(searchName))
                employees = _unitOfWork.Employees.GetAllWithDepartments();
            else employees = _unitOfWork.Employees.GetAll(searchName);
            var employeesVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeesVM);
        }

        public IActionResult Create()
        {
            var departments = _unitOfWork.Departments.GetAll();
            SelectList items = new SelectList(departments, "Id", "Name");
            ViewBag.Departments = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.Employees.create(employee);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(employeeVM);
        }

        public IActionResult Details(int? id) => EmployeeControleerHandler(id, nameof(Details));
        public IActionResult Edit(int? id) => EmployeeControleerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.Employees.Update(employee);
                    if (_unitOfWork.SaveChanges() > 0)
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

        public IActionResult Delete(int? id) => EmployeeControleerHandler(id, nameof(Delete));



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _unitOfWork.Employees.Get(id.Value);
            if (employee == null) return NotFound();
            try
            {
                _unitOfWork.Employees.Delete(employee);
                if (_unitOfWork.SaveChanges() > 0 && employee.ImageName is not null)
                    DocumentSettings.DeleteFile("Images",employee.ImageName);   
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
            if (ViewName == nameof(Edit))
            {
                var departments = _unitOfWork.Departments.GetAll();
                SelectList items = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = items;
            }
            if (!id.HasValue) return BadRequest();
            var employee = _unitOfWork.Employees.Get(id.Value);
            //var employeeVM = _mapper.Map<Employee , EmployeeViewModel>(employee);
            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);  //ممكن اكتبها كده عادي
            if (employee == null) return NotFound();
            return View(ViewName, employeeVM);
        }

    }
}

