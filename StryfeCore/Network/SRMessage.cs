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
        PlayerInfo,
        PlayerId,
        Position,
        LoginUsername,
        LoginPassword,
        VisiblePlayers,
    }

    [Serializable]
    public abstract class SRMessage
    {
        public Dictionary<ArgumentName, object> args;

        public SRMessage()
        {
            args = new Dictionary<ArgumentName, object>();
        }
    }
}
