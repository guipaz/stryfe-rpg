using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network.SharedModels
{
    [Serializable]
    public class PlayerInfo
    {
        public int id;
        public string name;
        public int x;
        public int y;
    }
}
