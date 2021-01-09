using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp.Control;
using ConsoleApp.Data;
using ConsoleApp.Objects;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Game {
    public class FastGame : PageModel {

        private readonly AppDbContext ctx;
        public GameProperties Props;
        public int GameId;

        public FastGame(AppDbContext ctx) {
            this.ctx = ctx;
        }
        
        public async Task<ActionResult> OnGetAsync(int? gameId, int? row, int? col) {
            if (gameId == null) return RedirectToPage("/");
            GameId = gameId.Value;

            var save = await ctx.Saves.FirstAsync(x => x.SaveObjectId == gameId.Value);
            
            var propsDb = ctx.Properties.First(x => x.BattlePropertiesObjectId == save.BattlePropertiesObjectId);
            var test = ctx.PropertiesFlotillas.Where(x => x.BattleId == propsDb.BattlePropertiesObjectId).ToList();
            var flotilla1 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[1].FlotillaId);
            var flotilla2 = ctx.Flotillas.First(x => x.BattleFlotillasObjectId == test[0].FlotillaId);
            var ships1 = ctx.Ships.Where(x => x.FlotillaId == flotilla1.BattleFlotillasObjectId).ToList();
            var ships2 = ctx.Ships.Where(x => x.FlotillaId == flotilla2.BattleFlotillasObjectId).ToList();

            Flotilla player1Flotilla = new Flotilla {
                Destroyed = false, 
                FlotillaHealth = flotilla1.FlotillaHealth, 
                Ships = new List<Ship>(), 
                Size = flotilla1.Size
            };
            
            Flotilla player2Flotilla = new Flotilla {
                Destroyed = false, 
                FlotillaHealth = flotilla2.FlotillaHealth, 
                Ships = new List<Ship>(), 
                Size = flotilla2.Size
            };

            if (player1Flotilla.FlotillaHealth == 0 || player2Flotilla.FlotillaHealth == 0) return Page();

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

            Props = new GameProperties {
                Id = propsDb.GameId,
                GameMode = propsDb.GameMode,
                Player1Name = propsDb.Player1Name,
                Player2Name = propsDb.Player2Name,
                Player1Flotilla = player1Flotilla,
                Player2Flotilla = player2Flotilla,
                FieldSize = JsonSerializer.Deserialize<int[]>(propsDb.FieldSize),
                Player1FieldArray = JsonSerializer.Deserialize<string[]>(propsDb.Player1FieldArray),
                Player2FieldArray = JsonSerializer.Deserialize<string[]>(propsDb.Player2FieldArray),
                CurrentPlayer = propsDb.CurrentPlayer,
                Round = propsDb.Round,
                SelectableRowCount = propsDb.SelectableRowCount,
                BattleHistory = JsonSerializer.Deserialize<List<string>>(propsDb.BattleHistory),
                MenuOptions = JsonSerializer.Deserialize<List<string>>(propsDb.MenuOptions)
            };
            
            Props.LoadPlayer1FieldFromArray();
            Props.LoadPlayer2FieldFromArray();
            Props.Player1Flotilla.LoadShipFromArrays();
            Props.Player2Flotilla.LoadShipFromArrays();

            if (row != null && col != null) {
                BattleManager manager = new BattleManager();
                Props.Manager = manager;
                if (manager.CheckMove(Props.Player2Field, row.Value, col.Value)) { 
                    manager.MakeMove(Props, row.Value, col.Value);
                    Props.LoadPlayer1FieldToArray();
                    Props.LoadPlayer2FieldToArray();
                    manager.TimeManager.SaveRoundHistory(Props.Id);
                } 
                if (Props.Winner != null) {
                    return Page();
                }
            }

            // REWRITE BASE INFO

            flotilla1.Size = Props.Player1Flotilla.Size;
            flotilla1.FlotillaHealth = Props.Player1Flotilla.FlotillaHealth;
            
            flotilla2.Size = Props.Player2Flotilla.Size;
            flotilla2.FlotillaHealth = Props.Player2Flotilla.FlotillaHealth;

            int index = 0;
            while (index < Props.Player1Flotilla.Ships.Count) {
                var shipFromDb = ships1[index];
                var shipFromGame = Props.Player1Flotilla.Ships[index];
                shipFromDb.Name = shipFromGame.Name;
                shipFromDb.Size = shipFromGame.Size;
                shipFromDb.Health = shipFromGame.Health;
                shipFromDb.ShipCellsArray = JsonSerializer.Serialize(shipFromGame.ShipCellsArray);
            }

            index = 0;
            while (index < Props.Player2Flotilla.Ships.Count) {
                var shipFromDb = ships2[index];
                var shipFromGame = Props.Player2Flotilla.Ships[index++];
                shipFromDb.Name = shipFromGame.Name;
                shipFromDb.Size = shipFromGame.Size;
                shipFromDb.Health = shipFromGame.Health;
                shipFromDb.ShipCellsArray = JsonSerializer.Serialize(shipFromGame.ShipCellsArray);
            }

            propsDb.Player1FieldArray = JsonSerializer.Serialize(Props.Player1FieldArray);
            propsDb.Player2FieldArray = JsonSerializer.Serialize(Props.Player2FieldArray);
            propsDb.CurrentPlayer = Props.CurrentPlayer;
            propsDb.Round = Props.Round;
            propsDb.BattleHistory = JsonSerializer.Serialize(Props.BattleHistory);
            propsDb.MenuOptions = JsonSerializer.Serialize(Props.MenuOptions);
            
            await ctx.SaveChangesAsync();
            
            return Page();
        }
    }
}