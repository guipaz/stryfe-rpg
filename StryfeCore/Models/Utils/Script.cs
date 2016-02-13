using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Models.Utils
{
    public class Script
    {
        public int Id { get; set; }
        public List<ScriptCommand> Commands { get; set; }
    }
}
