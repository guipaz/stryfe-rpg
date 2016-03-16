using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network
{
    public enum OrderType
    {
        UpdateServerList,
        SetPlayerInfo,
        UpdateMapInfo,
        UpdateNPlayerInfo
    }
    
    [Serializable]
    public class SROrderMessage : SRMessage
    {
        public OrderType type;

        public SROrderMessage(OrderType type)
        {
            this.type = type;
        }
    }
}
