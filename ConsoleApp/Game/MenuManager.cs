using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp {

    public class MenuManager {
        private Settings _defaultSettings, _userSettings;
        private MenuRenderer _menuRenderer;
        private MenuEventListener _eventListener;

        private const string Path = "C:/Users/Mihhail/RiderProjects/icd0008-2020f/ConsoleApp/Game/";
        private string _gameType;

        public void Start() {
            LoadSettings();
            MenuLevel currentLevel = new MenuLevel("Main Menu", MenuLevelDataContainer.GetSubmenuList("Main Menu"));
            currentLevel.LevelDescription = MenuLevelDataContainer.GetLevelDescription(currentLevel.LevelTitle);
            _menuRenderer = new MenuRenderer();
            _eventListener = new MenuEventListener(this, _menuRenderer);
            _menuRenderer.RenderMenu(_eventListener, currentLevel, 0);
        }

        private void LoadSettings() {
            string pathDefaultSettings = $"{Path}default_settings.json"; 
            StreamReader fileDefaultSettings = new StreamReader(pathDefaultSettings);
            string jsonString = fileDefaultSettings.ReadToEnd();
            _defaultSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileDefaultSettings.Close();
            
            string pathUserSettings = $"{Path}user_settings.json"; 
            StreamReader fileUserSettings = new StreamReader(pathUserSettings);
            jsonString = fileUserSettings.ReadToEnd();
            _userSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            fileUserSettings.Close();
        }

        private void ResetSettingsToDefault() {
            string path = $"{Path}user_settings.json";
            StreamWriter fileUserSettings = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(_defaultSettings);
            fileUserSettings.Write(jsonString);
            fileUserSettings.Close();
        }
        
        public void EventEnter(MenuLevel level, int selectedRow, int submenuCount) {
            switch (level.SubmenuList[selectedRow]) {
                case "Exit":
                    Console.Clear();
                    Environment.Exit(0);
                    break;

                case "Back":
                    SwitchParametersToPreviousMenu(level);
                    _menuRenderer.RenderMenu(_eventListener, level, 0);
                    break;

                case "Fast Game":
                    _gameType = "Fast Game";
                    GameManager fastGame = new GameManager();
                    fastGame.Start(_gameType, _defaultSettings);
                    break;

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    _gameType = level.SubmenuList[selectedRow];
                    level.PreviousMenu.Add(level.LevelTitle);
                    level.LevelTitle = level.SubmenuList[selectedRow];
                    level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                    level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                    _menuRenderer.RenderMenu(_eventListener, level, 0);
                    break;

                case "Default settings":
                    GameManager defaultGame = new GameManager();
                    defaultGame.Start(_gameType, _defaultSettings);
                    break;

                case "User settings":
                    GameManager userGame = new GameManager();
                    userGame.Start(_gameType, _userSettings);
                    break;
                
                case "Reset user settings to default":
                    ResetSettingsToDefault();
                    SwitchParametersToPreviousMenu(level);
                    _menuRenderer.RenderMenu(_eventListener, level, 0);
                    break;
                
                case "Ship arrangement": 
                    SetShipArrangement(level, selectedRow);
                    break;

                case "Ship count": 
                    SetShipCount(level, selectedRow);
                    break;

                case "Ship settings": 
                    SetShipSettings(level, selectedRow);
                    break;

                case "Battlefield size":
                    SetBattlefieldSize(level, selectedRow);
                    break;

                default: {
                    DefaultStatement(level, selectedRow, submenuCount);
                    break;
                }
            }
        }

        public void SwitchParametersToPreviousMenu(MenuLevel level) {
            level.LevelTitle = level.PreviousMenu[^1];
            level.RemoveLastPreviousMenu();
            level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
            level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
        }

        private void DefaultStatement(MenuLevel level, int selectedRow, int submenuCount) {
            if (MenuLevelDataContainer.GetSubmenuList(level.SubmenuList[selectedRow]) != null) {
                if (level.SubmenuList[selectedRow] == "Main Menu") {
                    level.ClearPreviousMenu();
                }
                else {
                    level.PreviousMenu.Add(level.LevelTitle);
                }

                level.LevelTitle = level.SubmenuList[selectedRow];
                level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                _menuRenderer.RenderMenu(_eventListener, level, 0);
            }
            else {
                _eventListener.EventListener(level, selectedRow, submenuCount);
            }
        }
        
        private void SetShipArrangement(MenuLevel level, int selectedRow) {
            _menuRenderer.RenderMenu(_eventListener, level, selectedRow);
        }

        private void SetShipCount(MenuLevel level, int selectedRow) {
            _menuRenderer.RenderMenu(_eventListener, level, selectedRow);
        }

        private void SetShipSettings(MenuLevel level, int selectedRow) {
            _menuRenderer.RenderMenu(_eventListener, level, selectedRow);
        }

        private void SetBattlefieldSize(MenuLevel level, int selectedRow) {
            _menuRenderer.RenderMenu(_eventListener, level, selectedRow);
        }
    }
}