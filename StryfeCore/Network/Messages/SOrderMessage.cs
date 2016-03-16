using StryfeCore.Network.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network.Messages
{
    public enum OrderType
    {
        MovePlayer,
        PlayerInformation
    }

    [Serializable]
    public class SOrderMessage : SMessage
    {
        public OrderType type;
        public SOrder order;

        public SOrderMessage() { }
        public SOrderMessage(OrderType type, SOrder order)
        {
            this.type = type;
            this.order = order;
        }
    }
}
