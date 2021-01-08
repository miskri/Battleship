using Domain.Objects;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<SaveObject> Saves { get; set; } = default!;
        public DbSet<BattleFlotillasObject> Flotillas { get; set; } = default!;
        public DbSet<BattleShipsObject> Ships { get; set; } = default!;
        public DbSet<BattlePropertiesObject> Properties { get; set; } = default!;
        public DbSet<PropertiesFlotillasObject> PropertiesFlotillas { get; set; } = default!;

        
        public AppDbContext() {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data source=C:\\Users\\Mihhail\\RiderProjects\\icd0008-2020f\\ConsoleApp\\Database\\base.db");
            
            //dotnet ef migrations add SavesDbCreation --project DAL --startup-project Game --context DAL.AppDbContext
            //dotnet ef database update --project DAL --startup-project Game --context DAL.AppDbContext
            //dotnet ef migrations remove --project DAL --startup-project Game --context DAL.AppDbContext
        }
        public AppDbContext(DbContextOptions options): base(options) {}
    }
}