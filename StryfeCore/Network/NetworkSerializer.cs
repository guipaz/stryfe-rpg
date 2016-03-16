using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StryfeCore.Network
{
    public class NetworkSerializer
    {
        public static byte[] SerializeObject(object pObjectToSerialize)
        {
            BinaryFormatter lFormatter = new BinaryFormatter();
            MemoryStream lStream = new MemoryStream();
            lFormatter.Serialize(lStream, pObjectToSerialize);
            byte[] lRet = new byte[lStream.Length];
            lStream.Position = 0;
            lStream.Read(lRet, 0, (int)lStream.Length);
            lStream.Close();
            return lRet;
        }

        public static T DeserializeObject<T>(byte[] pData)
        {
            if (pData == null)
                return default(T);
            BinaryFormatter lFormatter = new BinaryFormatter();
            MemoryStream lStream = new MemoryStream(pData);
            object lRet = lFormatter.Deserialize(lStream);
            lStream.Close();
            return (T)lRet;
        }
    }
}
