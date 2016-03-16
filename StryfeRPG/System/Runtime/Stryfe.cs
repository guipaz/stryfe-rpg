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
using MonoGame.Extended.BitmapFonts;
using StryfeRPG.Managers.GUI;
using Lidgren.Network;
using System.Threading;
using StryfeRPG.System.Network;
using StryfeCore.Network;
using StryfeRPG.Scenes;

namespace StryfeRPG
{
    public class Stryfe : Game, ISceneResponder
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene currentScene;

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

            new Thread(ClientHandler.Instance.Run).Start();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Global.MapFont = Content.Load<SpriteFont>("Fonts/DetailFont"); //TODO: get this straight
            Global.DialogFont = Content.Load<SpriteFont>("Fonts/DialogFont");
            Global.DetailFont = Content.Load<SpriteFont>("Fonts/DetailFont");

            Global.Viewport = GraphicsDevice.Viewport;

            Utils.LoadDialogs();
            Utils.LoadScripts();
            Utils.LoadItems();

            //ChangeScene(Scene.SceneType.Game);
            ChangeScene(Scene.SceneType.Login);
        }
        
        protected override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            currentScene.Draw(gameTime);   
            
            base.Draw(gameTime);
        }

        public void ChangeScene(Scene.SceneType type)
        {
            currentScene = new GameScene(spriteBatch, this);
            currentScene.LoadScene();
        }

        protected override void UnloadContent()
        {
            ClientHandler.Instance.Stop();
        }
    }
}
