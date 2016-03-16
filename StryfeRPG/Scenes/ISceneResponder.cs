using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Scenes
{
    public interface ISceneResponder
    {
        void Exit();
        void ChangeScene(Scene.SceneType type);
    }
}
