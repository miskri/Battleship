using System;

namespace ConsoleApp {

    public class MenuEventListener {
        
        public void EventListener(MenuProperties props) {
            while (true) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.UpArrow when props.SelectedRow - 1 >= 0:
                        props.SelectedRow--;
                        props.Renderer.RenderMenu(props);
                        break;

                    case ConsoleKey.DownArrow when props.SelectedRow + 1 < props.Level.SubmenuList.Count:
                        props.SelectedRow++;
                        props.Renderer.RenderMenu(props);
                        break;

                    case ConsoleKey.Enter:
                        props.Manager.EventEnter(props);
                        break;

                    case ConsoleKey.Backspace:
                        props.SelectedRow = 0;
                        if (props.Level.LevelTitle != "Main Menu") props.Manager.SwitchParametersToPreviousMenu(props.Level);
                        props.Renderer.RenderMenu(props);
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }   
            }
        }
    }
}