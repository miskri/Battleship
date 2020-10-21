using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp {
    public class UserSettings {
        public string HighlightColor { get; set; }
        public string TextColor { get; set; }
        public bool ShipArrangement { get; set; }
        public int[] ShipCount { get; set; }
        public int[] ShipSettings { get; set; }
        public int[] BattlefieldSize { get; set; }
    }

    public class ConsoleMenu {
        private const int MenuLength = 40;
        private UserSettings _settings;
        private ConsoleColor _highlighColor, _textColor, _backgroundColor;
        private string _gameType;

        public void Start() {
            LoadUserSettings();
            MenuLevel currentLevel = new MenuLevel("Main Menu", MenuLevelDataContainer.GetSubmenuList("Main Menu"));
            currentLevel.LevelDescription = MenuLevelDataContainer.GetLevelDescription(currentLevel.LevelTitle);
            RenderMenu(currentLevel, 0);
        }

        private void LoadUserSettings() {
            var path = "C:/Users/Mihhail/RiderProjects/icd0008-2020f/ConsoleApp/Game/user_settings.json";
            StreamReader file = new StreamReader(path);
            string jsonString = file.ReadToEnd();
            _settings = JsonSerializer.Deserialize<UserSettings>(jsonString);
            _highlighColor = LoadColor(_settings.HighlightColor);
            _textColor = LoadColor(_settings.TextColor);
            _backgroundColor = ConsoleColor.Black;
        }

        private ConsoleColor LoadColor(string color) {
            switch (color) {
                
                case "White":
                    return ConsoleColor.White;
                case "Blue":
                    return ConsoleColor.DarkBlue;
                case "Red":
                    return ConsoleColor.Red;
                case "Green":
                    return ConsoleColor.DarkGreen;
            }

            return ConsoleColor.DarkMagenta;
        }

        private void RenderMenu(MenuLevel level, int selected) {
            List<string> submenuList = level.SubmenuList;
            Console.Clear();
            Console.WriteLine(new string('=', MenuLength));
            if (level.LevelDescription != "") {
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(level.LevelDescription);
                Console.ForegroundColor = _textColor;
                Console.Write(new string(' ', MenuLength - 3 - level.LevelDescription.Length));
                Console.WriteLine("|");
            }

            for (int i = 0; i < submenuList.Count; i++) {
                if (i == selected) {
                    Console.Write("|");
                    Console.BackgroundColor = _highlighColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" " + submenuList[i] + new string(' ', MenuLength - 3 - submenuList[i].Length));
                    Console.BackgroundColor = _backgroundColor;
                    Console.ForegroundColor = _textColor;
                    Console.WriteLine("|");
                    continue;
                }

                Console.WriteLine("| " + submenuList[i] + new string(' ', MenuLength - 3 - submenuList[i].Length) + "|");
            }

            Console.WriteLine(new string('=', MenuLength));
            Console.CursorVisible = false;
            EventListener(level, selected, submenuList.Count);
        }

        private void EventListener(MenuLevel level, int selected, int submenuCount) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                
                case ConsoleKey.UpArrow when selected - 1 >= 0:
                    RenderMenu(level, --selected);
                    break;

                case ConsoleKey.DownArrow when selected + 1 < submenuCount:
                    RenderMenu(level, ++selected);
                    break;

                case ConsoleKey.Enter:
                    EventEnter(level, selected, submenuCount);
                    break;

                case ConsoleKey.Backspace:
                    if (level.LevelTitle != "Main Menu") {
                        level.LevelTitle = level.PreviousMenu[^1];
                        level.RemoveLastPreviousMenu();
                        level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                        level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                    }

                    RenderMenu(level, 0);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    break;

                default:
                    EventListener(level, selected, submenuCount);
                    break;
            }
        }

        private void EventEnter(MenuLevel level, int selected, int submenuCount) {
            switch (level.SubmenuList[selected]) {
                
                case "Exit":
                    Console.Clear();
                    break;

                case "Back":
                    level.LevelTitle = level.PreviousMenu[^1];
                    level.RemoveLastPreviousMenu();
                    level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                    level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                    RenderMenu(level, 0);
                    break;

                case "Fast Game":
                    _gameType = "Fast Game";
                    GameManager fastGame = new GameManager();
                    fastGame.Start(_gameType, false, new[] {1, 1, 1, 1, 1},
                        new[] {5, 4, 3, 2, 1}, new[] {10, 10});
                    break;

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    _gameType = level.SubmenuList[selected];
                    level.PreviousMenu.Add(level.LevelTitle);
                    level.LevelTitle = level.SubmenuList[selected];
                    level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                    level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                    RenderMenu(level, 0);
                    break;

                case "Default settings":
                    GameManager defaultGame = new GameManager();
                    defaultGame.Start(_gameType, false, new[] {1, 1, 1, 1, 1},
                        new[] {5, 4, 3, 2, 1}, new[] {10, 10});
                    break;

                case "User settings":
                    GameManager userGame = new GameManager();
                    userGame.Start(_gameType, _settings.ShipArrangement, _settings.ShipCount, _settings.ShipSettings,
                        _settings.BattlefieldSize);
                    break;

                default: {
                    if (MenuLevelDataContainer.GetSubmenuList(level.SubmenuList[selected]) != null) {
                        if (level.SubmenuList[selected] == "Main Menu") {
                            level.ClearPreviousMenu();
                        }
                        else {
                            level.PreviousMenu.Add(level.LevelTitle);
                        }

                        level.LevelTitle = level.SubmenuList[selected];
                        level.SubmenuList = MenuLevelDataContainer.GetSubmenuList(level.LevelTitle);
                        level.LevelDescription = MenuLevelDataContainer.GetLevelDescription(level.LevelTitle);
                        RenderMenu(level, 0);
                    }
                    else {
                        EventListener(level, selected, submenuCount);
                    }

                    break;
                }
            }
        }
    }
}