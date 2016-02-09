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
        public FacingDirection defaultDirection { get; set; }
        public FacingDirection direction { get; set; }
        public CharacterSheet sheet { get; set; }
        public int dialogId { get; set; }

        public Character() { }
        public Character(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            Console.WriteLine(String.Format("NPC: {0}", obj.Name));
            sheet = new CharacterSheet(textureId, tileset);
            dialogId = obj.Properties.ContainsKey("dialog") ? int.Parse(obj.Properties["dialog"]) : -1;

            if (textureId == sheet.gidUp)
                direction = FacingDirection.Up;
            else if (textureId == sheet.gidLeft)
                direction = FacingDirection.Left;
            else if (textureId == sheet.gidRight)
                direction = FacingDirection.Right;
            else
                direction = FacingDirection.Down;

            defaultDirection = direction;
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
            if (dialogId == -1)
                return;

            LookAt(Global.Player.mapPosition);
            DialogManager.Instance.ActivateDialog(Global.GetDialog(dialogId), this);
        }
        
        public override void Dismiss()
        {
            direction = defaultDirection;
        }
    }
}
