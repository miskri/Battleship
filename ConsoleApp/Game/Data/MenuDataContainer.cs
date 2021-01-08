using System.Collections.Generic;
using ConsoleApp.Control;
using ConsoleApp.Objects;

namespace ConsoleApp.Data {
    public static class MenuDataContainer {

        public static Settings UserSettings;
        public static List<string> GetSubmenuList(string title) {
            switch (title) {
                case "Main Menu":
                    return new List<string> {"New Game", "Fast Game", "Load Game", "Settings", "Exit"};

                case "New Game":
                    return new List<string> {"Player vs Player", "Player vs AI", "AI vs AI", "Back", "Exit"};

                case "Load Game":
                    List<string> submenuList = new List<string>();
                    List<string> saves = SaveManager.GetSavesList();
                    if (saves.Count > 0) submenuList.AddRange(saves.GetRange(0, saves.Count));
                    submenuList.Add("Back");
                    submenuList.Add("Exit");
                    return submenuList;

                case "Settings":
                    return new List<string> {"Add custom ship", "Ship arrangement", "Ship settings", "Battlefield size",
                        "Reset user settings to default", "Back", "Exit"};

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return new List<string> {"Default settings", "User settings", "Back", "Main Menu", "Exit"};
                
                case "Ship settings":
                    List<string> shipsSettings = new List<string>();
                    shipsSettings.AddRange(UserSettings.ShipNames);
                    shipsSettings.AddRange(new []{"Back", "Main Menu", "Exit"});
                    return shipsSettings;
            }

            return SaveManager.ContainsThisName(title) ? new List<string> {"Load", "Delete", "Back", "Main Menu", "Exit"} : null;
        }

        public static string GetLevelDescription(string levelTitle) {
            switch (levelTitle) {
                case "Main Menu":
                    return "Console Battleship ver 1.0r";
                
                case "New Game":
                    return "Select game mode";
                
                case "Load Game":
                    return SaveManager.Saves.Count == 0 ? "You don't have any saves yet" : "Load Game";

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return "Which battle settings we should use?";
                
                case "Settings":
                    return "Settings";
                
                case "Ship settings":
                    return "Which ship do you want to change?";
            }

            return SaveManager.ContainsThisName(levelTitle) ? levelTitle : "";
        }
    }
}