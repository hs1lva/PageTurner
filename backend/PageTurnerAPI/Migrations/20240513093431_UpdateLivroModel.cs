using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PageTurnerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLivroModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idiomaOriginalLivro",
                table: "Livro",
                newName: "keyOL");

            migrationBuilder.AddColumn<string>(
                name: "capaLarge",
                table: "Livro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "capaMedium",
                table: "Livro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "capaSmall",
                table: "Livro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "capaLarge",
                table: "Livro");

            migrationBuilder.DropColumn(
                name: "capaMedium",
                table: "Livro");

            migrationBuilder.DropColumn(
                name: "capaSmall",
                table: "Livro");

            migrationBuilder.RenameColumn(
                name: "keyOL",
                table: "Livro",
                newName: "idiomaOriginalLivro");
        }
    }
}
