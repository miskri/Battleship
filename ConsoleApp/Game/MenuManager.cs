using System;
using System.Linq;

namespace ConsoleApp {

    public class MenuManager {
        
        private MenuProperties _props;

        public void Start() {
            _props = new MenuProperties {Manager = this, SettingsManager = new SettingsManager()};
            _props.SettingsManager.LoadSettings();
            MenuDataContainer.UserSettings = _props.SettingsManager.UserSettings;
            _props.Level = new MenuLevel("Main Menu", MenuDataContainer.GetSubmenuList("Main Menu"));
            _props.Level.LevelDescription = MenuDataContainer.GetLevelDescription(_props.Level.LevelTitle);
            _props.Renderer = new MenuRenderer {PreviousFrame = "Empty frame"};
            _props.EventListener = new MenuEventListener();
            _props.SettingsEventListener = new SettingsEventListener();
            _props.SelectedRow = 0;
            _props.Renderer.RenderMenu(_props);
            _props.EventListener.EventListener(_props);
        }

        public void EventEnter(MenuProperties props) {
            string selectedMenu = props.Level.SubmenuList[props.SelectedRow];
            switch (selectedMenu) {
                case "Exit":
                    Console.Clear();
                    Environment.Exit(0);
                    break;

                case "Back":
                    GetBack(props);
                    break;

                case "Fast Game":
                    props.GameMode = "Fast Game";
                    GameManager fastGame = new GameManager();
                    fastGame.Start(props.GameMode, props.SettingsManager.DefaultSettings);
                    break;

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    GameModeSettings(props);
                    break;

                case "Default settings":
                    GameManager defaultGame = new GameManager();
                    defaultGame.Start(props.GameMode, props.SettingsManager.DefaultSettings);
                    break;

                case "User settings":
                    GameManager userGame = new GameManager();
                    userGame.Start(props.GameMode, props.SettingsManager.UserSettings);
                    break;
                
                case "Load":
                    LoadSavedGame(props.Level.LevelTitle);
                    break;
                
                case "Delete":
                    Save saveForDelete = SaveManager.GetSaveReference(props.Level.LevelTitle);
                    SaveManager.DeleteSavedGame(saveForDelete);
                    GetBack(props);
                    break;
                
                case "Reset user settings to default":
                    props.SettingsManager.ResetSettingsToDefault();
                   GetBack(props);
                    break;
                
                case "Ship arrangement": 
                    props.SettingsManager.SetShipArrangement(props);
                    break;

                case "Battlefield size":
                    props.SettingsManager.SetBattlefieldSize(props);
                    break;
                
                case "Add custom ship":
                    props.SettingsManager.AddCustomShip(props);
                    break;
                
                case "Carrier": case "Battleship": case "Submarine": case "Cruiser": case "Patrol":
                    props.SettingsManager.SetShipSettings(props, selectedMenu);
                    break;

                default: {
                    if (props.SettingsManager.UserSettings.ShipNames.Contains(selectedMenu)) {
                        props.SettingsManager.SetShipSettings(props, selectedMenu);
                    }
                    DefaultStatement(props);
                    break;
                }
            }
        }

        public void SwitchParametersToPreviousMenu(MenuLevel level) {
            level.LevelTitle = level.PreviousMenu[^1];
            level.RemoveLastPreviousMenu();
            level.SubmenuList = MenuDataContainer.GetSubmenuList(level.LevelTitle);
            level.LevelDescription = MenuDataContainer.GetLevelDescription(level.LevelTitle);
        }

        private void GetBack(MenuProperties props) {
            SwitchParametersToPreviousMenu(props.Level);
            props.SelectedRow = 0;
            props.Renderer.RenderMenu(props);
        }

        private void DefaultStatement(MenuProperties props) {
            if (MenuDataContainer.GetSubmenuList(props.Level.SubmenuList[props.SelectedRow]) == null) {
                return;
            }

            if (props.Level.SubmenuList[props.SelectedRow] == "Main Menu") {
                props.Level.ClearPreviousMenu();
            }
            else {
                props.Level.PreviousMenu.Add(props.Level.LevelTitle);
            }
            
            props.Level.LevelTitle = props.Level.SubmenuList[props.SelectedRow];
            props.Level.SubmenuList = MenuDataContainer.GetSubmenuList(props.Level.LevelTitle);
            props.Level.LevelDescription = MenuDataContainer.GetLevelDescription(props.Level.LevelTitle);
            props.SelectedRow = 0;
            props.Renderer.RenderMenu(props);
        }

        private void GameModeSettings(MenuProperties props) {
            props.GameMode = props.Level.SubmenuList[props.SelectedRow];
            props.Level.PreviousMenu.Add(props.Level.LevelTitle);
            props.Level.LevelTitle = props.Level.SubmenuList[props.SelectedRow];
            props.Level.SubmenuList = MenuDataContainer.GetSubmenuList(props.Level.LevelTitle);
            props.Level.LevelDescription = MenuDataContainer.GetLevelDescription(props.Level.LevelTitle);
            props.SelectedRow = 0;
            props.Renderer.RenderMenu(props);
        }

        private void LoadSavedGame(string saveName) {
            Save selectedSave = SaveManager.GetSave(saveName);
            selectedSave.Properties.Manager = new BattleManager();
            selectedSave.Properties.Manager.LoadBattle(selectedSave.Properties);
        }
    }
}