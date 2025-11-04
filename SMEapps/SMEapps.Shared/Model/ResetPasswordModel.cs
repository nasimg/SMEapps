using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class ResetPasswordModel
    {
        public string Email { get; set; } = "";
        public string ResetPasswordToken { get; set; } = "";
        [Required, MinLength(6)]
        public string NewPassword { get; set; } = "";
        [Required, Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
