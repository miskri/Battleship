using System.Linq;

namespace ConsoleApp {
    
    public static class StringValidator {

        private static readonly string[] IgnoredNames = {"New Game", "Fast Game", "Load Game", "Settings", "Exit", 
            "Player vs Player", "Player vs AI", "AI vs AI", "Back", "Add custom ship", "Ship arrangement", 
            "Ship settings", "Battlefield size", "Reset user settings to default", "Default settings", "User settings",
            "Main Menu", "Load", "Delete"};
        
        public static bool ValidateShipName(string name) {
            return name.Length > 0 && name.Length < 21 && !IgnoredNames.Contains(name);
        }
    }
}