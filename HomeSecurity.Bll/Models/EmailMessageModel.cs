using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class EmailMessageModel
    {
        public EmailMessageModel()
        {
            ToAddresses = new List<EmailAddressModel>();
            FromAddresses = new List<EmailAddressModel>();
        }

        public List<EmailAddressModel> ToAddresses { get; set; }
        public List<EmailAddressModel> FromAddresses { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
