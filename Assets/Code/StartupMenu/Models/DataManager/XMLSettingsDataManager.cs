using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

namespace StartupMenu.DataManager
{
    public class XMLSettingsDataManager : SettingsDataManager
    {
        private readonly XmlSerializer _serializer;
        private readonly string _filePath;

        public XMLSettingsDataManager()
        {
            _serializer =  new XmlSerializer(typeof(SaveLoadSettingsModel));

            _filePath = Application.persistentDataPath + "/GameSettings.xml";

            if (!System.IO.File.Exists(_filePath))
            {
                IsDataStored = false;
            }
            else 
            {
                IsDataStored = true;
            }
        }

        public override SaveLoadSettingsModel LoadData()
        {
            if (!System.IO.File.Exists(_filePath))
            {
                return null;
            }

            using (XmlReader reader = XmlReader.Create(_filePath))
            {
                return (SaveLoadSettingsModel)_serializer.Deserialize(reader);
            }
        }

        public override void SaveData(SaveLoadSettingsModel data)
        {            
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                Indent = true
            };

            using (XmlWriter writer = XmlWriter.Create(_filePath, writerSettings))
            {
                _serializer.Serialize(writer, data);
            }
        }
    }
}
