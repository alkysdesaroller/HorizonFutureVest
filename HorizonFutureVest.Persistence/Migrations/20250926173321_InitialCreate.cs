using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorizonFutureVest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionTasas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TasaMinima = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TasaMaxima = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionTasas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MacroIndicador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    EsMejorMasAlto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroIndicador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoIso = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MacroIndicadorSimulacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacroindicadorId = table.Column<int>(type: "int", nullable: false),
                    PesoSimulacion = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroIndicadorSimulacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroIndicadorSimulacion_MacroIndicador_MacroindicadorId",
                        column: x => x.MacroindicadorId,
                        principalTable: "MacroIndicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicadorPorPais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaisId = table.Column<int>(type: "int", nullable: false),
                    MacroindicadorId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadorPorPais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicadorPorPais_MacroIndicador_MacroindicadorId",
                        column: x => x.MacroindicadorId,
                        principalTable: "MacroIndicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicadorPorPais_Pais_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorPorPais_MacroindicadorId",
                table: "IndicadorPorPais",
                column: "MacroindicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadorPorPais_PaisId_MacroindicadorId_Year",
                table: "IndicadorPorPais",
                columns: new[] { "PaisId", "MacroindicadorId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MacroIndicadorSimulacion_MacroindicadorId",
                table: "MacroIndicadorSimulacion",
                column: "MacroindicadorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pais_CodigoIso",
                table: "Pais",
                column: "CodigoIso",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionTasas");

            migrationBuilder.DropTable(
                name: "IndicadorPorPais");

            migrationBuilder.DropTable(
                name: "MacroIndicadorSimulacion");

            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropTable(
                name: "MacroIndicador");
        }
    }
}
