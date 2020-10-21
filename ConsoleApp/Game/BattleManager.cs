using System;

namespace ConsoleApp {
    public class BattleManager {
        private Random _rand = new Random();
        private BoardRenderer _renderer;
        private string[,] _playerOneField, _playerTwoField;
        private string _playerOneName, _playerTwoName;

        public BattleManager(BoardRenderer renderer, string[,] playerOneField, string[,] playerTwoField, 
            string playerOneName, string playerTwoName) {
            _renderer = renderer;
            _playerOneField = playerOneField;
            _playerTwoField = playerTwoField;
            _playerOneName = playerOneName;
            _playerTwoName = playerTwoName;
        }

        public void Start() {
            _renderer.RenderBoard(_playerOneField, _playerTwoField, 0, 0, _playerOneName);
        }

        public bool CanMakeThisMove(string[,] fieldToCheck, int x, int y) {
            return fieldToCheck[x, y] != "collision" && fieldToCheck[x, y] != "miss";
        }

        public void MakeAMove(string[,] attackedField, string[,] field, int x, int y) {
            if (attackedField[x, y] == "ship") {
                attackedField[x, y] = "collision";
                _renderer.BattleHistory.Add(
                    $"{_playerOneName} hit {_renderer.Alphabet[y]}{x + 1}. Result - ship damaged!");
            }
            else {
                attackedField[x, y] = "miss";
                _renderer.BattleHistory.Add($"{_playerOneName} hit {_renderer.Alphabet[y]}{x + 1}. Result - miss!");
            }

            field = AiMove(field);
            _renderer.RenderBoard(field, attackedField, x, y, _playerOneName);
        }

        private string[,] AiMove(string[,] field) {
            bool moveIsDone = false;
            while (!moveIsDone) {
                int x = _rand.Next(0, field.GetLength(0));
                int y = _rand.Next(0, field.GetLength(1));
                if (field[x, y] == "ship") {
                    field[x, y] = "collision";
                    _renderer.BattleHistory.Add(
                        $"{_playerTwoName} hit {_renderer.Alphabet[y]}{x + 1}. Result - ship damaged!");
                    moveIsDone = true;
                }
                else if (field[x, y] == null || field[x, y] == "border") {
                    field[x, y] = "miss";
                    _renderer.BattleHistory.Add($"{_playerTwoName} hit {_renderer.Alphabet[y]}{x + 1}. Result - miss!");
                    moveIsDone = true;
                }
            }
            return field;
        }
    }
}