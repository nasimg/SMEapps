using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string ModuleName { get; set; }

        public string IconName { get; set; }

        public string Icon { get; set; }

        public string Class { get; set; }

        public string GroupTitle { get; set; }

        public bool? Status { get; set; }
        [Required(ErrorMessage = "Menu Position is required")]
        public int? MenuPosition { get; set; } = 0;

        public string CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        public bool IsActive { get; set; } = true;
    }
}
