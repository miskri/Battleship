using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp.Control;
using ConsoleApp.Data;
using ConsoleApp.Objects;
using DAL;
using Domain.Objects;
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

            Flotilla player1Flotilla = SaveManager.GetFlotilla(flotilla1);
            Flotilla player2Flotilla = SaveManager.GetFlotilla(flotilla2);

            if (player1Flotilla.Destroyed || player2Flotilla.Destroyed) return Page();

            SaveManager.FillFlotilla(player1Flotilla, ships1);
            SaveManager.FillFlotilla(player2Flotilla, ships2);

            Props = SaveManager.GetGameProps(propsDb, player1Flotilla, player2Flotilla);
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
            flotilla1.ShipCount = Props.Player1Flotilla.ShipCount;
            
            flotilla2.Size = Props.Player2Flotilla.Size;
            flotilla2.FlotillaHealth = Props.Player2Flotilla.FlotillaHealth;
            flotilla2.ShipCount = Props.Player2Flotilla.ShipCount;
            
            UpdateShips(Props.Player1Flotilla, ships1);
            UpdateShips(Props.Player2Flotilla, ships2);

            propsDb.Player1FieldArray = JsonSerializer.Serialize(Props.Player1FieldArray);
            propsDb.Player2FieldArray = JsonSerializer.Serialize(Props.Player2FieldArray);
            propsDb.CurrentPlayer = Props.CurrentPlayer;
            propsDb.Round = Props.Round;
            propsDb.BattleHistory = JsonSerializer.Serialize(Props.BattleHistory);
            propsDb.MenuOptions = JsonSerializer.Serialize(Props.MenuOptions);
            
            await ctx.SaveChangesAsync();
            
            return Page();
        }

        private void UpdateShips(Flotilla flotilla, IReadOnlyList<BattleShipsObject> ships) {
            int index = 0;
            while (index < flotilla.ShipCount) {
                BattleShipsObject shipFromDb = ships[index];
                Ship shipFromGame = flotilla.Ships[index++];
                shipFromDb.Name = shipFromGame.Name;
                shipFromDb.Size = shipFromGame.Size;
                shipFromDb.Health = shipFromGame.Health;
                shipFromDb.ShipCellsArray = JsonSerializer.Serialize(shipFromGame.ShipCellsArray);
            }
        }
    }
}