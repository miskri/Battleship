using System.Collections.Generic;

namespace ConsoleApp {
    
    // Class for placing ships on the board
    public class BoardBuilder {

        private readonly GameManager _manager;
        private int _lastRow = 0, _lastCol = 0, _lastDirection = 0;
        
        public BoardBuilder(GameManager manager) {
            _manager = manager; // The game manager who called the ship placement
        }

        // Go through all ships from the settings and arrange them
        public void Start(Settings settings, BuilderProperties props) {
            props.PlayerFlotilla = new Flotilla();
            for (int i = 0; i < settings.ShipCount.Length; i++) { // ship count
                for (int j = 0; j < settings.ShipCount[i]; j++) { // ship size
                    props.CurrentShipName = props.ShipNames[i]; // ship name
                    props.CursorLength = settings.ShipSettings[i]; // set ship size
                    props.Direction = _lastDirection; // set direction
                    props.Renderer.RenderBoard(props, _lastRow, _lastCol); // render
                    props.EventListener.EventListener(props, _lastRow, _lastCol);
                }
            }
            _manager.SetPlayerField(props.PlayerField); // set field
            _manager.SetPlayerFlotilla(props.PlayerFlotilla); // set ship
        }
        
        // Does ship been placed or not
        public bool ShipPlacement(BuilderProperties props, int row, int col) {
            List<string> availableDirections = FieldManager.GetAvailableDirections(props.ShipArrangement, 
                props.Renderer.TemporaryField, row, col, props.CursorLength);
            string directionString = props.Direction == 0 ? "down" : "right";
            if (!availableDirections.Contains(directionString)) {
                return false;
            }

            Ship ship;
            // Put ship if you can
            (props.PlayerField, ship) = FieldManager.PutShip(props.ShipArrangement, props.PlayerField, 
                directionString, row, col, props.CursorLength);
            ship.Name = props.CurrentShipName;
            props.PlayerFlotilla.AddShip(ship);
            // Remember last coordinates
            _lastRow = row;
            _lastCol = col;
            _lastDirection = props.Direction;
            return true;
        }
        
    }
}