using Microsoft.AspNetCore.Mvc;
using NetProductApp.Models;
using BCrypt.Net;

namespace NetProductApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string AdminRole = "admin";
        private const string ShopperRole = "shopper";

        public LoginController(ApplicationDbContext context){
            _context = context;
        }

        public IActionResult Index()
        {
            Console.WriteLine("index");
            // just to add an admin login with hashed pw to db
            /*
            string password = "password123";
            var hashedpw = BCrypt.Net.BCrypt.HashPassword(password);
            var login = new Login
            {
                username = "admin1",
                password = hashedpw,
                role = AdminRole

            };
            _context.logins.Add(login);
            await _context.SaveChangesAsync();
            */
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Index(string username, string password)
        {
            Console.WriteLine("index(string username, string password)");
            var login = _context.logins.SingleOrDefault(l => l.username == username);
            
            if(login != null && BCrypt.Net.BCrypt.Verify(password, login.password)){
                Console.WriteLine("correct pw"); 
                if(login.role == AdminRole){
                    HttpContext.Session.SetString("UserRole", AdminRole);
                    TempData["SuccessMessage"] = "Login successful! Welcome, Admin.";
                    return RedirectToAction("Index", "Products");
                }
                else{
                    HttpContext.Session.SetString("UserRole", ShopperRole);
                    TempData["SuccessMessage"] = "Login successful! Welcome, Shopper.";
                    return RedirectToAction("Index", "Products");
                }
            }
            
            // if login failed
            Console.WriteLine("login failed");
            TempData["ErrorMessage"] = "Invalid Login Attempt.";
            ModelState.AddModelError("", "Invalid Login Attempt.");
            //return RedirectToAction("Index", "Login");
            return View();
        }

        // only shoppers register through website, admin are added directly to db
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password1, string password2)
        {
            //var hashedpw = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine(password1);
            Console.WriteLine(password2);
            if(password2 != null && password1 !=null && password1 == password2){
                var login = new Login
                {
                    username = username,
                    password = BCrypt.Net.BCrypt.HashPassword(password1),
                    role = "shopper"

                };
                _context.logins.Add(login);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Regestration Successful.";
            }
            else{
                TempData["ErrorMessage"] = "Registration failed.";
            }
            return View();
        }
        
    }
}
