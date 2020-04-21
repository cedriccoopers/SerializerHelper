using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SerializerHelper
{
    public static class SerializerHelper
    {
        public static String XMLSerializeObject(Object pObject, Type type)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var memoryStream = new MemoryStream();
            var xs = new XmlSerializer(type);
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, pObject, ns);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            var xmlString = Utf8ByteArrayToString(memoryStream.ToArray());

            if (xmlString.Substring(0, 1).Equals("<"))
                xmlString = xmlString.Substring(1);

            return xmlString;
        }

        public static Object XMLDeserializeObject(String stringObject, Type type)
        {
            var xs = new XmlSerializer(type);
            var memoryStream = new MemoryStream(StringToUtf8ByteArray(stringObject));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            return xs.Deserialize(memoryStream);
        }

        public static String JSONSerializeObject(Object pObject, Type type)
        {
            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            ser.WriteObject(ms, pObject);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static Object JSONDeserializeObject(String pJsonString, Type type)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pJsonString));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);

            object deserializeObject = ser.ReadObject(ms);
            ms.Close();
            return deserializeObject;
        }

        private static String Utf8ByteArrayToString(Byte[] charaters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(charaters);
            return (constructedString);
        }

        private static Byte[] StringToUtf8ByteArray(String xmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }
    }
}
