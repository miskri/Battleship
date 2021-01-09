using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp.Data;
using ConsoleApp.Graphics;

namespace ConsoleApp.Objects {
    
    // This class manages ships and loads them from save
    [Serializable] public class Flotilla {

        public bool Destroyed = false;
        public List<Ship> Ships { get; set; }
        public int ShipCount { get; set; }
        public int Size { get; set; }
        public int FlotillaHealth { get; set; }

        public Flotilla() {}
        public void AddShip(Ship ship) {
            if (Ships == null) Ships = new List<Ship>();
            Ships.Add(ship);
            ShipCount++;
            Size += ship.Size;
            FlotillaHealth += ship.Health;
        }

        // Set damage to ship, if its destroyed remove it from flotilla, if it was the last one put bool destroyed on flotilla
        public void SetDamage(GameProperties props, int row, int col) {
            foreach (Ship ship in Ships.Where(ship => ship.ShipCells.Contains((row, col)))) {
                if (ship.SetHit() == 0) {
                    string msg = $"{Color.YellowText}Round {props.Round} - {props.CurrentPlayer}{Color.RedText} " +
                                 $"destroys {Color.YellowText}{props.GetDefenderName()} {ship.Name}!";
                    props.Manager.AddToBattleHistory(props, message: msg);
                    ShipCount--;
                }

                if (--FlotillaHealth == 0) Destroyed = true;
                break;
            }
        }

        public void LoadShipFromArrays() {
            foreach (Ship ship in Ships) {
                ship.LoadShipCellsFromArray();
            }
        }
    }
}