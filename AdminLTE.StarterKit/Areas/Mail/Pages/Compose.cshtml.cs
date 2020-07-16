using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.StarterKit.Models;
using AdminLTE.StarterKit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminLTE.StarterKit.Areas.Mail.Pages
{
    public class ComposeModel : PageModel
    {
        private readonly IEmailService _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public ComposeModel(IEmailService emailSender, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }
        public class InputModel
        {
            [Required]
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string From { get; set; }
            public List<IFormFile> files { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        private async Task<IActionResult> LoadAsync()
        {

            Input = new InputModel
            {
                From = _userManager.GetUserAsync(User).Result.Email,
                files = new List<IFormFile>()
                
            };
            return Page();
        }
        public async Task OnGet()
        {
            await LoadAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _emailSender.Send(Input.To, Input.Subject,Input.Body,from:Input.From,files:Input.files);
            await LoadAsync();
            return Page();
        }
    }
}
