using System.Collections.Generic;

namespace ConsoleApp {
    
    public class Flotilla {
        
        public bool Destroyed = false;
        public List<Ship> Ships { get; private set; }
        public int Size { get; private set; }
        public int TotalHealth { get; private set; }

        public void AddShip(Ship ship) {
            Ships.Add(ship);
            Size += ship.Size;
            TotalHealth += ship.Health;
        }

        public void SetDamage(int row, int col) {
            
        }
    }
}