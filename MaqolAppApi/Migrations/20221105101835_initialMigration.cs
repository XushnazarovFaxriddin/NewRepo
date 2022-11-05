using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaqolAppApi.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mualliflar",
                columns: table => new
                {
                    MuallifId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ism = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Familya = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RasmUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TugilganKuni = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mualliflar", x => x.MuallifId);
                });

            migrationBuilder.CreateTable(
                name: "Maqolalar",
                columns: table => new
                {
                    MaqolaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sarlavha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Batafsil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KurishlarSoni = table.Column<int>(type: "int", nullable: false),
                    YaratilganVaqti = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    RasmUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuallifId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maqolalar", x => x.MaqolaId);
                    table.ForeignKey(
                        name: "FK_Maqolalar_Mualliflar_MuallifId",
                        column: x => x.MuallifId,
                        principalTable: "Mualliflar",
                        principalColumn: "MuallifId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maqolalar_MuallifId",
                table: "Maqolalar",
                column: "MuallifId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maqolalar");

            migrationBuilder.DropTable(
                name: "Mualliflar");
        }
    }
}
