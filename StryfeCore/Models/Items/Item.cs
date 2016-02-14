using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StryfeCore.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Items
{
    public enum ItemType
    {
        Usable, Quest, Misc, Equipment
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Weight { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType Type { get; set; }
        public string Description { get; set; }

        public List<AttributeModifier> Modifiers { get; set; }
        public int ScriptId { get; set; } // Activated when used (usable), equipped (armor, weapon) or gathered (misc, quest)

        // Graphic stuff
        public string TextureName { get; set; }
        public int Gid { get; set; }
        public int TextureTileSize { get; set; }

        public Item()
        {
            Modifiers = new List<AttributeModifier>();
            ScriptId = -1;
            TextureTileSize = 34;
            Gid = -1;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Id, Name);
        }

        public Item Clone()
        {
            Item i = null;
            if (Type == ItemType.Equipment)
                i = new Equipment();
            else
                i = new Item();

            i.Id = Id;
            i.Name = Name;
            i.Price = Price;
            i.Weight = Weight;
            i.Type = Type;
            i.Description = Description;
            i.Modifiers = new List<AttributeModifier>();
            foreach (AttributeModifier o in Modifiers)
                i.Modifiers.Add(o);
            i.ScriptId = ScriptId;
            i.TextureName = TextureName;
            i.Gid = Gid;
            i.TextureTileSize = TextureTileSize;

            return i;
        }
    }
}
