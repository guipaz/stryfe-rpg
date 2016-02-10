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
        public AttributeSheet Attributes { get; set; }

        public FacingDirection DefaultDirection { get; set; }
        public FacingDirection Direction { get; set; }
        public CharacterSheet Sheet { get; set; }
        public int DialogId { get; set; }
        public int ScriptId { get; set; }

        public Character() { }

        // Map characters
        public Character(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            Sheet = new CharacterSheet(TextureId, tileset);
            DialogId = obj.Properties.ContainsKey("dialog") ? int.Parse(obj.Properties["dialog"]) : -1;
            ScriptId = obj.Properties.ContainsKey("script") ? int.Parse(obj.Properties["script"]) : -1;

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

        public int GetSprite()
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
        
        public void LookAt(Vector2 position)
        {
            if (MapPosition.X < position.X)
            {
                Direction = FacingDirection.Right;
            } else if (MapPosition.X > position.X)
            {
                Direction = FacingDirection.Left;
            } else if (MapPosition.Y > position.Y)
            {
                Direction = FacingDirection.Up;
            } else
            {
                Direction = FacingDirection.Down;
            }
        }

        public override void PerformAction()
        {
            if (ScriptId != -1)
            {
                LookAt(Global.Player.MapPosition);
                ScriptInterpreter.Instance.RunScript(ScriptId, this);
                return;
            }

            if (DialogId != -1)
            {
                LookAt(Global.Player.MapPosition);
                DialogManager.Instance.ActivateDialog(Global.GetDialog(DialogId), this);
            }
        }
        
        public override void Dismiss()
        {
            Direction = DefaultDirection;
        }
    }
}
