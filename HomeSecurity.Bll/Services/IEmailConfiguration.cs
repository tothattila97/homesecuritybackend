using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Services
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        string SmtpAddress { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
    }
}
