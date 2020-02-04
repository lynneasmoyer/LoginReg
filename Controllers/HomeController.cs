using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.EntityFrameworkCore;
using LoginReg.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        public HomeController(HomeContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User register)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == register.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hash = new PasswordHasher<User>();
                    register.Password = hash.HashPassword(register, register.Password);

                    dbContext.Users.Add(register);
                    dbContext.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("signin")]
        public IActionResult SignIn(LoginUser logUser)
        {
            if(ModelState.IsValid)
            {
                User check = dbContext.Users.FirstOrDefault(u => u.Email == logUser.LoginEmail);
                if( check == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid email or password.");
                    return View("Login");
                }
                else
                {
                    PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser>();
                    var result = compare.VerifyHashedPassword(logUser, check.Password, logUser.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginEmail", "Invalid email or password.");
                        return View("Login");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", check.UserId);
                        return RedirectToAction("Success");
                    }
                }
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("success")]

        public IActionResult Success()
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("Logout");
            }
            ViewBag.User = userInDb;
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
            
        }

        private User LoggedIn()
        {
            return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }



        ///////////////////////////////////////////////////

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
