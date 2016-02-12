using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StryfeRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Items
{
    public enum ModifierType
    {
        Equipment, Temporary, Permanent
    }
    public class AttributeModifier
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CharacterAttribute Attribute { get; set; }
        public int Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ModifierType Type { get; set; }

        // For temporary modifiers outside battle
        public int Duration { get; set; }
        public double TimeLeft { get; set; }
    }
}
