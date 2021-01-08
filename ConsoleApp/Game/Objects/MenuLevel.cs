using System.Collections.Generic;

namespace ConsoleApp.Objects {
    public class MenuLevel {
        public string LevelTitle;
        public string LevelDescription;
        public List<string> SubmenuList;
        public List<string> PreviousMenu;

        public MenuLevel(string title, List<string> list) {
            LevelTitle = title;
            SubmenuList = list;
            PreviousMenu = new List<string> {""};
            LevelDescription = "";
        }

        public void RemoveLastPreviousMenu() {
            PreviousMenu.RemoveAt(PreviousMenu.Count - 1);
        }

        public void ClearPreviousMenu() {
            PreviousMenu = new List<string> {""};
        }
    }
}