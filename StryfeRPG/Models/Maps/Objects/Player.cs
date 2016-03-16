using Microsoft.Xna.Framework;
using StryfeCore.Network;
using StryfeCore.Network.SharedModels;
using StryfeRPG.Models.Items;
using StryfeRPG.Models.Maps;
using StryfeRPG.System;
using StryfeRPG.System.Network;
using StryfeRPG.System.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace StryfeRPG.Models.Characters
{
    public class Player : NPC
    {
        public int id;
        public PlayerInfo info { get; set; }

        public Player(TmxObject obj, Tileset tileset) : base(obj, tileset, "")
        {
            Name = "Stryfe";
            Attributes = new AttributeSheet();
        }

        public void SetInfo(PlayerInfo info)
        {
            this.info = info;
            Global.Player.MapPosition = new Vector2(info.x, info.y);
            Global.Player.Name = info.name;
        }

        public override bool Move(Vector2 pos)
        {
            Vector2 newPos = MapPosition + pos;
            info.x = (int)newPos.X;
            info.y = (int)newPos.Y;

            ClientHandler.Instance.GetHandler().Send(ActionType.UpdatePosition, new Dictionary<ArgumentName, object>() { { ArgumentName.PlayerInfo, info } });

            return base.Move(pos);
        }
    }
}
