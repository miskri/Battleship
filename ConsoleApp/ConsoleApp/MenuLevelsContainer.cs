using System.Collections.Generic;

namespace ConsoleApp
{
    public static class MenuLevelsContainer
    {
        public static List<string> GetSubmenuList(string title)
        {
            switch (title)
            {
                case "Main Menu":
                    return new List<string>{"New Game", "Load Game", "Settings", "Exit"};
                case "New Game":
                    return new List<string>{"Player vs Player", "Player vs AI", "AI vs AI", "Back", "Exit"};
                case "Load Game":
                    return new List<string>{"<save>", "Back", "Exit"};
                case "Settings":
                    return new List<string>{"Set menu size", "Set menu color", "Back", "Exit"};
                case "Set menu size":
                    return new List<string>{"Length", "Height", "Back", "Main Menu", "Exit"};
                case "Set menu color":
                    return new List<string>{"Text color", "Background color", "Back", "Main Menu", "Exit"};
            }
            return null;
        }
    }
}