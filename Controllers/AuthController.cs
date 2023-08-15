using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using mvc.Data;
using mvc.Models;
using mvc.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            return RedirectToAction("Login");
        }
        
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserDto req)
        {
            User User = new User();
            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User.Username = req.Username;
            User.PasswordHash = passwordHash;
            User.PasswordSalt = passwordSalt;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Can't Register");
                return View(req);
            }
            _db.User.Add(User);
            await _db.SaveChangesAsync();
            return RedirectToAction("Login","Auth");
        }
        [HttpGet]
        public ActionResult Login() {
            ClaimsPrincipal claimUser = HttpContext.User;
            
            if (claimUser.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Book");
            }
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Login(UserDto req)
        {
            
            if (!_db.User.Any(x=> x.Username == req.Username))
            {
                ModelState.AddModelError("", "User Not Found");
                return View(req);
            }
            User User = _db.User.First(u => u.Username == req.Username);
            if (!VerifyPasswordHash(req.Password, User.PasswordHash, User.PasswordSalt)) {
                ModelState.AddModelError("","Wrong Authentication");
                return View(req);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.Username),
                new Claim(ClaimTypes.Role, User.Role)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            if(User.Role=="admin"){
                return RedirectToAction("Index", "Book");
            }
            return RedirectToAction("Index", "Store");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

        public IActionResult Denied(){
            return View();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }
    
}
