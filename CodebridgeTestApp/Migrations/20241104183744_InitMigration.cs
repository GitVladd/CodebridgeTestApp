using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodebridgeTestApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tail_length = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.name);
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "name", "color", "tail_length", "weight" },
                values: new object[,]
                {
                    { "Jessy", "black & white", 7, 14 },
                    { "Neo", "red & amber", 22, 32 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dogs");
        }
    }
}
