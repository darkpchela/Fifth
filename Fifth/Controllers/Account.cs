using Fifth.Interfaces;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fifth.Controllers
{
    public class Account : Controller
    {
        private readonly IAppAuthenticationService authenticationService;

        public Account(IAppAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new UserSignInVM());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInVM signInVM)
        {
            if (!ModelState.IsValid)
                return View(signInVM);

            var res = await authenticationService.SignInAsync(signInVM.Login, signInVM.Password);
            if (!res)
            {
                ModelState.AddModelError("Authorization error", "Invalid login/password");
                return View(signInVM);
            }

            return RedirectToAction(nameof(Home.Index), nameof(Home));
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new UserSignUpVM());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpVM signUpVM)
        {
            if (!ModelState.IsValid)
                return View(signUpVM);
            var res = await authenticationService.SignUpAsync(signUpVM.Login, signUpVM.Password);
            if (!res)
            {
                ModelState.AddModelError("Registration error", "User already exists");
                return View(signUpVM);
            }

            return RedirectToAction(nameof(Home.Index), nameof(Home));
        }

        public async Task<IActionResult> SignOut()
        {
            await authenticationService.SignOutAsync();
            return RedirectToAction(nameof(Home.Index), nameof(Home));
        }
    }
}