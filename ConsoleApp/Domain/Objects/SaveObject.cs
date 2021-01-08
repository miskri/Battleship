namespace Domain.Objects {
    
    public class SaveObject {
        
        public string SaveObjectId { get; set; }
        public string SaveName { get; set; }
        public string BattlePropertiesObjectId { get; set; }
        public BattlePropertiesObject? BattleProperties { get; set; }
    }
}