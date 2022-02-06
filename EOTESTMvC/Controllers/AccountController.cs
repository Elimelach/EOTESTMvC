using EOTESTMvC.Models;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EOTESTMvC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User1> userManager;
        private readonly SignInManager<User1> signInManager;
        private readonly IHttpContextAccessor ctor;

        public AccountController(UserManager<User1> userMngr, SignInManager<User1> signInMngr,
            IHttpContextAccessor ctx)
        {
            userManager = userMngr;
            signInManager = signInMngr;
            ctor = ctx;
        }
        public IActionResult Index()
        {
            return RedirectToAction("LogIn");
        }
        [HttpGet]
        public ViewResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }
       
       
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var user = new User1
                {
                    UserName = model.Email,
                };
               
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.PasswordSignInAsync(model.Email,model.Password,false,false);



                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError("", e.Description);
                    }
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult LogIn()
        {
            var model = new LogViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                        return RedirectToAction("Index", "Home");
                }
            }
            
            ModelState.AddModelError("", "Invalid username/password.");
            return View("LogIn",model);
        }

        








    }
}


