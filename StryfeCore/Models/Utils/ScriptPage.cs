using StryfeRPG.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Models.Utils
{
    public class ScriptPage
    {
        public string Switch { get; set; }
        public string Condition { get; set; }
        public string Arguments { get; set; }
        public List<ScriptCommand> Commands { get; set; }
    }
}
