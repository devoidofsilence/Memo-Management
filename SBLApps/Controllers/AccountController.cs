using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBLApps.Services;
using System.Security.Claims;
using SBLApps.Models;
using System.DirectoryServices.AccountManagement;
using Newtonsoft.Json;
using SBLApps.Enums;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SBLApps.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Fields
        private DbHelper _dbHelper;
        #endregion

        #region Ctor
        public AccountController(DbHelper dBHelper)
        {
            _dbHelper = dBHelper;
        }
        #endregion

        #region Methods

        #region Get
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                if (returnUrl == null)
                {
                    return RedirectToAction("BlacklistingMemoMainList", "BlacklistingMemoMain");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult UserList()
        {
            var userRolesClaim = User.Claims.FirstOrDefault(c => c.Type == "Role");
            if (userRolesClaim != null && userRolesClaim.Value != "")
            {
                var userRoles = userRolesClaim.Value.Split(',').Select(int.Parse).ToList();
                if (userRoles.Contains((int)UserRoleEnum.Administrator))
                {
                    var userList = _dbHelper.GetAllUsersList();
                    return View(userList);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            var userRolesClaim = User.Claims.FirstOrDefault(c => c.Type == "Role");
            if (userRolesClaim != null && userRolesClaim.Value != "")
            {
                var userRoles = userRolesClaim.Value.Split(',').Select(int.Parse).ToList();
                if (userRoles.Contains((int)UserRoleEnum.Administrator))
                {
                    return View(new User());
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> UserRegistrationEdit(int userId)
        {
            var userRolesClaim = User.Claims.FirstOrDefault(c => c.Type == "Role");
            if (userRolesClaim != null && userRolesClaim.Value != "")
            {
                var userRoles = userRolesClaim.Value.Split(',').Select(int.Parse).ToList();
                if (userRoles.Contains((int)UserRoleEnum.Administrator))
                {
                    User user = _dbHelper?.GetAllUsersList()?.Where(x => x.UserId == userId)?.FirstOrDefault() ?? new User();
                    user.UserRoleList = new SelectList(await _dbHelper.GetAllUserRoles(), "UserRoleId", "UserRoleName");
                    user.UserRoleIds = _dbHelper?.GetAllUserRoleIdsOfUser(userId) ?? new List<int>();
                    return View("UserRegistration", user);
                }
            }
            return RedirectToAction("Login");
        }

        public bool UsernameExistsInAD(string username)
        {
            // Establish a connection to the Active Directory domain
            PrincipalContext context = new PrincipalContext(ContextType.Domain, "sbl.com", "tp_app", "$bl@!23$");

            // Create a user principal object with the specific samAccountName
            UserPrincipal userPrincipal = new UserPrincipal(context);
            userPrincipal.SamAccountName = username;

            // Create a principal searcher to perform the search operation
            PrincipalSearcher principalSearcher = new PrincipalSearcher(userPrincipal);

            // Perform the search and retrieve the results
            PrincipalSearchResult<Principal> searchResult = principalSearcher.FindAll();

            // Check if the search result contains any entries
            bool samAccountNameExists = searchResult.Any();

            if (samAccountNameExists)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Post
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            ViewData["ReturnUrl"] = model.RedirectUrl;
            if (!ModelState.IsValid)
            {
                return View();
            }

            //authenticating user from ad. 
            PrincipalContext context = new PrincipalContext(ContextType.Domain, "sbl.com", "tp_app", "$bl@!23$");
            if (context.ValidateCredentials(model.Username, model.Password))
            {
                //after validation. Get Role of User.
                User user = _dbHelper.GetUserRoleByUserName(model.Username);

                if (user == null)
                {
                    ModelState.AddModelError("CustomError", "You are not authorized to use this application. Please contact Administrator.");
                    return View(model);
                }
                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, model.Username),
                                    new Claim("FullName", user.Name),
                                    new Claim("Role", string.Join(",", user.UserRoleIds.Select(role => role.ToString())))
                                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),

                    IsPersistent = false,

                    IssuedUtc = DateTimeOffset.UtcNow
                };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(model.RedirectUrl) && Url.IsLocalUrl(model.RedirectUrl))
                {
                    return LocalRedirect(model.RedirectUrl);
                }
                return RedirectToAction("BlacklistingMemoMainList", "BlacklistingMemoMain");
            }
            ModelState.AddModelError("CustomError", "Username/Password is Incorrect !!!!.");
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut(LoginModel model)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegistration(User user)
        {
            int operationRows = 0;
            try
            {
                if (!UsernameExistsInAD(user.Username))
                {
                    TempData["NotificationType"] = "error";
                    TempData["NotificationMessage"] = $"Username doesnot exist in AD server";
                    return View(user);
                }

                //if (user.UserId == 0) // For new
                //{
                //    if (_dbHelper?.GetAllUsersList()?.Where(x => x.Username?.Trim() == user.Username?.Trim()).Count() > 0)
                //    {
                //        TempData["NotificationType"] = "error";
                //        TempData["NotificationMessage"] = $"Username already exists";
                //        return View(user);
                //    }
                //    user.AddedBy = User.Identity.Name;
                //    user.AddedDate = DateTime.Now;
                //    operationRows = await _dbHelper.SaveUser(user);
                //}
                //else // For edit
                //{
                user.ModifiedBy = User.Identity.Name;
                user.ModifiedDate = DateTime.Now;
                operationRows = await _dbHelper.EditUser(user);
                if (operationRows > 0)
                {
                    await _dbHelper.DeleteUserRoleMappers(user.UserId);
                    foreach (var role in user.UserRoleIds)
                    {
                        await _dbHelper.SaveUserRoleMapper(new UserRoleMapper() { UserId = user.UserId, UserRoleId = role });
                    }
                }
                //}

                // Set TempData for success notification
                TempData["NotificationType"] = "success";
                TempData["NotificationMessage"] = "Data submitted successfully.";

                if (operationRows > 0 && user.UserId != 0)
                {
                    return RedirectToAction("UserList");
                }

                return RedirectToAction("UserRegistration");
            }
            catch (Exception ex)
            {
                // Set TempData for success notification
                TempData["NotificationType"] = "error";
                TempData["NotificationMessage"] = $"Error Occured. {ex.Message}";
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SyncUserFromAD()
        {
            DateTime dtNow = DateTime.Now;
            using (var context = new PrincipalContext(ContextType.Domain, "sbl.com", "tp_app", "$bl@!23$"))
            {
                using (var userPrincipal = new UserPrincipal(context))
                {
                    using (var searcher = new PrincipalSearcher(userPrincipal))
                    {
                        var results = searcher.FindAll();
                        foreach (var result in results)
                        {
                            var user = (UserPrincipal)result;
                            User userinfo = _dbHelper?.GetAllUsersList()?.Where(c => (c.Username.Trim().ToLowerInvariant()) == user?.SamAccountName?.Trim().ToLowerInvariant())?.FirstOrDefault() ?? null;

                            var department = user.GetUnderlyingObject().GetType().GetProperty("Department")?.GetValue(user.GetUnderlyingObject())?.ToString() ?? userinfo?.Department;
                            var designation = user.GetUnderlyingObject().GetType().GetProperty("Title")?.GetValue(user.GetUnderlyingObject())?.ToString() ?? userinfo?.Designation;
                            if (userinfo == null)
                            {
                                //insert
                                userinfo = new User();
                                userinfo.Username = user.SamAccountName;
                                userinfo.Name = user.DisplayName ?? "-";
                                userinfo.Email = user.EmailAddress ?? "-";
                                userinfo.Designation = designation;
                                userinfo.Department = department;
                                userinfo.IsActive = user.Enabled ?? false;
                                //userinfo.Role = "authenticated";
                                userinfo.AddedBy = User.Identity?.Name;
                                userinfo.AddedDate = dtNow;
                                int i = await _dbHelper.SaveUser(userinfo);
                                if (i > 0)
                                {
                                    await _dbHelper.SaveUserRoleMapper(new UserRoleMapper() { UserId = userinfo.UserId, UserRoleId = (int)UserRoleEnum.Authenticated });
                                }
                            }
                            else
                            {
                                //update
                                userinfo.Username = user.SamAccountName;
                                userinfo.Name = user.DisplayName ?? "-";
                                userinfo.Email = user.EmailAddress ?? "-";
                                userinfo.Designation = designation;
                                userinfo.Department = department;
                                userinfo.IsActive = user.Enabled ?? false;
                                userinfo.ModifiedDate = dtNow;
                                userinfo.ModifiedBy = User.Identity?.Name;
                                int i = await _dbHelper.EditUser(userinfo);
                            }
                        }
                    }
                }
                // Set TempData for success notification
                TempData["NotificationType"] = "success";
                TempData["NotificationMessage"] = "Users synced successfully.";
                return RedirectToAction("UserList");
            }
        }

        #endregion

        #endregion
    }
}
