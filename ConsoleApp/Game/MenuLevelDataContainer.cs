using System.Collections.Generic;

namespace ConsoleApp {
    public static class MenuLevelDataContainer {
        public static List<string> GetSubmenuList(string title) {
            switch (title) {
                case "Main Menu":
                    return new List<string> {"New Game", "Fast Game", "Load Game", "Settings", "Exit"};

                case "New Game":
                    return new List<string> {"Player vs Player", "Player vs AI", "AI vs AI", "Back", "Exit"};

                case "Load Game":
                    return new List<string> {"<save>", "Back", "Exit"};

                case "Settings":
                    return new List<string> {"Battle settings", "Visualization settings", "Back", "Exit"};

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return new List<string> {"Default settings", "User settings", "Back", "Main Menu", "Exit"};

                case "Battle settings":
                    return new List<string> {
                        "Ship arrangement", "Ship count", "Ship settings", "Battlefield size", "Back", "Main Menu", "Exit"};

                case "Visualization settings":
                    return new List<string> {"Highlight color", "Text color", "Back", "Main Menu", "Exit"};
            }

            return null;
        }

        public static string GetLevelDescription(string levelTitle) {
            switch (levelTitle) {
                case "Main Menu":
                    return "Console Battleship ver 0.2";

                case "Player vs Player": case "Player vs AI": case "AI vs AI":
                    return "Which battle settings we should use?";
            }

            return "";
        }
    }
}