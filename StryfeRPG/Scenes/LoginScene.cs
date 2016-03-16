using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeRPG.System.Network;

namespace StryfeRPG.Scenes
{
    public class LoginScene : Scene
    {
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void LoadScene()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        void SendLogin()
        {
            SRActionMessage msg = new SRActionMessage(ActionType.Login, ServiceType.Login);
            msg.args = new Dictionary<ArgumentName, object>()
            {
                { ArgumentName.LoginUsername, "stryfe" },
                { ArgumentName.LoginPassword, "123" }
            };
            ClientHandler.Instance.SendMessage(msg);
        }
    }
}
