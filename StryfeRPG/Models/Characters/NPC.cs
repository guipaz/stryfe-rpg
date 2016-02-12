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

    public class NPC : Maps.MapObject
    {
        // Attributes
        public AttributeSheet Attributes { get; set; }
        
        // Sprite direction
        public FacingDirection DefaultDirection { get; set; }
        public FacingDirection Direction { get; set; }

        // Sprite sheet
        public SpriteSheet Sheet { get; set; }

        // Dialog and script ids
        public int DialogId { get; set; }
        
        public NPC(TmxObject obj, Tileset tileset, string mapName) : base(obj, tileset, mapName)
        {
            Sheet = new SpriteSheet(TextureId, tileset);
            DialogId = obj.Properties.ContainsKey("dialog") ? int.Parse(obj.Properties["dialog"]) : -1;

            if (TextureId == Sheet.GidUp)
                Direction = FacingDirection.Up;
            else if (TextureId == Sheet.GidLeft)
                Direction = FacingDirection.Left;
            else if (TextureId == Sheet.GidRight)
                Direction = FacingDirection.Right;
            else
                Direction = FacingDirection.Down;

            DefaultDirection = Direction;
        }

        public void LookAt(Vector2 position)
        {
            if (MapPosition.X < position.X)
            {
                Direction = FacingDirection.Right;
            }
            else if (MapPosition.X > position.X)
            {
                Direction = FacingDirection.Left;
            }
            else if (MapPosition.Y > position.Y)
            {
                Direction = FacingDirection.Up;
            }
            else
            {
                Direction = FacingDirection.Down;
            }
        }

        public override int GetSprite()
        {
            if (Sheet == null)
                return TextureId;

            switch (Direction)
            {
                case FacingDirection.Up:
                    return Sheet.GidUp;
                case FacingDirection.Down:
                    return Sheet.GidDown;
                case FacingDirection.Left:
                    return Sheet.GidLeft;
                case FacingDirection.Right:
                    return Sheet.GidRight;
            }

            return TextureId;
        }
        
        public override void PerformAction()
        {
            if (ScriptId != -1 || DialogId != -1)
            {
                LookAt(Global.Player.MapPosition);
            }

            base.PerformAction();

            if (DialogId != -1 && ScriptId != -1)
            {
                DialogManager.Instance.ActivateDialog(Global.GetDialog(DialogId), this);
            }
        }
        
        public override void Dismiss()
        {
            Direction = DefaultDirection;
        }
    }
}
