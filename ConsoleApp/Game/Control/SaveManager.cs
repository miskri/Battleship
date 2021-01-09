using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConsoleApp.Data;
using ConsoleApp.Objects;
using ConsoleApp.Utils;
using DAL;
using Domain.Objects;

namespace ConsoleApp.Control {
    
    public static class SaveManager {

        public static List<Save> Saves;
        private static string _savePath = "C:\\Users\\Mihhail\\RiderProjects\\icd0008-2020f\\ConsoleApp\\Game\\Resources\\saves.json";

        public static List<string> GetSavesList() {
            if (Saves == null) LoadSaves();
            List<string> saveNames = new List<string>();
            if (Saves != null && Saves.Count > 0) saveNames = Saves.Select(save => save.SaveName).ToList();
            saveNames.Reverse();
            return saveNames;
        }

        public static Save GetSave(string saveName) {
            if (Saves == null) LoadSaves();
            Save dbSave = LoadFromDb(saveName);
            return dbSave ?? Saves?.FirstOrDefault(save => save.SaveName == saveName).DeepClone();
        }

        public static Save GetSaveReference(string saveName) {
            if (Saves == null) LoadSaves();
            return Saves?.FirstOrDefault(save => save.SaveName == saveName);
        }

        public static int SaveGame(Save save) {
            if (Saves == null) LoadSaves();
            Saves?.Add(save);
            SaveChanges();
            return SaveToDb(save);
        }

        public static void DeleteSavedGame(Save save) {
            if (Saves == null) LoadSaves();
            Saves?.Remove(Saves.FirstOrDefault(x => x.SaveName == save.SaveName));
            SaveChanges();
            DeleteFromDb(save.SaveName);
        }
        
        public static void LoadSaves() {
            Saves = new List<Save>();
            string path = _savePath;
            StreamReader saveFileReader = new StreamReader(path);
            string jsonString = saveFileReader.ReadToEnd();
            Save[] savesArray = JsonSerializer.Deserialize<Save[]>(jsonString);
            Saves = savesArray.ToList();
            saveFileReader.Close();
        }

        private static void SaveChanges() {
            string path = _savePath;
            StreamWriter saveFileWriter = new StreamWriter(path);
            string jsonString = JsonSerializer.Serialize(Saves, DataUtils.JsonOptions);
            saveFileWriter.Write(jsonString);
            saveFileWriter.Close();
        }

        public static bool ContainsThisName(string name) {
            if (Saves == null) LoadSaves();
            return Saves.Any(save => save.SaveName == name);
        }

        private static int SaveToDb(Save save) {
            using AppDbContext ctx = new AppDbContext();

            GameProperties props = save.Properties;
            
            Flotilla flotilla1 = props.Player1Flotilla;
            BattleFlotillasObject flotillaInDb1 = GetBattleFlotilla(flotilla1);
            
            Flotilla flotilla2 = save.Properties.Player2Flotilla;
            BattleFlotillasObject flotillaInDb2 = GetBattleFlotilla(flotilla2);

            ctx.Flotillas.Add(flotillaInDb1);
            ctx.Flotillas.Add(flotillaInDb2);
            ctx.SaveChanges();

            foreach (Ship ship in flotilla1.Ships) {
                BattleShipsObject shipInDb = GetBattleShip(flotillaInDb1, ship);
                ctx.Add(shipInDb);
            }
            
            foreach (Ship ship in flotilla2.Ships) {
                BattleShipsObject shipInDb = GetBattleShip(flotillaInDb2, ship);
                ctx.Add(shipInDb);
            }
            ctx.SaveChanges();

            BattlePropertiesObject propertiesInDb = GetBattleProps(props);

            ctx.Add(propertiesInDb);
            ctx.SaveChanges();
            
            var propertiesFlotillasInDb1 = new PropertiesFlotillasObject {
                BattleId = propertiesInDb.BattlePropertiesObjectId,
                FlotillaId = flotillaInDb1.BattleFlotillasObjectId
            };
            ctx.Add(propertiesFlotillasInDb1);
            
            var propertiesFlotillasInDb2 = new PropertiesFlotillasObject {
                BattleId = propertiesInDb.BattlePropertiesObjectId,
                FlotillaId = flotillaInDb2.BattleFlotillasObjectId
            };
            ctx.Add(propertiesFlotillasInDb2);
            ctx.SaveChanges();

            var saveInDb = new SaveObject {
                SaveName = save.SaveName,
                BattlePropertiesObjectId = propertiesInDb.BattlePropertiesObjectId
            };

            ctx.Add(saveInDb);
            ctx.SaveChanges();

            return saveInDb.SaveObjectId;
        }

        private static BattleFlotillasObject GetBattleFlotilla(Flotilla flotilla) {
            BattleFlotillasObject flotillaObject = new BattleFlotillasObject {
                Size = flotilla.Size,
                FlotillaHealth = flotilla.FlotillaHealth,
                ShipCount = flotilla.ShipCount
            };
            return flotillaObject;
        }

        private static BattleShipsObject GetBattleShip(BattleFlotillasObject flotillaInDb, Ship ship) {
            BattleShipsObject shipInDb = new BattleShipsObject {
                FlotillaId = flotillaInDb.BattleFlotillasObjectId,
                Name = ship.Name,
                Size = ship.Size,
                Health = ship.Health,
                ShipCellsArray = JsonSerializer.Serialize(ship.ShipCellsArray),
            };
            return shipInDb;
        }

        private static BattlePropertiesObject GetBattleProps(GameProperties props) {
            BattlePropertiesObject propertiesInDb = new BattlePropertiesObject {
                GameMode = props.GameMode,
                GameId = props.Id,
                Player1Name = props.Player1Name,
                Player2Name = props.Player2Name,
                FieldSize =  JsonSerializer.Serialize(props.FieldSize),
                Player1FieldArray = JsonSerializer.Serialize(props.Player1FieldArray),
                Player2FieldArray = JsonSerializer.Serialize(props.Player2FieldArray),
                CurrentPlayer = props.CurrentPlayer,
                Round = props.Round,
                SelectableRowCount = props.SelectableRowCount,
                BattleHistory = JsonSerializer.Serialize(props.BattleHistory),
                MenuOptions = JsonSerializer.Serialize(props.MenuOptions)
            };
            return propertiesInDb;
        }

        private static Save LoadFromDb(string saveName) {
            using AppDbContext ctx = new AppDbContext();

            SaveObject save = ctx.Saves.FirstOrDefault(x => x.SaveName == saveName);
            if (save == null) return null;
            
            var props = ctx.Properties.First(x => x.BattlePropertiesObjectId == save.BattlePropertiesObjectId);
            var test = ctx.PropertiesFlotillas.Where(x => x.BattleId == props.BattlePropertiesObjectId).ToList();
            var flotilla1 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[1].FlotillaId);
            var flotilla2 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[0].FlotillaId);
            var ships1 = ctx.Ships.Where(x => x.FlotillaId == flotilla1.BattleFlotillasObjectId).ToList();
            var ships2 = ctx.Ships.Where(x => x.FlotillaId == flotilla2.BattleFlotillasObjectId).ToList();

            Flotilla player1Flotilla = GetFlotilla(flotilla1);
            Flotilla player2Flotilla = GetFlotilla(flotilla2);
            
            
            FillFlotilla(player1Flotilla, ships1);
            FillFlotilla(player2Flotilla, ships2);

            GameProperties properties = GetGameProps(props, player1Flotilla, player2Flotilla);
            
            Save saveFromDb = new Save(save.SaveName, properties);
            return saveFromDb;
        }

        public static Flotilla GetFlotilla(BattleFlotillasObject flotillaInDb) {
            Flotilla flotilla = new Flotilla {
                Destroyed = false, 
                FlotillaHealth = flotillaInDb.FlotillaHealth, 
                ShipCount = flotillaInDb.ShipCount,
                Ships = new List<Ship>(), 
                Size = flotillaInDb.Size
            };
            return flotilla;
        }

        public static void FillFlotilla(Flotilla flotilla, IEnumerable<BattleShipsObject> ships) {
            foreach (BattleShipsObject ship in ships) {
                flotilla.Ships.Add(new Ship {
                    Health = ship.Health,
                    Name = ship.Name,
                    ShipCellsArray = JsonSerializer.Deserialize<int[]>(ship.ShipCellsArray),
                    Size = ship.Size
                });
            }
        }

        public static GameProperties GetGameProps(BattlePropertiesObject propsInDb, Flotilla flotilla1, Flotilla flotilla2) {
            GameProperties properties = new GameProperties {
                Id = propsInDb.GameId,
                GameMode = propsInDb.GameMode,
                Player1Name = propsInDb.Player1Name,
                Player2Name = propsInDb.Player2Name,
                Player1Flotilla = flotilla1,
                Player2Flotilla = flotilla2,
                FieldSize = JsonSerializer.Deserialize<int[]>(propsInDb.FieldSize),
                Player1FieldArray = JsonSerializer.Deserialize<string[]>(propsInDb.Player1FieldArray),
                Player2FieldArray = JsonSerializer.Deserialize<string[]>(propsInDb.Player2FieldArray),
                CurrentPlayer = propsInDb.CurrentPlayer,
                Round = propsInDb.Round,
                SelectableRowCount = propsInDb.SelectableRowCount,
                BattleHistory = JsonSerializer.Deserialize<List<string>>(propsInDb.BattleHistory),
                MenuOptions = JsonSerializer.Deserialize<List<string>>(propsInDb.MenuOptions)
            };
            return properties;
        }

        private static void DeleteFromDb(string saveName) {
            using AppDbContext ctx = new AppDbContext();

            SaveObject save = ctx.Saves.FirstOrDefault(x => x.SaveName == saveName);
            if (save == null) return;
            
            var props = ctx.Properties.First(x => x.BattlePropertiesObjectId == save.BattlePropertiesObjectId);
            var test = ctx.PropertiesFlotillas.Where(x => x.BattleId == props.BattlePropertiesObjectId).ToList();
            var flotilla1 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[1].FlotillaId);
            var flotilla2 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[0].FlotillaId);
            var ships1 = ctx.Ships.Where(x => x.BattleShipsObjectId == flotilla1.BattleFlotillasObjectId).ToList();
            var ships2 = ctx.Ships.Where(x => x.BattleShipsObjectId == flotilla2.BattleFlotillasObjectId).ToList();
            
            ctx.Ships.RemoveRange(ships1);
            ctx.Ships.RemoveRange(ships2);
            ctx.Flotillas.Remove(flotilla1);
            ctx.Flotillas.Remove(flotilla2);
            ctx.PropertiesFlotillas.RemoveRange(test);
            ctx.Properties.Remove(props);
            ctx.Saves.Remove(save);
            ctx.SaveChanges();
        }
    }
}