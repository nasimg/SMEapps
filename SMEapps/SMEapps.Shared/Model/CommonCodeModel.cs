using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class CommonCodeModel
    {
        [Key]
        public int CommonCodeId { get; set; }

        [Required]
        public string CodeType { get; set; } = default!;

        [Required]
        [StringLength(100)]

        public string CodeValue { get; set; } = default!;

        public string DisplayName { get; set; } = default!;

        public int? ParentId { get; set; }

        public int? OrgId { get; set; }


        public string? AccountCode { get; set; }

        public string? OwnCode { get; set; }

        public bool? Status { get; set; }

        public string? CreatedBy { get; set; }


        public DateTime? DateCreated { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
