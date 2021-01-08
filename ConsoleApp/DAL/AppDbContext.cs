using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        //public DbSet<Settings> Settings { get; set; } = default!; //TODO
        //public DbSet<SaveObject> Saves { get; set; } = default!; //TODO
        
        public AppDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlite("Data source=C:/Users/Exeof/icd0008-2019f/Homework01/Connect4/Data.db"); //TODO
            
            //dotnet ef migrations add SavesDbCreation --project DAL --startup-project Connect4 --context DAL.AppDbContext
            //dotnet ef database update --project DAL --startup-project Connect4 --context DAL.AppDbContext
            //dotnet ef migrations remove --project DAL --startup-project Connect4 --context DAL.AppDbContext
        }
        public AppDbContext(DbContextOptions options): base(options)
        {
        }
    }
}