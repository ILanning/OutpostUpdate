using System.IO;
using System.Xml.Serialization;

namespace Outpost
{
    public abstract class Serializable<T>
    {
        static public T Read(string filePath)
        {
            T newObject;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StreamReader streamReader = new StreamReader(filePath);
            newObject = (T)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            return newObject;
        }

        static public void Write(object toBeSerialized, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toBeSerialized.GetType());
            StreamWriter streamWriter = new StreamWriter(File.Create(filePath));
            xmlSerializer.Serialize(streamWriter, toBeSerialized);
            streamWriter.Close();
        }
    }
}
