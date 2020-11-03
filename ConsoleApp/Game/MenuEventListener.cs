using System;

namespace ConsoleApp {

    public class MenuEventListener {

        private readonly MenuManager _menuManager;
        private readonly MenuRenderer _menuRenderer;
        
        public MenuEventListener(MenuManager menuManager, MenuRenderer renderer) {
            _menuManager = menuManager;
            _menuRenderer = renderer;
        }
        
        public void EventListener(MenuLevel level, int selectedRow, int submenuCount) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.UpArrow when selectedRow - 1 >= 0: 
                    _menuRenderer.RenderMenu(this, level, --selectedRow);
                    break;

                case ConsoleKey.DownArrow when selectedRow + 1 < submenuCount:
                    _menuRenderer.RenderMenu(this, level, ++selectedRow);
                    break;

                case ConsoleKey.Enter:
                    EventEnter(level, selectedRow, submenuCount);
                    break;

                case ConsoleKey.Backspace:
                    if (level.LevelTitle != "Main Menu") {
                        SwitchParametersToPreviousMenu(level);
                    }
                    _menuRenderer.RenderMenu(this, level, 0);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    break;

                default:
                    EventListener(level, selectedRow, submenuCount);
                    break;
            }
        }

        private void EventEnter(MenuLevel level, int selectedRow, int submenuCount) {
            switch (level.SubmenuList[selectedRow]) {
                case "Exit":
                    Console.Clear();
                    Environment.Exit(0);
                    break;

                case "Back":
                    SwitchParametersToPreviousMenu(level);
                    _menuRenderer.RenderMenu(this, level, 0);
                    break;

                case "Fast Game":
                    _menuManager.GameType = "Fast Game";
                    GameManager fastGame = new GameManager();
                    fastGame.Start(_menuManager.GameType, false, new[] {1, 1, 1, 1, 1},
                        new[] {5, 4, 3, 2, 1}, new[] {10, 10});
                    break;

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    _menuManager.GameType = level.SubmenuList[selectedRow];
                    level.PreviousMenu.Add(level.LevelTitle);
                    level.LevelTitle = level.SubmenuList[selectedRow];
                    level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                    level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                    _menuRenderer.RenderMenu(this, level, 0);
                    break;

                case "Default settings":
                    GameManager defaultGame = new GameManager();
                    defaultGame.Start(_menuManager.GameType, false, new[] {1, 1, 1, 1, 1},
                        new[] {5, 4, 3, 2, 1}, new[] {10, 10});
                    break;

                case "User settings":
                    GameManager userGame = new GameManager();
                    Settings userSettings = _menuManager.UserSettings;
                    userGame.Start(_menuManager.GameType, userSettings.ShipArrangement, userSettings.ShipCount, 
                        userSettings.ShipSettings, userSettings.BattlefieldSize);
                    break;
                
                case "Reset user settings to default":
                    _menuManager.ResetSettingsToDefault();
                    SwitchParametersToPreviousMenu(level);
                    _menuRenderer.RenderMenu(this, level, 0);
                    break;
                
                case "Ship arrangement": case "Ship count": case "Ship settings": case "Battlefield size":
                        _menuManager.SetUserSettings(level.SubmenuList[selectedRow]);
                    break;

                default: {
                    DefaultStatement(level, selectedRow, submenuCount);
                    break;
                }
            }
        }

        private void SwitchParametersToPreviousMenu(MenuLevel level) {
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
                _menuRenderer.RenderMenu(this, level, 0);
            }
            else {
                EventListener(level, selectedRow, submenuCount);
            }
        }
    }
}