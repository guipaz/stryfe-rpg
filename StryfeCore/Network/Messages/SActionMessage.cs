using StryfeCore.Network.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network.Messages
{
    public enum ActionType
    {
        MapLoaded,
        Login,
        Movement
    }

    [Serializable]
    public class SActionMessage : SMessage
    {
        public ActionType type;
        public SAction action;

        public SActionMessage() { }
        public SActionMessage(ActionType type, SAction action)
        {
            this.type = type;
            this.action = action;
        }
    }
}
