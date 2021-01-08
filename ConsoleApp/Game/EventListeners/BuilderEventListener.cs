using System;
using ConsoleApp.Control;
using ConsoleApp.Data;

namespace ConsoleApp.EventListeners {
    
    // Reading clicks during the ship placing, moving around the field, transferring information for rendering to the render.
    public class BuilderEventListener {
        
        public void EventListener(BuilderProperties props, int row, int col) {
            bool shipPlaced = false;
            while (!shipPlaced) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.RightArrow when col + 1 < props.PlayerField.GetLength(1):
                        props.Renderer.RenderBoard(props, row, ++col);
                        break;

                    case ConsoleKey.LeftArrow when col - 1 >= 0:
                        props.Renderer.RenderBoard(props, row, --col);
                        break;

                    case ConsoleKey.UpArrow when row - 1 >= 0:
                        props.Renderer.RenderBoard(props, --row, col);
                        break;

                    case ConsoleKey.DownArrow when row + 1 < props.SelectableRowCount + props.MenuOptions.Count:
                        props.Renderer.RenderBoard(props, ++row, col);
                        break;

                    case ConsoleKey.Enter when row >= props.SelectableRowCount:
                        MenuEnterEvent(props.SelectableRowCount, row);
                        props.Renderer.RenderBoard(props, row, col);
                        break;
                
                    case ConsoleKey.R:
                        props.Direction = props.Direction == 0 ? 1 : 0;
                        props.Renderer.RenderBoard(props, row, col);
                        break;
                
                    case ConsoleKey.Enter:
                        if (props.Builder.ShipPlacement(props, row, col)) shipPlaced = true;
                        break;
                
                    case ConsoleKey.M:
                        row = props.SelectableRowCount;
                        props.Renderer.RenderBoard(props, row, 0);
                        break;
                }
            }
        }
        
        private void MenuEnterEvent(int selectableFieldRowCount, int row) {
            switch (row - selectableFieldRowCount) {
                case 0: // Main Menu option
                    MenuManager newMenuManager = new MenuManager();
                    newMenuManager.Start();
                    break;

                case 1: // Quit
                    Environment.Exit(0);
                    break;
            }
        }
    }
}