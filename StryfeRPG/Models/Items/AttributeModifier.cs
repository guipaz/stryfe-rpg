using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StryfeRPG.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StryfeRPG.Models.Items
{
    public class AttributeModifier
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CharacterAttribute Attribute { get; set; }
        public int Value { get; set; }
        public string Type { get; set; }
    }
}
