using System.Collections.Generic;

namespace Domain.Objects {
    
    public class BattlePropertiesObject {
        
        public int BattlePropertiesObjectId { get; set; }
        public string GameId { get; set; }
        public string GameMode { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string FieldSize { get; set; }
        public string Player1FieldArray { get; set; }
        public string Player2FieldArray { get; set; }
        public string CurrentPlayer { get; set; }
        public int Round { get; set; }
        public int SelectableRowCount { get; set; }
        public string BattleHistory { get; set; }
        public string MenuOptions { get; set; }
        public ICollection<PropertiesFlotillasObject>? PropertiesFlotillas { get; set; }
    }
}