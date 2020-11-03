using System.IO;
using System.Text.Json;

namespace ConsoleApp {

    public class MenuManager {
        
        public Settings DefaultSettings, UserSettings;
        private MenuRenderer _menuRenderer;
        private MenuEventListener _eventListener;

        private readonly string _path = "C:/Users/Mihhail/RiderProjects/icd0008-2020f/ConsoleApp/Game/";
        public string GameType;

        public void Start() {
            LoadSettings();
            MenuLevel currentLevel = new MenuLevel("Main Menu", MenuLevelDataContainer.GetSubmenuList("Main Menu"));
            currentLevel.LevelDescription = MenuLevelDataContainer.GetLevelDescription(currentLevel.LevelTitle);
            _menuRenderer = new MenuRenderer(this);
            _eventListener = new MenuEventListener(this, _menuRenderer);
            _menuRenderer.RenderMenu(_eventListener, currentLevel, 0);
        }

        private void LoadSettings() {
            string pathDefaultSettings = $"{_path}default_settings.json"; 
            StreamReader fileDefaultSettings = new StreamReader(pathDefaultSettings);
            string jsonString = fileDefaultSettings.ReadToEnd();
            DefaultSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileDefaultSettings.Close();
            
            string pathUserSettings = $"{_path}user_settings.json"; 
            StreamReader fileUserSettings = new StreamReader(pathUserSettings);
            jsonString = fileUserSettings.ReadToEnd();
            UserSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileUserSettings.Close();
        }

        public void ResetSettingsToDefault() {
            string path = $"{_path}user_settings.json";
            StreamWriter fileUserSettings = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(DefaultSettings);
            fileUserSettings.Write(jsonString);
            fileUserSettings.Close();
        }

        public void SetUserSettings(string parameter) {
            switch (parameter) {
                case "Ship arrangement":
                    SetShipArrangement();
                    break;
                
                case "Ship count":
                    SetShipCount();
                    break;
                
                case "Ship settings":
                    SetShipSettings();
                    break;
                
                case "Battlefield size":
                    SetBattlefieldSize();
                    break;
            }
        }

        private void SetShipArrangement() {
            
        }

        private void SetShipCount() {
            
        }

        private void SetShipSettings() {
            
        }

        private void SetBattlefieldSize() {
            
        }
    }
}