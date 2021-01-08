using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    BattlePropertiesObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<string>(type: "TEXT", nullable: true),
                    GameMode = table.Column<string>(type: "TEXT", nullable: true),
                    Player1Name = table.Column<string>(type: "TEXT", nullable: true),
                    Player2Name = table.Column<string>(type: "TEXT", nullable: true),
                    FieldSize = table.Column<string>(type: "TEXT", nullable: true),
                    Player1FieldArray = table.Column<string>(type: "TEXT", nullable: true),
                    Player2FieldArray = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentPlayer = table.Column<string>(type: "TEXT", nullable: true),
                    Round = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectableRowCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleHistory = table.Column<string>(type: "TEXT", nullable: true),
                    MenuOptions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.BattlePropertiesObjectId);
                });

            migrationBuilder.CreateTable(
                name: "Saves",
                columns: table => new
                {
                    SaveObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SaveName = table.Column<string>(type: "TEXT", nullable: true),
                    BattlePropertiesObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saves", x => x.SaveObjectId);
                    table.ForeignKey(
                        name: "FK_Saves_Properties_BattlePropertiesObjectId",
                        column: x => x.BattlePropertiesObjectId,
                        principalTable: "Properties",
                        principalColumn: "BattlePropertiesObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesFlotillas",
                columns: table => new
                {
                    PropertiesFlotillasObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattleId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlotillaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesFlotillas", x => x.PropertiesFlotillasObjectId);
                    table.ForeignKey(
                        name: "FK_PropertiesFlotillas_Properties_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Properties",
                        principalColumn: "BattlePropertiesObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    BattleShipsObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlotillaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipCellsArray = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.BattleShipsObjectId);
                });

            migrationBuilder.CreateTable(
                name: "Flotillas",
                columns: table => new
                {
                    BattleFlotillasObjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    FlotillaHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    BattleShipsObjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    PropertiesFlotillasObjectId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flotillas", x => x.BattleFlotillasObjectId);
                    table.ForeignKey(
                        name: "FK_Flotillas_PropertiesFlotillas_PropertiesFlotillasObjectId",
                        column: x => x.PropertiesFlotillasObjectId,
                        principalTable: "PropertiesFlotillas",
                        principalColumn: "PropertiesFlotillasObjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flotillas_Ships_BattleShipsObjectId",
                        column: x => x.BattleShipsObjectId,
                        principalTable: "Ships",
                        principalColumn: "BattleShipsObjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flotillas_BattleShipsObjectId",
                table: "Flotillas",
                column: "BattleShipsObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Flotillas_PropertiesFlotillasObjectId",
                table: "Flotillas",
                column: "PropertiesFlotillasObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesFlotillas_BattleId",
                table: "PropertiesFlotillas",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesFlotillas_FlotillaId",
                table: "PropertiesFlotillas",
                column: "FlotillaId");

            migrationBuilder.CreateIndex(
                name: "IX_Saves_BattlePropertiesObjectId",
                table: "Saves",
                column: "BattlePropertiesObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_FlotillaId",
                table: "Ships",
                column: "FlotillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertiesFlotillas_Flotillas_FlotillaId",
                table: "PropertiesFlotillas",
                column: "FlotillaId",
                principalTable: "Flotillas",
                principalColumn: "BattleFlotillasObjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Flotillas_FlotillaId",
                table: "Ships",
                column: "FlotillaId",
                principalTable: "Flotillas",
                principalColumn: "BattleFlotillasObjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flotillas_PropertiesFlotillas_PropertiesFlotillasObjectId",
                table: "Flotillas");

            migrationBuilder.DropForeignKey(
                name: "FK_Flotillas_Ships_BattleShipsObjectId",
                table: "Flotillas");

            migrationBuilder.DropTable(
                name: "Saves");

            migrationBuilder.DropTable(
                name: "PropertiesFlotillas");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "Flotillas");
        }
    }
}
