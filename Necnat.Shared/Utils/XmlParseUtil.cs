using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Necnat.Shared.Utils
{
    public static class XmlParseUtil
    {
        public static Stream ToStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static T ParseXML<T>(string s) where T : class
        {
            var reader = XmlReader.Create(ToStream(s.Trim()), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
    }
}
