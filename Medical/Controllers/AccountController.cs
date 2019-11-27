using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Medical.Utility;

namespace Medical.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model, string returnUrl = "/")
        {
            UserModel loginUser = null;
            try
            {
                loginUser = _dbContext.Users.Where(user => user.user_email == model.user_email && user.user_password == model.user_password).First();
            }
            catch
            {
                ViewData["ErrorMessage"] = "Your credential is incorrect.";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim("user_name", loginUser.user_name == null ? "" : loginUser.user_name),
                new Claim(ClaimTypes.Role, loginUser.user_role),
                new Claim("user_id", "" + loginUser.user_id),
                new Claim("user_phone", loginUser.user_phone == null ? "" : loginUser.user_phone),
                new Claim("user_dob", loginUser.user_dob == null ? "" : loginUser.user_dob),
                new Claim("user_address", loginUser.user_address == null ? "" : loginUser.user_address),
            };

            var userIdentity = new ClaimsIdentity(claims, "user");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");
            // check there is same user email with this model.
            int nCnt = _dbContext.Users.Where(user => user.user_email == model.user_email).Count();
            if (nCnt > 0)
            {
                ViewData["ErrorMessage"] = "There exists an user with this email. please try with other one.";
                return View();
            }
            model.user_role = Global.ROLE_CUSTOMER;
            _dbContext.Users.Add(model);
            _dbContext.SaveChanges();

            List<Claim> claims = new List<Claim>
            {
                new Claim("user_name", model.user_name == null ? "" : model.user_name),
                new Claim(ClaimTypes.Role, model.user_role),
                new Claim("user_id", "" + model.user_id),
                new Claim("user_phone", model.user_phone == null ? "" : model.user_phone),
                new Claim("user_dob", model.user_dob == null ? "" : model.user_dob),
                new Claim("user_address", model.user_address == null ? "" : model.user_address),
            };

            var userIdentity = new ClaimsIdentity(claims, "user");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

        public IActionResult Employee(string message = "")
        {
            List<UserModel> arrEmployees = _dbContext.Users.Where(user => user.user_role == Global.ROLE_CLERK || user.user_role == Global.ROLE_DELIVERY).ToList();
            ViewData["message"] = message;
            return View(arrEmployees);
        }
    
        // Update or Create Clerk and Delivery body
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UserModel model)
        {
            string message = "";
            int nCnt = _dbContext.Users.Where(user => user.user_email == model.user_email).Count();
            if (nCnt > 0 && model.user_id == 0)
            {
                message = "There exists an user with this email. please try with other one.";
                return RedirectToAction("Employee", new { message = message });
            }

            if (model.user_id == 0)
            {
                message = "Created new user successfully.";
            }
            else
            {
                message = "Updated the user successfully.";
            }

            _dbContext.Users.Update(model);
            _dbContext.SaveChanges();
            
            return RedirectToAction("Employee", new { message = message});
        }

        //Delete a user
        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                _dbContext.Users.Remove(_dbContext.Users.Find(id));
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

    }
}