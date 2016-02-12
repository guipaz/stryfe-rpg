using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;
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

        bool didPressControl = false;

        public void Update(double timePassed)
        {
            KeyboardState state = Keyboard.GetState();

            // Control management
            if (state.IsKeyDown(Keys.LeftControl) && !didPressControl)
            {
                didPressControl = true;
            }
            else if (!state.IsKeyDown(Keys.LeftControl) && didPressControl)
            {
                didPressControl = false;

                // Dialog action
                if (DialogManager.Instance.IsDialogActive())
                {
                    DialogManager.Instance.NextMessage();
                    return;
                }

                // Map action

                Vector2 position = Global.Player.MapPosition;
                Vector2 direction = Vector2.Zero;
                switch (Global.Player.Direction)
                {
                    case FacingDirection.Up:
                        direction = new Vector2(0, -1);
                        break;
                    case FacingDirection.Down:
                        direction = new Vector2(0, 1);
                        break;
                    case FacingDirection.Left:
                        direction = new Vector2(-1, 0);
                        break;
                    case FacingDirection.Right:
                        direction = new Vector2(1, 0);
                        break;
                }

                MapManager.Instance.PerformAction(position + direction);
            }

            // If there's a dialog active, does nothing other than press Control
            if (DialogManager.Instance.IsDialogActive())
                return;

            // Movement cooldown
                if (currentCooldown > 0)
            {
                currentCooldown -= timePassed;
                return;
            }

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
                // Change direction
                if (moveX > 0)
                    Global.Player.Direction = FacingDirection.Right;
                else if (moveX < 0)
                    Global.Player.Direction = FacingDirection.Left;
                else if (moveY > 0)
                    Global.Player.Direction = FacingDirection.Down;
                else if (moveY < 0)
                    Global.Player.Direction = FacingDirection.Up;

                // Make movement
                Vector2 movement = new Vector2(moveX, moveY);
                if (!MapManager.Instance.GetCollision(Global.Player.MapPosition + movement))
                {
                    Global.Player.Move(movement);
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
