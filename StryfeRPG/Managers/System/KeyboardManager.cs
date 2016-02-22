using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StryfeRPG.Managers.Data;
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

        double hudCooldown = 0;
        double hudActionsPerSecond = 5;

        private Keys ActionButton = Keys.Z;
        private Keys CancelButton = Keys.X;
        private Keys InventoryButton = Keys.E;
        private Keys CharacterButton = Keys.C;
        private Keys EquipmentButton = Keys.W;

        //TODO: use an array of "key released"
        KeyboardState currentState;
        KeyboardState lastFrameState;
        KeyboardState oldState;

        public void Update(double timePassed)
        {
            currentState = Keyboard.GetState();
            lastFrameState = oldState;
            oldState = currentState; // for the next frame
            
            // Action management
            if (IsKeyReleased(ActionButton))
            {
                // Dialog action
                if (DialogManager.Instance.IsDialogActive())
                {
                    DialogManager.Instance.NextMessage();
                    return;
                } else if (WindowManager.IsWindowOpened)
                {
                    WindowManager.CurrentManager.PerformAction();
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

            // Cancel management
            if (IsKeyReleased(CancelButton))
            {
                if (DialogManager.Instance.IsDialogActive())
                    DialogManager.Instance.SkipMessage();
                else if (WindowManager.IsWindowOpened)
                    WindowManager.CurrentManager.CloseWindow();

                return;
            }

            // If there's a dialog active, does nothing other than press Control
            if (DialogManager.Instance.IsDialogActive())
                return;

            // Windows buttons
            if (IsKeyReleased(InventoryButton))
            {
                InventoryManager.Instance.ToggleWindow();
            }
            else if (IsKeyReleased(EquipmentButton))
            {
                EquipmentManager.Instance.ToggleWindow();
            } else if (IsKeyReleased(CharacterButton))
            {
                CharacterManager.Instance.ToggleWindow();
            }

            // Movement cooldown
            if (!WindowManager.IsWindowOpened && currentCooldown > 0)
            {
                currentCooldown -= timePassed;
                return;
            } else if (WindowManager.IsWindowOpened && hudCooldown > 0)
            {
                hudCooldown -= timePassed;
                return;
            }

            int moveX = 0;
            int moveY = 0;

            if (currentState.IsKeyDown(Keys.Up))
                moveY--;
            else if (currentState.IsKeyDown(Keys.Right))
                moveX++;
            else if (currentState.IsKeyDown(Keys.Down))
                moveY++;
            else if (currentState.IsKeyDown(Keys.Left))
                moveX--;

            if (moveX != 0 || moveY != 0)
            {
                Vector2 movement = new Vector2(moveX, moveY);

                // If inventory is opened, send the movement to it
                if (WindowManager.IsWindowOpened)
                {
                    WindowManager.CurrentManager.Move(movement);

                    hudCooldown = 1 / hudActionsPerSecond;
                    return;
                }

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
                if (!MapManager.Instance.GetCollision(Global.Player.MapPosition + movement))
                {
                    Global.Player.Move(movement);
                    currentCooldown = 1 / actionsPerSecond;
                }
            }
        }

        private bool IsKeyReleased(Keys key)
        {
            return currentState.IsKeyUp(key) && lastFrameState.IsKeyDown(key);
        }

        // Singleton stuff
        private static KeyboardManager instance;
        protected KeyboardManager()
        {
            currentState = new KeyboardState();
            lastFrameState = new KeyboardState();
            oldState = new KeyboardState();
        }

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
