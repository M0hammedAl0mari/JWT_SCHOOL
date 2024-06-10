using Microsoft.AspNetCore.Mvc;
using lastTest.Models;
using lastTest.DataBase;
using lastTest.Repository;
using lastTest.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace lastTest.Controllers
{
    public class AccessController : Controller
    {
        private readonly UserRepository _repository;
        private readonly TokenService _tokenService;
        private readonly MyContext _context;

        public AccessController(UserRepository userRepository, TokenService tokenService, MyContext context)
        {
            _repository = userRepository;
            _tokenService = tokenService;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            var user = _repository.LoginUser(modelLogin);

            if (user != null)
            {
                var _token = _tokenService.GenerateToken(user.UserId, user.Role.Name);

                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    Path = "/",
                };

                Response.Cookies.Append("AuthToken", _token, cookieOptions);
                var userId = user.UserId;

                if (user.Role.Name == "Student")
                {
                    return RedirectToAction("Index", "Student", new { userID = userId });
                }
                else
                {
                    return RedirectToAction("Index", "Teacher", new { userID = userId });
                }
            }

            ViewData["ValidateMessage"] = "User Not Found";
            return View();
        }

        public IActionResult LogOut()
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                Path = "/",
            };

            Response.Cookies.Append("AuthToken", "", cookieOptions);
            return View("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(VMRegister model)
        {
            if (ModelState.IsValid)
            {
                User user;          
                if (model.Role == "Teacher")
                {
                    user = new Teacher
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password, 
                        RoleId = _context.Roles.FirstOrDefault(r => r.Name == "Teacher").Id,
                        gender = model.Gender
                    };
                }
                else if (model.Role == "Student")
                {
                    user = new Student
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password, 
                        RoleId = _context.Roles.FirstOrDefault(r => r.Name == "Student").Id,
                        Age = model.Age
                    };
                }
                else
                {
                    ModelState.AddModelError("", "Invalid role selected.");
                    return View(model);
                }

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Student");
            }
            return View(model);
        }
    }
}
