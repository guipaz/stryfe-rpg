using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeRPG.System.Network;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StryfeRPG.System.Forms;
using StryfeRPG.System;

namespace StryfeRPG.Scenes
{
    public class LoginScene : Scene
    {
        SButton loginButton;

        bool locked = false;

        public LoginScene(SpriteBatch sb, ISceneResponder s) : base (sb, s)
        {

        }

        public override void LoadScene()
        {
            device = responder.GetGraphicsDevice();
            FormUtils.device = device;

            int width = device.Viewport.Width;
            int height = device.Viewport.Height;

            loginButton = new SButton(new Rectangle(width / 2 - 100, height / 2 - 30, 200, 60), Color.CadetBlue, "Login");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            responder.GetGraphicsDevice().Clear(Color.Beige);

            loginButton.Draw(spriteBatch);

            spriteBatch.End();
        }
        
        public override void Update(GameTime gameTime)
        {
            if (locked)
                return;

            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                if (loginButton.IsInside(state.Position))
                {
                    SendLogin();
                    locked = true;
                }
            }
        }

        void SendLogin()
        {
            SRActionMessage msg = new SRActionMessage(ActionType.DoLogin, ServiceType.Login);
            msg.args = new Dictionary<ArgumentName, object>()
            {
                { ArgumentName.LoginUsername, "stryfe" },
                { ArgumentName.LoginPassword, "123" }
            };
            ClientHandler.Instance.SendMessage(msg);
        }
    }
}
