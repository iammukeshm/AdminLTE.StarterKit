using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE.StarterKit.Areas.Identity.Pages.Account
{
    public class RolesModel : PageModel
    {
        public List<IdentityRole> Roles{ get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            this.Roles = new List<IdentityRole>();
        }
        public async Task OnGet()
        {
            Roles = await _roleManager.Roles.ToListAsync();
        }
        public async Task OnPostAsync(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            await OnGet();
        }
    }
}
