using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp {
    
    public static class SaveManager {

        public static List<Save> Saves;

        public static List<string> GetSavesList() {
            if (Saves == null) LoadSaves();
            List<string> saveNames = new List<string>();
            if (Saves != null && Saves.Count > 0) saveNames = Saves.Select(save => save.SaveName).ToList();
            saveNames.Reverse();
            return saveNames;
        }

        public static Save GetSave(string saveName) {
            if (Saves == null) LoadSaves();
            return Saves.FirstOrDefault(save => save.SaveName == saveName).DeepClone(); // Data.DeepClone(obj)
        }

        public static Save GetSaveReference(string saveName) {
            if (Saves == null) LoadSaves();
            return Saves.FirstOrDefault(save => save.SaveName == saveName);
        }

        public static void SaveGame(Save save) {
            if (Saves == null) LoadSaves();
            Saves.Add(save);
            SaveChanges();
        }

        public static void DeleteSavedGame(Save save) {
            if (Saves == null) LoadSaves();
            Saves.Remove(save);
            SaveChanges();
        }
        
        public static void LoadSaves() {
            Saves = new List<Save>();
            string path = $"{Data.Path}saves.json";
            StreamReader saveFileReader = new StreamReader(path);
            string jsonString = saveFileReader.ReadToEnd();
            Save[] savesArray = JsonSerializer.Deserialize<Save[]>(jsonString);
            Saves = savesArray.ToList();
            saveFileReader.Close();
        }

        private static void SaveChanges() {
            string path = $"{Data.Path}saves.json";
            StreamWriter saveFileWriter = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(Saves, Data.JsonOptions);
            saveFileWriter.Write(jsonString);
            saveFileWriter.Close();
        }

        public static bool ContainsThisName(string name) {
            if (Saves == null) LoadSaves();
            return Saves.Any(save => save.SaveName == name);
        }
    }
}