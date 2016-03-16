using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network.Orders
{
    [Serializable]
    public class SOrderMovePlayer : SOrder
    {
        public int id;
        public int x;
        public int y;
    }
}
