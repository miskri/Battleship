using System.Collections.Generic;

namespace Domain.Objects {
    
    public class BattleShipsObject {
        
        public int BattleShipsObjectId { get; set; }
        public int FlotillaId { get; set; }
        public BattleFlotillasObject? Flotilla { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public string ShipCellsArray { get; set; }
        public ICollection<BattleFlotillasObject>? BattleFlotillas { get; set; }
    }
}