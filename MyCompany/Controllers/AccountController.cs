using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using MyCompany.Models;

namespace MyCompany.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public class UserRolesViewModel
        {

            public string RoleId { get; set; }
            public string UserId { get; set; }
        }




        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IRule> roleManager;



        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.Title = "Вход";
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

       
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Services");
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Title = "Регистрация";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.UserName };
                // добавляем пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                 
                if (result.Succeeded)
                {

                    result = await userManager.AddToRoleAsync(user, "user");
                }
                if (result.Succeeded)
                {
                   
                     await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    return RedirectToAction("Index", "Services");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}