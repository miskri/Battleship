using System;

namespace ConsoleApp {
    public class GameEventListener {
        private BoardRenderer _renderer;
        private BattleManager _battleManager;
        
        public GameEventListener(BattleManager manager, BoardRenderer renderer) {
            _renderer = renderer;
            _battleManager = manager;
        }

        public void EventListener(string[,] shipField, string[,] hitField, int row, int col, string currentPlayer, 
            int selectableFieldRowCount = 0) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.RightArrow when col + 1 < shipField.GetLength(1):
                    _renderer.RenderBoard(shipField, hitField,
                        row, ++col, currentPlayer, this);
                    break;

                case ConsoleKey.LeftArrow when col - 1 >= 0:
                    _renderer.RenderBoard(shipField, hitField,
                        row, --col, currentPlayer, this);
                    break;

                case ConsoleKey.UpArrow when row - 1 >= 0:
                    _renderer.RenderBoard(shipField, hitField,
                        --row, col, currentPlayer, this);
                    break;

                case ConsoleKey.DownArrow when row + 1 < selectableFieldRowCount + _renderer.MenuOptions.Count:
                    _renderer.RenderBoard(shipField, hitField,
                        ++row, col, currentPlayer, this);
                    break;

                case ConsoleKey.Enter when row >= selectableFieldRowCount:
                    MenuEnterEvent(selectableFieldRowCount, row);
                    _renderer.RenderBoard(shipField, hitField,
                        row, col, currentPlayer, this);
                    break;

                case ConsoleKey.Enter:
                    if (_battleManager.CheckMove(hitField, row, col)) { 
                        _battleManager.MakeMove(hitField, shipField, row, col);
                    }
                    EventListener(shipField, hitField, row, col, currentPlayer, selectableFieldRowCount);
                    break;
                
                case ConsoleKey.M:
                    _renderer.RenderBoard(shipField, hitField,
                        selectableFieldRowCount, 0, currentPlayer, this);
                    break;

                default:
                    EventListener(shipField, hitField, row, col, currentPlayer, selectableFieldRowCount);
                    break;
            }
        }

        private void MenuEnterEvent(int selectableFieldRowCount, int row) {
            switch (3 - _renderer.MenuOptions.Count + row - selectableFieldRowCount) {
                case 0:
                    SaveProgress();
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

        private void SaveProgress() {
            MenuLevelDataContainer.Saves.Add($"{_renderer.GameMode} - {DateTime.Now}");
            _renderer.BattleHistory.Add("Game was saved successfully!");
        }
    }
}