using Microsoft.Xna.Framework;
using StryfeRPG.Managers;
using StryfeRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Maps
{
    public class Teleport : MapObject
    {
        public string Map { get; set; }
        public Vector2 TeleportPosition { get; set; }
        public FacingDirection Direction { get; set; }

        public Teleport(TmxObject obj, Tileset tileset) : base(obj, tileset)
        {
            string[] property = obj.Properties["teleport"].Split(',');
            Map = property[0];
            TeleportPosition = new Vector2(int.Parse(property[1]), int.Parse(property[2]));

            switch (property[3])
            {
                case "up":
                    Direction = FacingDirection.Up;
                    break;
                case "down":
                    Direction = FacingDirection.Down;
                    break;
                case "left":
                    Direction = FacingDirection.Left;
                    break;
                case "right":
                    Direction = FacingDirection.Right;
                    break;
            }
        }

        public override void PerformAction()
        {
            MapManager.Instance.Teleport(this);
        }
    }
}
