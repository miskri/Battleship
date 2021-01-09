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
            var flotillaInDb1 = new BattleFlotillasObject {
                Size = flotilla1.Size,
                FlotillaHealth = flotilla1.FlotillaHealth,
                ShipCount = flotilla1.ShipCount
            };
            Flotilla flotilla2 = save.Properties.Player2Flotilla;
            var flotillaInDb2 = new BattleFlotillasObject {
                Size = flotilla2.Size,
                FlotillaHealth = flotilla2.FlotillaHealth,
                ShipCount = flotilla2.ShipCount
            };

            ctx.Flotillas.Add(flotillaInDb1);
            ctx.Flotillas.Add(flotillaInDb2);
            ctx.SaveChanges();

            foreach (Ship ship in flotilla1.Ships) {
                var shipInDb = new BattleShipsObject {
                    FlotillaId = flotillaInDb1.BattleFlotillasObjectId,
                    Name = ship.Name,
                    Size = ship.Size,
                    Health = ship.Health,
                    ShipCellsArray = JsonSerializer.Serialize(ship.ShipCellsArray),
                };
                ctx.Add(shipInDb);
            }
            
            foreach (Ship ship in flotilla2.Ships) {
                var shipInDb = new BattleShipsObject {
                    FlotillaId = flotillaInDb2.BattleFlotillasObjectId,
                    Name = ship.Name,
                    Size = ship.Size,
                    Health = ship.Health,
                    ShipCellsArray = JsonSerializer.Serialize(ship.ShipCellsArray),
                };
                ctx.Add(shipInDb);
            }
            ctx.SaveChanges();
            
            var propertiesInDb = new BattlePropertiesObject {
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
            // TODO refactor
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

            Flotilla player1Flotilla = new Flotilla {
                Destroyed = false, 
                FlotillaHealth = flotilla1.FlotillaHealth, 
                ShipCount = flotilla1.ShipCount,
                Ships = new List<Ship>(), 
                Size = flotilla1.Size
            };
            
            Flotilla player2Flotilla = new Flotilla {
                Destroyed = false, 
                FlotillaHealth = flotilla2.FlotillaHealth, 
                ShipCount = flotilla2.ShipCount,
                Ships = new List<Ship>(), 
                Size = flotilla2.Size
            };

            foreach (var ship in ships1) {
                player1Flotilla.Ships.Add(new Ship {
                    Health = ship.Health,
                    Name = ship.Name,
                    ShipCellsArray = JsonSerializer.Deserialize<int[]>(ship.ShipCellsArray),
                    Size = ship.Size
                });
            }
            
            foreach (var ship in ships2) {
                player2Flotilla.Ships.Add(new Ship {
                    Health = ship.Health,
                    Name = ship.Name,
                    ShipCellsArray = JsonSerializer.Deserialize<int[]>(ship.ShipCellsArray),
                    Size = ship.Size
                });
            }

            GameProperties properties = new GameProperties {
                Id = props.GameId,
                GameMode = props.GameMode,
                Player1Name = props.Player1Name,
                Player2Name = props.Player2Name,
                Player1Flotilla = player1Flotilla,
                Player2Flotilla = player2Flotilla,
                FieldSize = JsonSerializer.Deserialize<int[]>(props.FieldSize),
                Player1FieldArray = JsonSerializer.Deserialize<string[]>(props.Player1FieldArray),
                Player2FieldArray = JsonSerializer.Deserialize<string[]>(props.Player2FieldArray),
                CurrentPlayer = props.CurrentPlayer,
                Round = props.Round,
                SelectableRowCount = props.SelectableRowCount,
                BattleHistory = JsonSerializer.Deserialize<List<string>>(props.BattleHistory),
                MenuOptions = JsonSerializer.Deserialize<List<string>>(props.MenuOptions)
            };
            
            Save saveFromDb = new Save(save.SaveName, properties);
            return saveFromDb;
        }

        private static void DeleteFromDb(string saveName) {
            using AppDbContext ctx = new AppDbContext();

            SaveObject save = ctx.Saves.FirstOrDefault(x => x.SaveName == saveName);
            if (save == null) return;
            
            var props = ctx.Properties.First(x => x.BattlePropertiesObjectId == save.BattlePropertiesObjectId);
            var test = ctx.PropertiesFlotillas.Where(x => x.BattleId == props.BattlePropertiesObjectId).ToList();
            var flotilla1 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[0].FlotillaId);
            var flotilla2 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[1].FlotillaId);
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