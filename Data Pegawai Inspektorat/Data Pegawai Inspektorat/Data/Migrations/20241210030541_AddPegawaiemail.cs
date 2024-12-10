using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Pegawai_Inspektorat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPegawaiemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Pegawais",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Pegawais");
        }
    }
}
