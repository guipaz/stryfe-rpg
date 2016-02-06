using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledSharp;
using StryfeRPG.Managers;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;

namespace StryfeRPG
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.SetContent(Content);
        }

        protected override void Initialize()
        {
            CameraManager.Instance.Bounds = GraphicsDevice.Viewport.Bounds;
            CameraManager.Instance.Zoom = 2;
        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MapManager.Instance.spriteBatch = spriteBatch;
            MapManager.Instance.LoadMap("exampleMap");

            Player.Instance.texture = Global.GetTexture("charsets");
            Player.Instance.textureId = 1;
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardManager.Instance.Update(gameTime.ElapsedGameTime.TotalSeconds);
            MapManager.Instance.Update(gameTime.ElapsedGameTime.TotalSeconds);

            Console.WriteLine(Player.Instance.mapPosition);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CameraManager.Instance.Position = Player.Instance.currentPosition;

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: CameraManager.Instance.TransformMatrix);
            MapManager.Instance.Draw(gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
