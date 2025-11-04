using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class BranchInfo
    {
        [Required(ErrorMessage = "Branch Code is required")]
        public string BranchCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string BranchName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? BranchShortName { get; set; }

        [StringLength(50)]
        public string? ContactPerson { get; set; }

        [StringLength(120)]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(30)]
        public string? Telephone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(30)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? BranchType { get; set; }

        [StringLength(50)]
        public string? CellPhone { get; set; }

        [StringLength(20)]
        public string? AreaCode { get; set; }

        public bool IsEnabledTRG { get; set; } = true;

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
