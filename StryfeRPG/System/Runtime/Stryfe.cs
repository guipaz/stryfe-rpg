using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledSharp;
using StryfeRPG.Managers;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;
using StryfeRPG.Models.Utils;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using StryfeRPG.Managers.Data;

namespace StryfeRPG
{
    public class Stryfe : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Stryfe()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            Global.SetContent(Content);
        }

        protected override void Initialize()
        {
            CameraManager.Instance.Bounds = GraphicsDevice.Viewport.Bounds;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.0f; // debug

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Global.MapFont = Content.Load<SpriteFont>("Fonts/DialogFont"); //TODO: get this straight
            Global.DialogFont = Content.Load<SpriteFont>("Fonts/DialogFont");
            Global.DetailFont = Content.Load<SpriteFont>("Fonts/DetailFont");
            Global.Viewport = GraphicsDevice.Viewport;

            Utils.LoadDialogs();
            Utils.LoadScripts();
            Utils.LoadItems();

            MapManager.Instance.spriteBatch = spriteBatch;
            MapManager.Instance.LoadMap("testMap");

            //testing
            Global.RetrieveAllItems();
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

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

            // Windows
            //TODO: change do WindowManager
            InventoryManager.Instance.Draw(spriteBatch, timePassed);
            EquipmentManager.Instance.Draw(spriteBatch, timePassed);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
