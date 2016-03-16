using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StryfeRPG.Managers;
using StryfeRPG.Managers.Data;
using StryfeRPG.Managers.GUI;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Scenes
{
    public class GameScene : Scene
    {
        public GameScene(SpriteBatch spriteBatch, ISceneResponder responder) : base(spriteBatch, responder)
        {
            this.spriteBatch = spriteBatch;
            this.responder = responder;
        }

        public override void LoadScene()
        {
            MapManager.Instance.spriteBatch = spriteBatch;
            MapManager.Instance.LoadMap("testMap");
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                responder.Exit();

            double timePassed = gameTime.ElapsedGameTime.TotalSeconds;

            // If game is waiting, doesn't listen to any keyboard event
            PauseManager.Instance.Update(timePassed);
            if (PauseManager.Instance.Waiting)
                return;

            KeyboardManager.Instance.Update(timePassed);

            // If game is paused, doesn't update the map
            if (PauseManager.Instance.Paused)
                return;

            MapManager.Instance.Update(timePassed);
            QuickMessageManager.Instance.Update(timePassed);
        }

        public override void Draw(GameTime gameTime)
        {
            CameraManager.Instance.Position = Global.Player.CurrentPosition;
            double timePassed = gameTime.ElapsedGameTime.TotalSeconds;

            // Maps (and everything in them)
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: CameraManager.Instance.TransformMatrix);
            MapManager.Instance.Draw(timePassed);
            spriteBatch.End();

            // HUD
            spriteBatch.Begin();
            HUDManager.Instance.Draw(spriteBatch);

            // Dialogs
            DialogManager.Instance.Draw(spriteBatch, timePassed);

            // Quick Messages
            QuickMessageManager.Instance.Draw(spriteBatch);

            // Windows
            if (WindowManager.IsWindowOpened)
                WindowManager.CurrentManager.Draw(spriteBatch, timePassed);

            spriteBatch.End();
        }
    }
}
