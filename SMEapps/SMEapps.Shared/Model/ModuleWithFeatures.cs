using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model;

public class ModuleWithFeatures
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int? MenuPosition { get; set; }
    public List<FeatureModel> Features { get; set; } = new();
}
