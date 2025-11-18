using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class FeatureModel
    {
        [Key]
        public int FeatureId { get; set; }

        [Required(ErrorMessage ="Module is Required")]
        public int? ModuleId { get; set; }

        public int? FeatureTypeId { get; set; }

        [StringLength(450)]
        [Required(ErrorMessage = "Feature Name is Required")]
        public string? FeatureName { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Path is Required")]
        public string? Path { get; set; }

        [StringLength(250)]
        public string? IconName { get; set; }

        [StringLength(250)]
        public string? Icon { get; set; }

        [StringLength(250)]
        public string? Class { get; set; }

        [StringLength(250)]
        public string? GroupTitle { get; set; }

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }

        [StringLength(450)]
        public string LastModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int? FeatureCode { get; set; }

        public int? OrderNo { get; set; } = 0;

        public bool? IsReport { get; set; }

    }
}
