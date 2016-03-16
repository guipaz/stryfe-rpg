using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Network
{
    public interface IMessageHandler
    {
        void Handle(SROrderMessage msg);
    }
}
