using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network
{
    public enum ArgumentName
    {
        ServerList,
        PlayerPosition,
        PlayerName,
        LoginUsername,
        LoginPassword,
    }

    [Serializable]
    public abstract class SRMessage
    {
        public Dictionary<ArgumentName, object> args;
    }
}
