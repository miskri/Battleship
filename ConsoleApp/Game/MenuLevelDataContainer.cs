using System.Collections.Generic;

namespace ConsoleApp {
    public static class MenuLevelDataContainer {

        public static List<string> Saves = new List<string>();
        public static List<string> GetSubmenuList(string title) {
            switch (title) {
                case "Main Menu":
                    return new List<string> {"New Game", "Fast Game", "Load Game", "Settings", "Exit"};

                case "New Game":
                    return new List<string> {"Player vs Player", "Player vs AI", "AI vs AI", "Back", "Exit"};

                case "Load Game":
                    List<string> submenuList = new List<string>();
                    if (Saves.Count > 0) {
                        submenuList.AddRange(Saves.GetRange(0, Saves.Count));
                    }
                    submenuList.Add("Back");
                    submenuList.Add("Exit");
                    return submenuList;

                case "Settings":
                    return new List<string> {"Ship arrangement", "Ship count", "Ship settings", "Battlefield size", 
                        "Reset user settings to default", "Back", "Exit"};

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return new List<string> {"Default settings", "User settings", "Back", "Main Menu", "Exit"};
            }

            return null;
        }

        public static string GetLevelDescription(string levelTitle) {
            switch (levelTitle) {
                case "Main Menu":
                    return "Console Battleship ver 0.5b";
                
                case "New Game":
                    return "Select game mode";
                
                case "Load Game" when Saves.Count == 0:
                    return "You don't have any saves yet";

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return "Which battle settings we should use?";
            }

            return "";
        }
    }
}