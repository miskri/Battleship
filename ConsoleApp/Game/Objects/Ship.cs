using System;

namespace ConsoleApp.Objects {
    
    [Serializable] public class Ship {

        public string Name { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        [NonSerialized] public (int, int)[] ShipCells;
        public int[] ShipCellsArray { get; set; }
        
        public Ship() {}
        
        public Ship((int, int)[] shipCells) {
            Size = shipCells.Length;
            Health = Size;
            ShipCells = shipCells;
            ShipCellsArray = new int[Size * 2];
            for (int i = 0; i < Size * 2; i++) {
                ShipCellsArray[i] = i % 2 == 0 ? ShipCells[i / 2].Item1 : ShipCells[i / 2].Item2;
            }
        }

        public int SetHit() {
            return --Health;
        }
        
        public void LoadShipCellsFromArray() {
            ShipCells = new (int, int)[ShipCellsArray.Length / 2];
            for (int i = 0; i < ShipCellsArray.Length; i += 2) {
                ShipCells[i / 2] = (ShipCellsArray[i], ShipCellsArray[i + 1]);
            }
        }
    }
}