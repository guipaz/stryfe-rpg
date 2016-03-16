using Lidgren.Network;
using StryfeCore.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StryfeServer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread t = new Thread(ServerHandler.Instance.Run);
            t.Start();

            while (t.IsAlive)
            {
                Thread.Sleep(1);
            }
        }
    }
}
