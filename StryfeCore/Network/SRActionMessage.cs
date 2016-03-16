using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network
{
    public enum ActionType
    {
        Login,
        GetMapData,
        MoveTo
    }

    public enum ServiceType
    {
        Login,
        Char,
        Map
    }

    [Serializable]
    public class SRActionMessage : SRMessage
    {
        public ActionType type;
        public ServiceType serviceType;

        public SRActionMessage (ActionType type, ServiceType serviceType)
        {
            this.type = type;
            this.serviceType = serviceType;
        }
    }
}
