using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StryfeRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Managers
{
    public class KeyboardManager
    {
        double currentCooldown = 0;
        double actionsPerSecond = 5;

        public void Update(double timePassed)
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= timePassed;
                return;
            }

            KeyboardState state = Keyboard.GetState();

            int moveX = 0;
            int moveY = 0;

            if (state.IsKeyDown(Keys.Up))
                moveY--;
            else if (state.IsKeyDown(Keys.Right))
                moveX++;
            else if (state.IsKeyDown(Keys.Down))
                moveY++;
            else if (state.IsKeyDown(Keys.Left))
                moveX--;

            if (moveX != 0 || moveY != 0)
            {
                Vector2 movement = new Vector2(moveX, moveY);
                if (!CollisionManager.Instance.GetCollision(Player.Instance.mapPosition + movement))
                {
                    Player.Instance.Move(movement);
                    currentCooldown = 1 / actionsPerSecond;
                }
            }
        }

        // Singleton stuff
        private static KeyboardManager instance;
        protected KeyboardManager() { }
        public static KeyboardManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KeyboardManager();
                }
                return instance;
            }
        }
    }
}
