using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppDevicesMedical.Migrations
{
    /// <inheritdoc />
    public partial class CUARTOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Cuarto_Id_cuarto_requerido",
                table: "Dispositivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cuarto",
                table: "Cuarto");

            migrationBuilder.DropColumn(
                name: "Equipamiento",
                table: "Cuarto");

            migrationBuilder.RenameTable(
                name: "Cuarto",
                newName: "Cuarto_Limpio");

            migrationBuilder.RenameColumn(
                name: "Estado_inspeccion_listo",
                table: "Cuarto_Limpio",
                newName: "Nivel_limpieza_iso");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha_ultima_validacion",
                table: "Cuarto_Limpio",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Clase_cuarto",
                table: "Cuarto_Limpio",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Capacidad_personas",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Capacidad_produccion",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Contaminacion_actual_ufc",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Conteo_particulas_no_viables",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Conteo_particulas_viables",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dependencia_proceso",
                table: "Cuarto_Limpio",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento_estandar_ref",
                table: "Cuarto_Limpio",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado_actual",
                table: "Cuarto_Limpio",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Etapa_proceso",
                table: "Cuarto_Limpio",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Filtracion_hepa",
                table: "Cuarto_Limpio",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Iluminacion_lux",
                table: "Cuarto_Limpio",
                type: "decimal(7,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Limite_contaminacion_ufc",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Limite_particulas_no_viables",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Limite_particulas_viables",
                table: "Cuarto_Limpio",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Nivel_ruido_db",
                table: "Cuarto_Limpio",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre_cuarto",
                table: "Cuarto_Limpio",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Presion_diferencial_pa",
                table: "Cuarto_Limpio",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuarto_Limpio",
                table: "Cuarto_Limpio",
                column: "Id_cuarto");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Cuarto_Limpio_Id_cuarto_requerido",
                table: "Dispositivos",
                column: "Id_cuarto_requerido",
                principalTable: "Cuarto_Limpio",
                principalColumn: "Id_cuarto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Cuarto_Limpio_Id_cuarto_requerido",
                table: "Dispositivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cuarto_Limpio",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Capacidad_produccion",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Contaminacion_actual_ufc",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Conteo_particulas_no_viables",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Conteo_particulas_viables",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Dependencia_proceso",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Documento_estandar_ref",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Estado_actual",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Etapa_proceso",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Filtracion_hepa",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Iluminacion_lux",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Limite_contaminacion_ufc",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Limite_particulas_no_viables",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Limite_particulas_viables",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Nivel_ruido_db",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Nombre_cuarto",
                table: "Cuarto_Limpio");

            migrationBuilder.DropColumn(
                name: "Presion_diferencial_pa",
                table: "Cuarto_Limpio");

            migrationBuilder.RenameTable(
                name: "Cuarto_Limpio",
                newName: "Cuarto");

            migrationBuilder.RenameColumn(
                name: "Nivel_limpieza_iso",
                table: "Cuarto",
                newName: "Estado_inspeccion_listo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha_ultima_validacion",
                table: "Cuarto",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Clase_cuarto",
                table: "Cuarto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Capacidad_personas",
                table: "Cuarto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Equipamiento",
                table: "Cuarto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuarto",
                table: "Cuarto",
                column: "Id_cuarto");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Cuarto_Id_cuarto_requerido",
                table: "Dispositivos",
                column: "Id_cuarto_requerido",
                principalTable: "Cuarto",
                principalColumn: "Id_cuarto");
        }
    }
}
