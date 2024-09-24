using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Collections.Generic;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private object user;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchEmail)
        {
            if (string.IsNullOrWhiteSpace(SearchEmail))
            {
                //var usersVM = await _userManager.Users.ToListAsync();                //   بدل معمل كده واروح احولها ل يوزر فيو موديل انا هستخدم السيليكت ع طول واعمل سيليكت لليوزرفيوموديل في نفس الخطوة بدل مروح اعمل ع خطوتين اني اجيب اليوزرس الاول من الداتابيز والخطوة التانية اني هحولهم لفيوموديل
                var usersVM = await _userManager.Users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = /*await*/ _userManager.GetRolesAsync(user).GetAwaiter().GetResult(),     // هنا مش هينع احط اويت لان ال انونميس  ميثود اللي بتاخدها السيليكت مش شغالة اسينك فهعمل الGetAwaiter().GetResult()  

                }).ToListAsync();
                return View(usersVM);
            }
            var user = await _userManager.FindByEmailAsync(SearchEmail);
            if (user == null) return View(Enumerable.Empty<UserViewModel>());
            var userVM = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };
            return View(new List<UserViewModel> { userVM });    //هعمل كده عشان الفيو مستني مني اي انيمرابل اوف يوزر فيو موديل لاني لو معملتش كده يبقي ببعت للفيو يوزرفيوموديل واحد
        }

        public async Task<IActionResult> Details(string id, string ViewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userVM = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };
            return View(ViewName, userVM);

        }

        public async Task<IActionResult> Edit(string id) => await Details(id, nameof(Edit));
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await _userManager.UpdateAsync(user);
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
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }
            //var userVM = new UserViewModel
            //{
            //    Id = user.Id,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    UserName = user.UserName,
            //    Email = user.Email,
            //    Roles = await _userManager.GetRolesAsync(user)
            //};
            //return View(userVM); 
            return View();
        }

    }
}
