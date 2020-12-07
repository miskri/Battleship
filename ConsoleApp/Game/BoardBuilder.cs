using System.Collections.Generic;

namespace ConsoleApp {
    
    public class BoardBuilder {

        private readonly GameManager _manager;
        private readonly BoardRenderer _renderer;
        public bool Contact;
        private string[,] _field;
        private int _lastRow = 0, _lastCol = 0, _lastDirection = 0;

        public BuilderEventListener EventListener;
        
        public BoardBuilder(GameManager manager, BoardRenderer renderer, IReadOnlyList<int> battlefieldSize) {
            _manager = manager;
            _renderer = renderer;
            _field = new string[battlefieldSize[1], battlefieldSize[0]];
        }

        public void Start(int[] shipCount, int[] shipSettings) {
            for (int i = 0; i < shipCount.Length; i++) {
                for (int j = 0; j < shipCount[i]; j++) {
                    _renderer.RenderBoardBuilder(EventListener, _field, _lastRow, _lastCol, shipSettings[i], 
                        _lastDirection, Contact);
                }
            }
            _manager.SetPlayerField(_field);
        }
        
        public void ShipPlacement(string[,] shipField, int row, int col, int cursorLength, 
            int direction, int selectableFieldRowCount = 0) {
            List<string> availableDirections = FieldManager.GetAvailableDirections(Contact, 
                _renderer.TemporaryField, row, col, cursorLength);
            string directionString = direction == 0 ? "down" : "right";
            if (availableDirections.Contains(directionString)) {
                _field = FieldManager.PutShip(Contact, _field, 
                    directionString, row, col, cursorLength);
                _lastRow = row;
                _lastCol = col;
                _lastDirection = direction;
            }
            else {
                EventListener.EventListener(shipField, row, col, cursorLength, direction, selectableFieldRowCount);
            }
        }
        
    }
}