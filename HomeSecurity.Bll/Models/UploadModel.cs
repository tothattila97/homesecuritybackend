using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSecurity.Bll.Models
{
    public class UploadModel
    {
        public IFormFile ImageFile { get; set; }
        public bool IsNotifiableByEmail { get; set; }
    }
}
