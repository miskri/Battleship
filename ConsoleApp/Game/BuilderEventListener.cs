using System;
using System.Collections.Generic;

namespace ConsoleApp {
    public class BuilderEventListener {
        private BoardRenderer _renderer;
        private BoardBuilder _boardBuilder;

        public BuilderEventListener(BoardRenderer renderer, BoardBuilder boardBuilder) {
            _renderer = renderer;
            _boardBuilder = boardBuilder;
        }
        
        public void EventListener(string[,] shipField, int row, int col, int cursorLength, 
            int direction, int selectableFieldRowCount = 0) {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                
                case ConsoleKey.RightArrow when col + 1 < shipField.GetLength(1):
                    _renderer.RenderBoardBuilder(this, shipField, row, ++col, cursorLength, direction,
                        _boardBuilder.Contact);
                    break;

                case ConsoleKey.LeftArrow when col - 1 >= 0:
                    _renderer.RenderBoardBuilder(this, shipField, row, --col, cursorLength, direction,
                        _boardBuilder.Contact);
                    break;

                case ConsoleKey.UpArrow when row - 1 >= 0:
                    _renderer.RenderBoardBuilder(this, shipField, --row, col, cursorLength, direction,
                        _boardBuilder.Contact);
                    break;

                case ConsoleKey.DownArrow when row + 1 < selectableFieldRowCount + _renderer.MenuOptions.Count:
                    _renderer.RenderBoardBuilder(this, shipField, ++row, col, cursorLength, direction,
                        _boardBuilder.Contact);
                    break;

                case ConsoleKey.Enter when row >= selectableFieldRowCount:
                    MenuEnterEvent(selectableFieldRowCount, row);
                    _renderer.RenderBoardBuilder(this, shipField, row, col, cursorLength, direction,
                        _boardBuilder.Contact);
                    break;
                
                case ConsoleKey.R:
                    int newDirection = direction == 0 ? 1 : 0;
                    _renderer.RenderBoardBuilder(this, shipField, row, col, cursorLength, newDirection,
                        _boardBuilder.Contact);
                    break;
                
                case ConsoleKey.Enter:
                    ShipPlacement(shipField, row, col, cursorLength, direction, selectableFieldRowCount);
                    break;
                
                case ConsoleKey.M:
                    _renderer.RenderBoardBuilder(this, shipField, selectableFieldRowCount, 0, 
                        cursorLength, direction, _boardBuilder.Contact);
                    break;
                
                default:
                    EventListener(shipField, row, col, cursorLength, direction, selectableFieldRowCount);
                    break;
            }
        }
        
        private void MenuEnterEvent(int selectableFieldRowCount, int row) {
            switch (row - selectableFieldRowCount) {
                case 0:
                    MenuManager newMenuManager = new MenuManager();
                    newMenuManager.Start();
                    break;

                case 1:
                    Environment.Exit(0);
                    break;
            }
        }

        private void ShipPlacement(string[,] shipField, int row, int col, int cursorLength, 
            int direction, int selectableFieldRowCount = 0) {
            List<string> availableDirections = FieldManager.CheckPosition(_boardBuilder.Contact, 
                _renderer.TemporaryField, row, col, cursorLength);
            string directionString = direction == 0 ? "down" : "right";
            if (availableDirections.Contains(directionString)) {
                _boardBuilder.Field = FieldManager.PutShip(_boardBuilder.Contact, _boardBuilder.Field, 
                    directionString, row, col, cursorLength);
                _boardBuilder.LastRow = row;
                _boardBuilder.LastCol = col;
                _boardBuilder.LastDirection = direction;
            }
            else {
                EventListener(shipField, row, col, cursorLength, direction, selectableFieldRowCount);
            }
        }
        
    }
}