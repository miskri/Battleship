using System;

namespace ConsoleApp {
    
    public class SettingsEventListener {
        
        public string EventListener(MenuProperties props, string[] options, string message, int selectedRow) {
            while (true) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.UpArrow when selectedRow - 1 >= 0:
                        props.Renderer.RenderSettingsSetup(options, message, --selectedRow);
                        break;

                    case ConsoleKey.DownArrow when selectedRow + 1 < options.Length:
                        props.Renderer.RenderSettingsSetup(options, message, ++selectedRow);
                        break;

                    case ConsoleKey.Enter:
                        return options[selectedRow];

                    case ConsoleKey.Backspace:
                        return "Return to settings menu";

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }   
            }
        }
    }
}