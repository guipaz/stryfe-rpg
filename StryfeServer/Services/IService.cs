using Lidgren.Network;
using StryfeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeServer.Services
{
    public interface IService
    {
        void Handle(SRActionMessage msg, NetConnection conn);
    }
}
