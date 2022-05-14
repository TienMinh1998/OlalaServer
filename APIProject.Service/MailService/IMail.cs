using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Service.MailService
{
    public interface IMail
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
