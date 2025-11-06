using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class Feature
    {
        [Key]
        public int FeatureId { get; set; }

        public int? ModuleId { get; set; }

        public int? FeatureTypeId { get; set; }

        [StringLength(450)]
        public string? FeatureName { get; set; }

        [StringLength(500)]
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

        public bool IsActive { get; set; }

        public int? FeatureCode { get; set; }

        public int? OrderNo { get; set; }

        public bool? IsReport { get; set; }

    }
}
