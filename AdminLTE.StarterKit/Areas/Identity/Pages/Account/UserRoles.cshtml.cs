using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.StarterKit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace AdminLTE.StarterKit.Areas.Identity.Pages.Account
{
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _toastNotification;

        public UserRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IToastNotification toastNotification)
        {
            _roleManager = roleManager;
            this._toastNotification = toastNotification;
            _userManager = userManager;

        }
        [BindProperty]
        public List<UserRolesViewModel> UserRoles { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string UserName { get; set; }
        public async Task OnGet(string userId)
        {
            UserRoles = new List<UserRolesViewModel>();
            UserId = userId;
            //ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return;
            }
            UserName = user.UserName;
            //ViewBag.UserName = user.UserName;
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                UserRoles.Add(userRolesViewModel);
            }
        }
        public async Task OnPost()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                _toastNotification.AddErrorToastMessage("Cannot remove user existing roles");
                return;
            }
            result = await _userManager.AddToRolesAsync(user, UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                _toastNotification.AddErrorToastMessage("Cannot add selected roles to user");
                return;
            }
            _toastNotification.AddSuccessToastMessage("Updated User Roles");
            await OnGet(UserId);
        }
    }
    public class UserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
