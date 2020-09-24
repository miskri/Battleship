using System.Collections.Generic;

namespace ConsoleApp
{

    public class MenuLevel
    {
        public List<string> SubmenuList;
        public string LevelTitle;
        public List<string> PreviousMenu;

        public MenuLevel(string title, List<string> list)
        {
            LevelTitle = title;
            SubmenuList = list;
            PreviousMenu = new List<string>{""};
        }
        
        public void SetTitle(string title)
        {
            LevelTitle = title;
        }
        
        public void SetSubmenuList(List<string> list)
        {
            SubmenuList = list;
        }

        public void AddPreviousMenu(string title)
        {
            PreviousMenu.Add(title);
        }
        
        public void RemoveLastPreviousMenu()
        {
            PreviousMenu.RemoveAt(PreviousMenu.Count - 1);
        }

        public void ClearPreviousMenu()
        {
            PreviousMenu = new List<string>{""};
        }
    }
}