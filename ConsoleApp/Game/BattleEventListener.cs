using System;

namespace ConsoleApp {
    
    // Reading clicks during the game, moving around the field, transferring information for rendering to the render.
    public class BattleEventListener {

        public void EventListener(GameProperties props, int row, int col) {
            while (true) {
                if (props.Winner != null && row >= props.MenuOptions.Count) {
                    row = 0;
                    props.Renderer.GameOverScreen(props, 0);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key) {
                    case ConsoleKey.RightArrow when col + 1 < props.FieldSize[1]:
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
                        MenuEnterEvent(props, props.SelectableRowCount, row);
                        props.Renderer.RenderBoard(props, row, col);
                        break;

                    case ConsoleKey.Enter:
                        if (props.Manager.CheckMove(props.GetDefenderField(), row, col)) props.Manager.MakeMove(props, row, col);
                        if (props.Winner != null) row = 0;
                        break;
                
                    case ConsoleKey.M:
                        row = props.SelectableRowCount;
                        props.Renderer.RenderBoard(props, row, 0);
                        break;
                }
            }
        }

        private void MenuEnterEvent(GameProperties props, int selectableFieldRowCount, int row) {
            /* Switch process description:
             4 - value of possible maximum count of menu items in game (step back, save, main menu, quit)
             MenuOptions.Count - current count of menu items
             row - current selected row (field height + current selected index in MenuOptions)
             selectableFieldRowCount - count of selectable rows for now:
                    Before 1 player wins: field height + MenuOptions.Count
                    After: MenuOptions.Count
            
            This flexible equation returns a value that will correspond to the desired item in the menu. */
            switch (4 - props.MenuOptions.Count + row - selectableFieldRowCount) { 
                case 0: // Step back
                    props.Manager.DoStepBack(props);
                    break;
                    
                case 1: // Save
                    props.Manager.SaveProgress(props);
                    break;

                case 2: // Main Menu
                    MenuManager newMenuManager = new MenuManager();
                    newMenuManager.Start();
                    break;

                case 3: // Quit
                    Environment.Exit(0);
                    break;
            }
        }
    }
}