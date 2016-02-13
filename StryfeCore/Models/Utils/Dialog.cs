using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Utils
{
    public class Dialog
    {
        public int Id { get; set; }
        public string CharacterName { get; set; }
        public List<string> Messages { get; set; }

        public Dialog() { }
        public Dialog(string name, List<string> messages)
        {
            CharacterName = name;
            Messages = messages;
        }
    }
}
