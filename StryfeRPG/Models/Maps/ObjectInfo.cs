using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StryfeRPG.Models.Maps
{
    public class ObjectInfo
    {
        public string ObjectIdentifier { get; set; } // The identifier of the object (id_name_map)
        public int NumberOfInteractions { get; set; } // Number of times the player interacted with it
        public bool IsActive { get; set; } // If the object is active in the map

        public ObjectInfo(string id)
        {
            ObjectIdentifier = id;
            NumberOfInteractions = 0;
            IsActive = true;
        }
    }
}
