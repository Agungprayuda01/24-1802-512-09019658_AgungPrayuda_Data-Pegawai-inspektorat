using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Pegawai_Inspektorat.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialPegawai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pegawais",
                columns: table => new
                {
                    Nik = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alamat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ttl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pegawais", x => x.Nik);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pegawais");
        }
    }
}
