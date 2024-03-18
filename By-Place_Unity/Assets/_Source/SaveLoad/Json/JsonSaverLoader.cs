using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SaveLoad.Load;
using SaveLoad.Save;
using Application = UnityEngine.Application;

namespace SaveLoad.Json
{
    public class JsonSaverLoader : ISaverLoader
    {
        private readonly string _masterPath = Application.persistentDataPath; 
        
        public bool Save<T>(ISavable<T> savable)
        {
            var fullPath = Path.Join(_masterPath, $"{savable.Path}.json");
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            var saveData = savable.GetSaveData();

            using var file = File.CreateText(fullPath);
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new BinaryConverter());
                serializer.Serialize(file, saveData);
            }

            return true;
        }

        public bool Load<T>(ILoadable<T> loadable)
        {
            var fullPath = Path.Join(_masterPath, $"{loadable.Path}.json");

            if (!File.Exists(fullPath))
                return false;

            T saveData;
            using (var file = File.OpenText(fullPath))
            {
                var serializer = new JsonSerializer();
                saveData = (T)serializer.Deserialize(file, typeof(T));
            }

            loadable.LoadSaveData(saveData);
            return true;
        }
    }
}