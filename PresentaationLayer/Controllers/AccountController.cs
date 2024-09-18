
namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		public IActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            var user = new ApplicationUser
            { 
              UserName = model.UserName, 
              FirstName = model.FirstName,
              LastName = model.LastName,
			  Email = model.Email,
			};
            var result = _userManager.CreateAsync(user,model.Password).Result;
            if (result.Succeeded)
                return RedirectToAction(nameof(Login));
            foreach(var error in result.Errors) 
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }
		public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //check email
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                //check password
                if(_userManager.CheckPasswordAsync(user,model.Password).Result) 
                {
                    //login
                    var result = _signManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;
                    if (result.Succeeded) return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", string.Empty));
                }
            }
            ModelState.AddModelError(string.Empty,"Incorrect Email Or Password");
            return View(model);
        }

        public IActionResult LogOut()
        {
            _signManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
		public IActionResult ForgetPassword(ForgetPasswordViewModel model)
		{
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                //create token
                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                //create url
                var url = Url.Action(nameof(ResetPassword), nameof(AccountController).Replace("Controller", string.Empty),
                    new { Email = model.Email, Token = token },Request.Scheme);
                //create email object
                var email = new Email
                {
                    Subject = "Reset Password",
                    Body = url!,
                    Recipient = model.Email
                };
                //send email
                MailSettings.SendMail(email);
                //redirect to check the inbox
                return RedirectToAction(nameof(ChaeckYourInBox));

            }
            ModelState.AddModelError(string.Empty, "User Not Found");
			return View(model);
		}

		public IActionResult ChaeckYourInBox()
		{
			return View();
		}
        public IActionResult ResetPassword(string email, string token)
        {
            if (email is null || token is null) return BadRequest();
            TempData["email"] = email;
            TempData["token"] = token;
			return View();
		}
        [HttpPost]
		public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            model.Email = TempData["email"]?.ToString() ?? string.Empty;
            model.Token = TempData["token"]?.ToString() ?? string.Empty;
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                var result = _userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;
                if (result.Succeeded) return RedirectToAction(nameof(Login));
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
			ModelState.AddModelError(string.Empty, "User Not Found");
            return View(model);

		}

	}
}
