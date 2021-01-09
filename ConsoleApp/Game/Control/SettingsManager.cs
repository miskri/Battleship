using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleApp.Data;
using ConsoleApp.Objects;
using ConsoleApp.Utils;

namespace ConsoleApp.Control {
    
    public class SettingsManager {
        
        public Settings UserSettings { get; private set; }
        public Settings DefaultSettings { get; private set; }
        private void SaveUserSettings(Settings settings) {
            UserSettings = settings;
            MenuDataContainer.UserSettings = UserSettings;
            
            string path = $"{DataUtils.Path}Resources/user_settings.json";
            StreamWriter fileUserSettings = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(settings, DataUtils.JsonOptions);
            
            fileUserSettings.Write(jsonString);
            fileUserSettings.Close();
        }
        
        public void LoadSettings() {
            // load default settings
            string pathDefaultSettings = $"{DataUtils.Path}Resources/default_settings.json";
            StreamReader fileDefaultSettings = new StreamReader(pathDefaultSettings);
            
            string jsonString = fileDefaultSettings.ReadToEnd();
            DefaultSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileDefaultSettings.Close();
            
            // load user settings
            string pathUserSettings = $"{DataUtils.Path}Resources/user_settings.json"; 
            StreamReader fileUserSettings = new StreamReader(pathUserSettings);
            
            jsonString = fileUserSettings.ReadToEnd();
            UserSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileUserSettings.Close();
        }

        public void ResetSettingsToDefault() {
            UserSettings = DefaultSettings.DeepClone(); // Data.DeepClone(obj)
            MenuDataContainer.UserSettings = UserSettings;
            string path = $"{DataUtils.Path}Resources/user_settings.json";
            
            StreamWriter fileUserSettings = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(DefaultSettings, DataUtils.JsonOptions);
            
            fileUserSettings.Write(jsonString);
            fileUserSettings.Close();
        }
        
        public void AddCustomShip(MenuProperties props) {
            Settings newSettings = UserSettings.DeepClone(); // Data.DeepClone(obj)

            if (SetNewShipSize(props, newSettings)) {
                string name = SetNewShipName(props);
                newSettings.ShipCount = newSettings.ShipCount.Add(0); // Data.Add(item)
                newSettings.ShipNames = newSettings.ShipNames.Add(name); // Data.Add(item)
                SaveUserSettings(newSettings);
            }

            props.Renderer.PreviousFrame = "";
            props.Renderer.RenderMenu(props);
        }
        
        private bool SetNewShipSize(MenuProperties props, Settings settings) {
            string[] options = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Return to settings menu"};
            const string message = "Enter new ship size";
            
            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);
            if (value == "Return to settings menu") return false;
            
            settings.ShipSettings = settings.ShipSettings.Add(int.Parse(value)); // Data.Add(item)
            return true;
        }
        
        private string SetNewShipName(MenuProperties props) {
            Console.Clear();
            Console.CursorVisible = true;
            string name;
            
            Console.WriteLine("Please enter name for new ship: ");
            while (true) {
                name = Console.ReadLine();
                if (StringValidator.ValidateShipName(name) && !UserSettings.ShipNames.Contains(name)) break;
                Console.WriteLine(UserSettings.ShipNames.Contains(name)
                    ? "Please enter valid name for new ship [Do not duplicate your ship names!]: "
                    : "Please enter valid name for new ship [Ship name must be with length from 1 to 20 chars and" +
                      " do not use menu names for ships!]: ");
            }
            
            //props.Level.SubmenuList.Add(name);
            Console.CursorVisible = false;
            return name;
        }

        public void SetShipArrangement(MenuProperties props) {
            Settings newSettings = UserSettings.DeepClone(); // Data.DeepClone(obj)
            string[] options = GetArrangementOptions();
            string currentValue = UserSettings.ShipArrangement.ToString();
            string message = $"Please enter available ship arrangement value [current: {currentValue}]";
            
            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);
            
            if (value != "Return to settings menu") {
                newSettings.ShipArrangement = bool.Parse(value);
                if (currentValue != value) SaveUserSettings(newSettings);
            }

            props.Renderer.PreviousFrame = "";
            props.Renderer.RenderMenu(props);
        }

        public void SetShipSettings(MenuProperties props, string shipName) {
            int index = GetShipIndex(UserSettings.ShipNames, shipName);
            Settings newSettings = UserSettings.DeepClone(); // Data.DeepClone(obj)

            if (SetShipSettingsCount(props, newSettings, shipName, index)) {
                if (SetShipSettingsSize(props, newSettings, shipName, index)) { 
                    SetShipSettingsName(props, newSettings, shipName, index);
                    SaveUserSettings(newSettings);
                }
            }

            props.Renderer.RenderMenu(props);
        }

        private bool SetShipSettingsCount(MenuProperties props, Settings settings, string shipName, int index)
        {
            string[] options = GetShipCountOptions(settings, index);
            string currentValue = UserSettings.ShipCount[index].ToString();
            string message = $"Please enter {shipName} count in battle [current: {currentValue}]";

            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);

            if (value == "Return to settings menu") return false;
            
            settings.ShipCount[index] = int.Parse(value);
            return true;
        }

        private bool SetShipSettingsSize(MenuProperties props, Settings settings, string shipName, int index)
        {
            string[] options = GetShipSizeOptions(settings, index);
            string currentValue = UserSettings.ShipSettings[index].ToString();
            string message = $"Please enter {shipName} size [current: {currentValue}]";
            
            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);

            if (value == "Return to settings menu") return false;

            settings.ShipSettings[index] = int.Parse(value);
            return true;
        }

        private void SetShipSettingsName(MenuProperties props, Settings settings, string shipName, int index)
        {
            string[] options = {"Yes", "No", "Return to settings menu"};
            string message = $"Do you want to rename {shipName}?";
            
            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);
            
            if (value == "No" || value == "Return to settings menu") return;

            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine($"Please enter new name for ship [Previous: {shipName}]: ");
            
            string newName = Console.ReadLine();
            
            settings.ShipNames[index] = newName;
            props.Level.SubmenuList[index] = newName;
            Console.CursorVisible = false;
            props.Renderer.PreviousFrame = "Empty frame";
        }
        
        public void SetBattlefieldSize(MenuProperties props) { 
            Settings newSettings = UserSettings.DeepClone(); // Data.DeepClone(obj)
            if (SetBattlefieldHeight(props, newSettings)) {
                if (SetBattlefieldWidth(props, newSettings)) SaveUserSettings(newSettings);
            }

            props.Renderer.PreviousFrame = "";
            props.Renderer.RenderMenu(props);
        }

        private bool SetBattlefieldHeight(MenuProperties props, Settings settings) {
            string[] options = GetBattlefieldOptions(settings);
            string currentValue = UserSettings.BattlefieldSize[0].ToString();
            string message = $"Please enter available battlefield height value [Current: {currentValue}]";

            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);

            if (value == "Return to settings menu") {
                return false;
            }

            settings.BattlefieldSize[0] = int.Parse(value);
            return true;
        }
        
        private bool SetBattlefieldWidth(MenuProperties props, Settings settings) {
            string[] options = GetBattlefieldOptions(settings, settings.BattlefieldSize[0]);
            string currentValue = UserSettings.BattlefieldSize[1].ToString();
            string message = $"Please enter available battlefield width [Current: {currentValue}]";

            props.Renderer.RenderSettingsSetup(options, message, 0);
            string value = props.SettingsEventListener.EventListener(props, options, message, 0);
            if (value == "Return to settings menu") {
                return false;
            }

            settings.BattlefieldSize[1] = int.Parse(value);
            return true;
        }
        
        private string[] GetArrangementOptions() {
            List<string> optionsList = new List<string>();
            if (CheckSettingsCorrectness(arrangNumBool: 1)) optionsList.Add("True");
            if (CheckSettingsCorrectness(arrangNumBool: 0)) optionsList.Add("False");
            optionsList.Add("Return to settings menu");
            return optionsList.ToArray();
        }

        private string[] GetShipCountOptions(Settings settings, int index) {
            List<string> optionsList = new List<string>();
            int[] checkList = settings.ShipCount;
            for (int i = 0; i < 21; i++) {
                checkList[index] = i;
                if (CheckSettingsCorrectness(shipCount: checkList)) optionsList.Add($"{i}");
            }
            optionsList.Add("Return to settings menu");
            return optionsList.ToArray();
        }
        
        private string[] GetShipSizeOptions(Settings settings, int index) {
            List<string> optionsList = new List<string>();
            int[] checkList = settings.ShipSettings;
            for (int i = 1; i < 11; i++) {
                checkList[index] = i;
                if (CheckSettingsCorrectness(shipSettings: checkList)) optionsList.Add($"{i}");
            }
            optionsList.Add("Return to settings menu");
            return optionsList.ToArray();
        }
        
        private string[] GetBattlefieldOptions(Settings settings, int checkHeight = -1) {
            List<string> optionsList = new List<string>();
            for (int i = 5; i < 27; i++) {
                int width = checkHeight == -1 ? settings.BattlefieldSize[1] : i;
                int height = checkHeight == -1 ? i : checkHeight;
                if (CheckSettingsCorrectness(battlefieldSize: new []{height, width})) optionsList.Add($"{i}");
            }
            optionsList.Add("Return to settings menu");
            return optionsList.ToArray();
        }

        private int GetShipIndex(IReadOnlyList<string> names, string shipName) {
            for (int i = 0; i < names.Count; i++) {
                if (shipName != names[i]) {
                    continue;
                }
                
                return i;
            }
            
            return -1;
        }

        private bool CheckSettingsCorrectness(int arrangNumBool = -1, IReadOnlyList<int> shipCount = null, 
            IReadOnlyList<int> shipSettings = null, IReadOnlyList<int> battlefieldSize = null) {

            bool arrangement = arrangNumBool == -1 ? UserSettings.ShipArrangement : arrangNumBool != 0;
            shipCount ??= UserSettings.ShipCount;
            shipSettings ??= UserSettings.ShipSettings;
            battlefieldSize ??= UserSettings.BattlefieldSize;
            
            int battlefieldHeight = battlefieldSize[0];
            int battlefieldWidth = battlefieldSize[1];
            int boardSize = battlefieldHeight * battlefieldWidth;
            if (arrangement) return boardSize >= CalculateShipsCapacity(shipCount, shipSettings);

            int shipsCapacity = CalculateShipsCapacity(shipCount, shipSettings) * 2;
            shipsCapacity += 6 * shipCount.Sum() - (battlefieldHeight * 2 + battlefieldWidth * 2);
            return boardSize >= shipsCapacity;
        }
        
        private static int CalculateShipsCapacity(IReadOnlyList<int> shipCount, IReadOnlyList<int> shipSettings) {
            int capacity = 0;
            for (int i = 0; i < shipCount.Count; i++) {
                for (int j = 0; j < shipCount[i]; j++) {
                    capacity += shipSettings[i];
                }
            }

            return capacity;
        }
    }
}