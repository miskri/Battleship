using System.Collections.Generic;

namespace Domain.Objects {
    
    public class PropertiesFlotillasObject {
        
        public string PropertiesFlotillasObjectId { get; set; }
        public string BattleId { get; set; }
        public BattlePropertiesObject? Battle { get; set; }
        public string FlotillaId { get; set; }
        public BattleFlotillasObject? Flotilla { get; set; }
        public ICollection<BattleFlotillasObject>? BattleFlotillas { get; set; }
    }
}