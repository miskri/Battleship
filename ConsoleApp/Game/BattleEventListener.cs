using System;

namespace ConsoleApp {
    public class BattleEventListener {
        private readonly BattleManager _manager;
        
        public BattleEventListener(BattleManager manager) {
            _manager = manager;
        }

        public void EventListener(string[,] shipField, string[,] hitField, int row, int col, string currentPlayer, 
            int selectableFieldRowCount = 0) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.RightArrow when col + 1 < shipField.GetLength(1):
                    _manager.Renderer.RenderBoard(shipField, hitField, row, ++col, 
                        currentPlayer, this);
                    break;

                case ConsoleKey.LeftArrow when col - 1 >= 0:
                    _manager.Renderer.RenderBoard(shipField, hitField, row, --col, 
                        currentPlayer, this);
                    break;

                case ConsoleKey.UpArrow when row - 1 >= 0:
                    _manager.Renderer.RenderBoard(shipField, hitField, --row, col, 
                        currentPlayer, this);
                    break;

                case ConsoleKey.DownArrow when row + 1 < selectableFieldRowCount + _manager.Renderer.MenuOptions.Count:
                    _manager.Renderer.RenderBoard(shipField, hitField, ++row, col, 
                        currentPlayer, this);
                    break;

                case ConsoleKey.Enter when row >= selectableFieldRowCount:
                    MenuEnterEvent(selectableFieldRowCount, row);
                    _manager.Renderer.RenderBoard(shipField, hitField, row, col, currentPlayer, this);
                    break;

                case ConsoleKey.Enter:
                    if (_manager.CheckMove(hitField, row, col)) { 
                        _manager.MakeMove(hitField, shipField, row, col);
                    }
                    EventListener(shipField, hitField, row, col, currentPlayer, selectableFieldRowCount);
                    break;
                
                case ConsoleKey.M:
                    _manager.Renderer.RenderBoard(shipField, hitField, selectableFieldRowCount, 
                        0, currentPlayer, this);
                    break;

                default:
                    EventListener(shipField, hitField, row, col, currentPlayer, selectableFieldRowCount);
                    break;
            }
        }

        private void MenuEnterEvent(int selectableFieldRowCount, int row) {
            switch (3 - _manager.Renderer.MenuOptions.Count + row - selectableFieldRowCount) {
                case 0:
                    _manager.SaveProgress();
                    break;

                case 1:
                    MenuManager newMenuManager = new MenuManager();
                    newMenuManager.Start();
                    break;

                case 2:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}