using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EasyCashIdentityProject.EntityLayer.Concrete;

namespace OzSapkaTShirt.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            ApplicationUser? user;

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            user = await _userManager.Users.Include(u => u.GenderType).Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            SelectList genders = new SelectList(_context.Genders, "Id", "Name");
            ViewData["Genders"] = genders;
            SelectList cities = new SelectList(_context.Cities, "PlateCode", "Name");

            ViewData["Cities"] = cities;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            IdentityResult? identityResult;
            SelectList genders, cities;

            if (ModelState.IsValid)
            {
                identityResult = _userManager.CreateAsync(user, user.PassWord).Result;
                if (identityResult == IdentityResult.Success)
                {
                    //Add customer role to user

                    return RedirectToAction("Index", "Home");
                }
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            genders = new SelectList(_context.Genders, "Id", "Name");
            cities = new SelectList(_context.Cities, "PlateCode", "Name");
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            SelectList genders, cities;

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            cities = new SelectList(_context.Cities, "PlateCode", "Name", user.CityCode);
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user.Trim());
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            IdentityResult? identityResult;
            SelectList genders, cities;
            ApplicationUser existingUser;

            if (id != user.Id)
            {
                return NotFound();
            }

            ModelState.Remove("PassWord");
            ModelState.Remove("ConfirmPassWord");
            if (ModelState.IsValid)
            {
                existingUser = _userManager.FindByIdAsync(id).Result;
                existingUser.Name = user.Name;
                existingUser.SurName = user.SurName;
                existingUser.Corporate = user.Corporate;
                existingUser.Address = user.Address;
                existingUser.Gender = user.Gender;
                existingUser.BirthDate = user.BirthDate;
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.CityCode = user.CityCode;
                identityResult = _userManager.UpdateAsync(existingUser).Result;
                if (identityResult == IdentityResult.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            cities = new SelectList(_context.Cities, "PlateCode", "Name", user.CityCode);
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.GenderType).Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'ApplicationContext.Product'  is null.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if ((Request.Cookies.ContainsKey("userName") & Request.Cookies.ContainsKey("passWord")))
            {
                ViewBag.username = Request.Cookies["userName"];
                ViewBag.password = Request.Cookies["passWord"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ApplicationUser user)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult;

            if (ModelState["UserName"].ValidationState == ModelValidationState.Valid)
            {
                if (ModelState["Password"].ValidationState == ModelValidationState.Valid)
                {
                    signInResult = _signInManager.PasswordSignInAsync(user.UserName, user.PassWord, false, false).Result;
                    if (signInResult.Succeeded == true)
                    {
                        string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        Order order = _context.Orders
                            .Where(o => o.UserId == userIdentity && o.Status == 0)
                            .Include(o => o.OrderProducts).FirstOrDefault();
                        if (order != null)
                        {
                            HttpContext.Session.SetInt32("BasketCount", order.OrderProducts.Sum(op => op.Quantity));
                        }

                        else
                        {
                            HttpContext.Session.SetInt32("BasketCount", 0);
                        }
                        if (user.RememberMe)
                        {
                            Response.Cookies.Append("userName", user.UserName);
                            Response.Cookies.Append("passWord", user.PassWord);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(user);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string Password)
        {
            string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityResult identityResult;
            ApplicationUser existingUser = _userManager.FindByIdAsync(userIdentity).Result;

            existingUser.PassWord = Password;
            existingUser.ConfirmPassWord = Password;
            existingUser.UserName = existingUser.UserName.Trim();
            identityResult = _userManager.ChangePasswordAsync(existingUser, oldPassword, Password).Result;
            if (identityResult.Succeeded == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            ApplicationUser applicationUser;

            string resetToken;

            if (ModelState.IsValid)
            {
                applicationUser = await _userManager.FindByEmailAsync(forgetPasswordViewModel.eMail);
                if (applicationUser != null)
                {
                    resetToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                    new SendMail(forgetPasswordViewModel.eMail, "jarvis.tony.34@gmail.com", "pqabajejwxfnsiau", "Admin-CapStone",
                        forgetPasswordViewModel.eMail, "Şifre Yenileme", "Şifre Yenileme için Kod : " + resetToken);

                    ViewData["eMail"] = forgetPasswordViewModel.eMail;
                    return View("ResetPassword");
                }
                ModelState.AddModelError("eMail", "Kullanıcı Bulunamadı");
            }
            ModelState.AddModelError("eMail", "Geçersiz Bir Mail Adresi Girildi");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            ApplicationUser applicationUser;
            IdentityResult identityResult;

            if (ModelState.IsValid)
            {
                applicationUser = await _userManager.FindByEmailAsync(resetPasswordViewModel.eMail);
                applicationUser.UserName = applicationUser.UserName.Trim();
                identityResult = await _userManager.ResetPasswordAsync(applicationUser, resetPasswordViewModel.Code, resetPasswordViewModel.PassWord);
                if (identityResult.Succeeded)
                {
                    RedirectToAction("Login");
                }
                ModelState.AddModelError("Code", "Doğru Kod Girdiğinizden Emin olun");

            }
            ModelState.AddModelError("PassWord", "Geçersiz Bir Parola Girildi");

            return View();
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
