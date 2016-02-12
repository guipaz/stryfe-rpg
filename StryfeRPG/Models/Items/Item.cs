using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Items
{
    public enum ItemType
    {
        Usable, Quest, Misc, Weapon, Armor
    }

    public class Item
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public ItemType Type { get; set; }
        public string Description { get; set; }
        public int ScriptId { get; set; }
    }
}
