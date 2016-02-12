using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StryfeRPG.Models.Characters;
using StryfeRPG.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Items
{
    public class UsableItem : Item
    {
        public List<AttributeModifier> Modifiers { get; set; }

        public UsableItem() : base()
        {
            Type = ItemType.Usable;
        }
    }
}
