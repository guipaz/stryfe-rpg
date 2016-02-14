using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StryfeCore.Models.Items;
using StryfeRPG.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.System.Serializers
{
    public class ItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Item).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            object target = null;
            if (item["Type"].Value<string>() == "Equipment")
            {
                target = new Equipment();
            }
            else
            {
                target = new Item();
            }

            serializer.Populate(item.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
