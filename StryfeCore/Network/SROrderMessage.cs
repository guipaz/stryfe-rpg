using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network
{
    public enum OrderType
    {
        // Login
        UpdateServerList,
        SetPlayerInfo,
        UpdateMapInfo,

        // Movement
        UpdateNPlayerInfo,

        // Failure
        LoginFailed
    }
    
    [Serializable]
    public class SROrderMessage : SRMessage
    {
        public OrderType type;

        public SROrderMessage(OrderType type, Dictionary<ArgumentName, object> args = null) : base()
        {
            this.type = type;
            this.args = args != null ? args : this.args;
        }
    }
}
