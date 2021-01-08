using System.Collections.Generic;

namespace Domain.Objects {
    
    public class PropertiesFlotillasObject {
        
        public int PropertiesFlotillasObjectId { get; set; }
        public int BattleId { get; set; }
        public BattlePropertiesObject? Battle { get; set; }
        public int FlotillaId { get; set; }
        public BattleFlotillasObject? Flotilla { get; set; }
        public ICollection<BattleFlotillasObject>? BattleFlotillas { get; set; }
    }
}