using Microsoft.Xna.Framework;
using StryfeRPG.Managers;
using StryfeRPG.Models.Maps;
using StryfeRPG.Models.Utils;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Characters
{
    public enum FacingDirection
    {
        Down, Left, Right, Up
    }

    public class Character : Maps.MapObject
    {
        public FacingDirection direction { get; set; }
        public CharacterSheet sheet { get; set; }

        public Character() { }
        public Character(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            Console.WriteLine(String.Format("NPC: {0}", obj.Name));
            sheet = new CharacterSheet(textureId, tileset);

            if (textureId == sheet.gidUp)
                direction = FacingDirection.Up;
            else if (textureId == sheet.gidLeft)
                direction = FacingDirection.Left;
            else if (textureId == sheet.gidRight)
                direction = FacingDirection.Right;
            else
                direction = FacingDirection.Down;
        }

        public int GetSprite()
        {
            if (sheet == null)
                return textureId;

            switch (direction)
            {
                case FacingDirection.Up:
                    return sheet.gidUp;
                case FacingDirection.Down:
                    return sheet.gidDown;
                case FacingDirection.Left:
                    return sheet.gidLeft;
                case FacingDirection.Right:
                    return sheet.gidRight;
            }

            return textureId;
        }
        
        public void LookAt(Vector2 position)
        {
            if (mapPosition.X < position.X)
            {
                direction = FacingDirection.Right;
            } else if (mapPosition.X > position.X)
            {
                direction = FacingDirection.Left;
            } else if (mapPosition.Y > position.Y)
            {
                direction = FacingDirection.Up;
            } else
            {
                direction = FacingDirection.Down;
            }
        }

        public override void PerformAction()
        {
            LookAt(Global.Player.mapPosition);

            Dialog dialog = new Dialog();
            List<string> messages = new List<string>();
            messages.Add("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed est lectus, consequat nec blandit eget, " +
                "rhoncus non ipsum. Aenean maximus venenatis convallis. Aliquam a dolor tortor. " +
                "Donec vulputate, metus non pellentesque consequat, sem ipsum posuere justo, in rutrum mi ex vel leo. Pellentesque faucibus elit eu justo egestas, et viverra turpis pulvinar.");
            messages.Add("The mouse location and button clicks are kept up to date in your XNA game.");
            dialog.messages = messages;
            DialogManager.Instance.ActivateDialog(dialog);
        }
    }
}
