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

        [StringLength(150)]
        [Required]

        public string DisplayName { get; set; } = default!;

        public int? ParentId { get; set; }

        public int? OrgId { get; set; }

        [StringLength(20)]

        public string? AccountCode { get; set; }

        [StringLength(20)]

        public string? OwnCode { get; set; }

        public bool? Status { get; set; }

        [StringLength(450)]
        public string? CreatedBy { get; set; }


        public DateTime? DateCreated { get; set; }

        [StringLength(450)]
        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
