using System;

namespace ConsoleApp {
    public class BattleEventListener {
        private BoardRenderer _renderer;
        private BattleManager _battleManager;

        public BattleEventListener(BattleManager manager, BoardRenderer renderer) {
            _renderer = renderer;
            _battleManager = manager;
        }

        public void BoardEventListener(string[,] shipField, string[,] hitField, int selectorX, int selectorY, string currentPlayer) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.RightArrow when selectorY + 1 < hitField.GetLength(1):
                    _renderer.RenderBoard(shipField, hitField, selectorX, ++selectorY,
                        currentPlayer);
                    break;

                case ConsoleKey.LeftArrow when selectorY - 1 >= 0:
                    _renderer.RenderBoard(shipField, hitField, selectorX, --selectorY,
                        currentPlayer);
                    break;

                case ConsoleKey.UpArrow when selectorX - 1 >= 0:
                    _renderer.RenderBoard(shipField, hitField, --selectorX, selectorY,
                        currentPlayer);
                    break;

                case ConsoleKey.DownArrow when selectorX + 1 < hitField.GetLength(0) + 3:
                    _renderer.RenderBoard(shipField, hitField, ++selectorX, selectorY,
                        currentPlayer);
                    break;

                case ConsoleKey.Enter when selectorX >= shipField.GetLength(1):
                    switch (selectorX - shipField.GetLength(1)) // TODO extract method
                    {
                        case 0:
                            break;

                        case 1:
                            ConsoleMenu newMenu = new ConsoleMenu();
                            newMenu.Start();
                            break;
                        
                        case 2:
                            Environment.Exit(0);
                            break;
                    }
                    BoardEventListener(shipField, hitField, selectorX, selectorY, currentPlayer);
                    break;

                case ConsoleKey.Enter:
                    if (_battleManager.CanMakeThisMove(hitField, selectorX, selectorY)) {
                        _battleManager.MakeAMove(hitField, shipField, selectorX, selectorY);
                    }
                    BoardEventListener(shipField, hitField, selectorX, selectorY, currentPlayer);
                    break;

                default:
                    BoardEventListener(shipField, hitField, selectorX, selectorY, currentPlayer);
                    break;
            }
        }
    }
}