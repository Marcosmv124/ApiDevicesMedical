using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppDevicesMedical.Migrations
{
    /// <inheritdoc />
    public partial class Tipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivodv_TipoDispositivo_Id_tipo_dispositivo",
                table: "Dispositivodv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoDispositivo",
                table: "TipoDispositivo");

            migrationBuilder.RenameTable(
                name: "TipoDispositivo",
                newName: "TipoDispositivos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoDispositivos",
                table: "TipoDispositivos",
                column: "Id_tipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivodv_TipoDispositivos_Id_tipo_dispositivo",
                table: "Dispositivodv",
                column: "Id_tipo_dispositivo",
                principalTable: "TipoDispositivos",
                principalColumn: "Id_tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivodv_TipoDispositivos_Id_tipo_dispositivo",
                table: "Dispositivodv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoDispositivos",
                table: "TipoDispositivos");

            migrationBuilder.RenameTable(
                name: "TipoDispositivos",
                newName: "TipoDispositivo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoDispositivo",
                table: "TipoDispositivo",
                column: "Id_tipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivodv_TipoDispositivo_Id_tipo_dispositivo",
                table: "Dispositivodv",
                column: "Id_tipo_dispositivo",
                principalTable: "TipoDispositivo",
                principalColumn: "Id_tipo");
        }
    }
}
