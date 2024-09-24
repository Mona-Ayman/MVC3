using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchName)
        {
            if (string.IsNullOrWhiteSpace(SearchName))
            {
                var rolesVM = await _roleManager.Roles.Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToListAsync();
                return View(rolesVM);
            }
            var role = await _roleManager.FindByNameAsync(SearchName);
            if (role == null) return View(Enumerable.Empty<RoleViewModel>());
            var roleVM = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(new List<RoleViewModel> { roleVM });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var role = new IdentityRole
            {
                Name = model.Name,
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }
        public async Task<IActionResult> Details(string id, string ViewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            var roleVM = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(ViewName, roleVM);
        }

        public async Task<IActionResult> Edit(string id) => await Details(id, nameof(Edit));
        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound();
                role.Name = model.Name;
                await _roleManager.UpdateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);

        }
        public async Task<IActionResult> Delete(string id) => await Details(id, nameof(Delete));
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound();
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            //var roleVM = new RoleViewModel
            //{
            //    Id = id,
            //    Name = role.Name,
            //};
            //return View(roleVM);
            return View();
        }

        public async Task<IActionResult> AddOrRemoveUserRoles(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewBag.roleId = roleId;
            var users = await _userManager.Users.ToListAsync();
            var usersVMs = new List<UserRoleViewModel>();    //بعمل ليست فاضية عشان اضيف فيها اليوزرس بعد محولهم لفيوموديل
            foreach (var user in users)
            {
                var usersVM = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    HasRole = await _userManager.IsInRoleAsync(user, role.Name)
                };
                usersVMs.Add(usersVM);
            }
            return View(usersVMs);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUserRoles(string roleId, List<UserRoleViewModel> usersVM)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            if (!ModelState.IsValid) return View(usersVM);
            foreach (var userVM in usersVM)
            {
                var user = await _userManager.FindByIdAsync(userVM.UserId);
                if (user is null) return NotFound();
                if (!await _userManager.IsInRoleAsync(user, role.Name) && userVM.HasRole)
                    await _userManager.AddToRoleAsync(user, role.Name);
                if (await _userManager.IsInRoleAsync(user, role.Name) && !userVM.HasRole)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Edit), new { id = roleId });
        }


    }
}
