using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class Feature
    {
        public int FeatureId { get; set; }
        public int ModuleId { get; set; }
        public string FeatureName { get; set; }
        public string Path { get; set; }
    }
}
