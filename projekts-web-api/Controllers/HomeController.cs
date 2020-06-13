using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IO;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly UserRepository userRepository;
        private readonly UserContext context;
        public HomeController(UserContext context, UserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository;
            this.context = context;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        public IActionResult Users(int page = 1)
        {
            int pageSize = 100;

            List<User> SortedUserList = context.userslist.OrderByDescending(x => x.Username).ToList();
            var count = SortedUserList.Count();
            var items = SortedUserList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Users = items
            };



            return View(viewModel);
        }
        [Authorize()]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Azure");
        }

        [Authorize()]
        public ActionResult Home()
        {
            return RedirectToAction("Home", "Azure");
        }


        [Authorize(Roles = "admin")]
        public ActionResult ResolutionList()
        {
            var model = userRepository.GetResolutions();
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddResolutions(int id)
        {
            LimitedResolution model = id == default ? new LimitedResolution() : userRepository.GetResolutionById(id);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddResolutions(LimitedResolution model)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveResolution(model);
                return RedirectToAction("ResolutionList");
            }

            return View(model);
        }


        [Authorize(Roles = "admin")]
        public ActionResult ExtensionList()
        {
            var model = userRepository.GetExtensions();
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddExtensions(int id)
        {
            FileProperties model = id == default ? new FileProperties() : userRepository.GetExtensionById(id);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddExtensions(FileProperties model)
        {
            if (ModelState.IsValid)
            {

                var isFileNameExtensionAlreadyexists = context.extensionlist.Any(x => x.FileNameExtension == model.FileNameExtension);
                if (isFileNameExtensionAlreadyexists)
                {
                    ModelState.AddModelError("FileNameExtension", "FileNameExtension already exists");
                    return View(model);
                }

                userRepository.SaveExtension(model);
                return RedirectToAction("ExtensionList");
            }

            return View(model);
        }




        [Authorize(Roles = "admin")]
        public ActionResult FileSize()
        {
            var model = userRepository.GetFileSize();
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddFileSize(int id)
        {
            LimitedSize model = id == default ? new LimitedSize() : userRepository.GetFileSizeById(id);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddFileSize(LimitedSize model)
        {
            if (ModelState.IsValid)
            {

                userRepository.SaveFileSize(model);
                return RedirectToAction("FileSize");
            }

            return View(model);
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult ExtensionDelete(int id)
        {
            userRepository.DeleteExtension(new FileProperties { Id = id });
            return RedirectToAction("ExtensionList");
        }




        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registration(int id)
        {
            User model = id == default ? new User() : userRepository.GetUserById(id);
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Registration(User model)
        {
            if (ModelState.IsValid)
            {

                var isUsernameAlreadyExists = context.userslist.Any(x => x.Username == model.Username);
                if (isUsernameAlreadyExists)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(model);
                }

                userRepository.SaveUser(model);
                return RedirectToAction("Index", "Azure");
            }

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AdmRegistration(int id)
        {
            User model = id == default ? new User() : userRepository.GetUserById(id);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AdmRegistration(User model)
        {
            if (ModelState.IsValid)
            {
                var isUsernameAlreadyExists = context.userslist.Any(x => x.Username == model.Username);
                if (isUsernameAlreadyExists)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(model);
                }
                userRepository.SaveUser(model);
                return RedirectToAction("Index", "Azure");
            }

            return View(model);
        }


        [Authorize(Roles = "admin")]
        public IActionResult UserEdit(int id)
        {
            User model = id == default ? new User() : userRepository.GetUserById(id);
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UserEdit(User model)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveUser(model);
                return RedirectToAction("Users", "Home");
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UserDelete(int id)
        {
            userRepository.DeleteUser(new User { Id = id });
            return RedirectToAction("Users");
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await context.userslist.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Username, model.Password);

                    return RedirectToAction("Index", "Azure");
                }
                ModelState.AddModelError("", "Wrong Username or Password");
                return View(model);
            }
            return View(model);
        }
        [AllowAnonymous]
        private async Task Authenticate(string username, string password)
        {
            var user = context.userslist.SingleOrDefault(x => x.Username == username && x.Password == password);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize()]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

    }

}

