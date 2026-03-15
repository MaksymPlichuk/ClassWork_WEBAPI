using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class ServiceResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public object? Payload { get; set; } = null;
    }
}
