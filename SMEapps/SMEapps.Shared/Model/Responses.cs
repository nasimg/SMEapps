using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class Responses
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public string ReturnCode { get; set; } = "";
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
