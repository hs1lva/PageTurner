using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class valordataRegistoNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Cidade_cidadeId",
                table: "Utilizador");

            migrationBuilder.DropIndex(
                name: "IX_Utilizador_cidadeId",
                table: "Utilizador");

            migrationBuilder.DropColumn(
                name: "cidadeId",
                table: "Utilizador");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dataRegisto",
                table: "Utilizador",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "dataRegisto",
                table: "Utilizador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cidadeId",
                table: "Utilizador",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_cidadeId",
                table: "Utilizador",
                column: "cidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Cidade_cidadeId",
                table: "Utilizador",
                column: "cidadeId",
                principalTable: "Cidade",
                principalColumn: "cidadeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
