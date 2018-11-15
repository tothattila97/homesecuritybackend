using HomeSecurity.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Services
{
    public interface IEmailService
    {
        void Send(EmailMessageModel emailMessage);
    }

}
