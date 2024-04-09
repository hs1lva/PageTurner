using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCidadeFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cidade_Pais_paisCidadepaisId",
                table: "Cidade");

            migrationBuilder.DropIndex(
                name: "IX_Cidade_paisCidadepaisId",
                table: "Cidade");

            migrationBuilder.RenameColumn(
                name: "paisCidadepaisId",
                table: "Cidade",
                newName: "paisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "paisId",
                table: "Cidade",
                newName: "paisCidadepaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Cidade_paisCidadepaisId",
                table: "Cidade",
                column: "paisCidadepaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cidade_Pais_paisCidadepaisId",
                table: "Cidade",
                column: "paisCidadepaisId",
                principalTable: "Pais",
                principalColumn: "paisId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
