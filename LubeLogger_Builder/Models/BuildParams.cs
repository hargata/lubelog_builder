using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LubeLogger_Builder.Models
{
    public class BuildParams
    {
        public string SourceFolder { get; set; } = "";
        public List<string> TargetArchs { get; set;} = new List<string>();
        public bool BuildSelfContained { get; set; } = true;
    }
}
