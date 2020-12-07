using System;

namespace ConsoleApp {

    public class MenuEventListener {

        private readonly MenuManager _menuManager;
        
        public MenuEventListener(MenuManager menuManager) {
            _menuManager = menuManager;
        }
        
        public void EventListener(MenuLevel level, int selectedRow, int submenuCount) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.UpArrow when selectedRow - 1 >= 0: 
                    _menuManager.MenuRenderer.RenderMenu(this, level, --selectedRow);
                    break;

                case ConsoleKey.DownArrow when selectedRow + 1 < submenuCount:
                    _menuManager.MenuRenderer.RenderMenu(this, level, ++selectedRow);
                    break;

                case ConsoleKey.Enter:
                    _menuManager.EventEnter(level, selectedRow, submenuCount);
                    break;

                case ConsoleKey.Backspace:
                    if (level.LevelTitle != "Main Menu") {
                        _menuManager.SwitchParametersToPreviousMenu(level);
                    }
                    _menuManager.MenuRenderer.RenderMenu(this, level, 0);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    break;

                default:
                    EventListener(level, selectedRow, submenuCount);
                    break;
            }
        }
    }
}