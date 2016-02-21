using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Models.Utils
{
    public class QuickMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public QuickMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
