using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateEstanteModelEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "livroNaEstante",
                table: "Estante",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "livroNaEstante",
                table: "Estante");
        }
    }
}
