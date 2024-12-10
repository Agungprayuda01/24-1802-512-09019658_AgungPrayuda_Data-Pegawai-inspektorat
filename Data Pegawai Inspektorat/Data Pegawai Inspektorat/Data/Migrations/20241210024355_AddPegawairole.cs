using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Pegawai_Inspektorat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPegawairole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "Pegawais",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "Pegawais");
        }
    }
}
