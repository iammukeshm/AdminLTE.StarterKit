using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.StarterKit.Services
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html, string from = null, List<IFormFile> files = null);
    }
}
