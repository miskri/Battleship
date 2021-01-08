namespace Domain.Objects {
    
    public class SaveObject {
        
        public int SaveObjectId { get; set; }
        public string SaveName { get; set; }
        public int BattlePropertiesObjectId { get; set; }
        public BattlePropertiesObject? BattleProperties { get; set; }
    }
}