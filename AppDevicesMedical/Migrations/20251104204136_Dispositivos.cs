using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppDevicesMedical.Migrations
{
    /// <inheritdoc />
    public partial class Dispositivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id_log = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_usuario = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id_log);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Categoria = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fecha_de_creación = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "ClaseRiesgo",
                columns: table => new
                {
                    Id_clase_riesgo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_clase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseRiesgo", x => x.Id_clase_riesgo);
                });

            migrationBuilder.CreateTable(
                name: "Cuarto",
                columns: table => new
                {
                    Id_cuarto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clase_cuarto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dimensiones_m2 = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Capacidad_personas = table.Column<int>(type: "int", nullable: false),
                    Control_temperatura = table.Column<bool>(type: "bit", nullable: false),
                    Control_humedad = table.Column<bool>(type: "bit", nullable: false),
                    Espec_flujo_aire = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Capacidad_hvac_cfm = table.Column<int>(type: "int", nullable: true),
                    Tipo_acondicionamiento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tiempo_reposicion_aire_min = table.Column<int>(type: "int", nullable: true),
                    Equipamiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocolo_acceso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocolo_validacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha_ultima_validacion = table.Column<DateTime>(type: "Date", nullable: true),
                    Periodo_revalidacion_meses = table.Column<int>(type: "int", nullable: true),
                    Estado_inspeccion_listo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notas_adicionales = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuarto", x => x.Id_cuarto);
                });

            migrationBuilder.CreateTable(
                name: "Especialidad",
                columns: table => new
                {
                    Id_Especialidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Especialidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidad", x => x.Id_Especialidad);
                });

            migrationBuilder.CreateTable(
                name: "Examenes",
                columns: table => new
                {
                    IdExamen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EnlaceFormulario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examenes", x => x.IdExamen);
                });

            migrationBuilder.CreateTable(
                name: "Normas",
                columns: table => new
                {
                    Id_norma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo_norma = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Normas", x => x.Id_norma);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id_rol = table.Column<int>(type: "int", nullable: false),
                    Nombre_rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id_status = table.Column<int>(type: "int", nullable: false),
                    Nombre_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id_status);
                });

            migrationBuilder.CreateTable(
                name: "TipoDispositivo",
                columns: table => new
                {
                    Id_tipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDispositivo", x => x.Id_tipo);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellido_paterno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    apellido_materno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numero_empleado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_status = table.Column<int>(type: "int", nullable: false),
                    id_especialidad = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Especialidad_id_especialidad",
                        column: x => x.id_especialidad,
                        principalTable: "Especialidad",
                        principalColumn: "Id_Especialidad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Rol",
                        principalColumn: "Id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Status_id_status",
                        column: x => x.id_status,
                        principalTable: "Status",
                        principalColumn: "Id_status",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispositivos",
                columns: table => new
                {
                    Id_dispositivo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion_detallada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_categoria = table.Column<int>(type: "int", nullable: true),
                    Id_clase_riesgo = table.Column<int>(type: "int", nullable: true),
                    Id_tipo_dispositivo = table.Column<int>(type: "int", nullable: true),
                    Id_cuarto_requerido = table.Column<int>(type: "int", nullable: true),
                    Es_invasivo = table.Column<bool>(type: "bit", nullable: false),
                    Requiere_biocompatibilidad = table.Column<bool>(type: "bit", nullable: false),
                    Requiere_prueba_residuales = table.Column<bool>(type: "bit", nullable: false),
                    Metodo_esterilizacion_req = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Estado_regulatorio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.Id_dispositivo);
                    table.ForeignKey(
                        name: "FK_Dispositivos_Categoria_Id_categoria",
                        column: x => x.Id_categoria,
                        principalTable: "Categoria",
                        principalColumn: "Id_Categoria");
                    table.ForeignKey(
                        name: "FK_Dispositivos_ClaseRiesgo_Id_clase_riesgo",
                        column: x => x.Id_clase_riesgo,
                        principalTable: "ClaseRiesgo",
                        principalColumn: "Id_clase_riesgo");
                    table.ForeignKey(
                        name: "FK_Dispositivos_Cuarto_Id_cuarto_requerido",
                        column: x => x.Id_cuarto_requerido,
                        principalTable: "Cuarto",
                        principalColumn: "Id_cuarto");
                    table.ForeignKey(
                        name: "FK_Dispositivos_TipoDispositivo_Id_tipo_dispositivo",
                        column: x => x.Id_tipo_dispositivo,
                        principalTable: "TipoDispositivo",
                        principalColumn: "Id_tipo");
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id_rol", "Descripcion", "Nombre_rol" },
                values: new object[,]
                {
                    { 1, "Administrador del sistema", "Admin" },
                    { 2, "Supervisor de área", "Supervisor" },
                    { 3, "Técnico de dispositivos", "Tecnico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_Id_categoria",
                table: "Dispositivos",
                column: "Id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_Id_clase_riesgo",
                table: "Dispositivos",
                column: "Id_clase_riesgo");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_Id_cuarto_requerido",
                table: "Dispositivos",
                column: "Id_cuarto_requerido");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_Id_tipo_dispositivo",
                table: "Dispositivos",
                column: "Id_tipo_dispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_especialidad",
                table: "Usuarios",
                column: "id_especialidad");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_rol",
                table: "Usuarios",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_status",
                table: "Usuarios",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_numero_empleado",
                table: "Usuarios",
                column: "numero_empleado",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Dispositivos");

            migrationBuilder.DropTable(
                name: "Examenes");

            migrationBuilder.DropTable(
                name: "Normas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "ClaseRiesgo");

            migrationBuilder.DropTable(
                name: "Cuarto");

            migrationBuilder.DropTable(
                name: "TipoDispositivo");

            migrationBuilder.DropTable(
                name: "Especialidad");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
