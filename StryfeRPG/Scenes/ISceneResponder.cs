using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Scenes
{
    public interface ISceneResponder
    {
        void ChangeScene(Scene.SceneType type);
        void Exit();
        GraphicsDevice GetGraphicsDevice();
        GraphicsDeviceManager GetGraphicsManager();
    }
}
