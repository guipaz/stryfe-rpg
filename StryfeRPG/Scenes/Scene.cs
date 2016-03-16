using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Scenes
{
    public abstract class Scene
    {
        public enum SceneType
        {
            Login,
            Game
        }

        protected SpriteBatch spriteBatch;
        protected ISceneResponder responder;
        protected GraphicsDevice device;

        public Scene(SpriteBatch sb, ISceneResponder r)
        {
            spriteBatch = sb;
            responder = r;
        }

        public abstract void LoadScene();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
