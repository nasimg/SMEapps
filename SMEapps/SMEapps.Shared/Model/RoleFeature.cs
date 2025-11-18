using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class RoleFeature
    {
        public int RoleFeatureId { get; set; }
        public string RoleId { get; set; }
        public string FeatureName { get; set; }
        public int FeatureKey { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanReport { get; set; }
        public Feature FeatureKeyNavigation { get; set; }
    }

}
