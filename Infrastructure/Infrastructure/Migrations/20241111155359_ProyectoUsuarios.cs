using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProyectoUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolUsuario_Rol_RolesId",
                table: "RolUsuario");

            migrationBuilder.DropTable(
                name: "ProyectoUsuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rol",
                table: "Rol");

            migrationBuilder.RenameTable(
                name: "Rol",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProyectoUsuario",
                columns: table => new
                {
                    ProyectoUsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProyectoUsuariosId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoUsuario", x => new { x.ProyectoUsuariosId, x.ProyectoUsuariosId1 });
                    table.ForeignKey(
                        name: "FK_ProyectoUsuario_Proyectos_ProyectoUsuariosId",
                        column: x => x.ProyectoUsuariosId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoUsuario_Usuarios_ProyectoUsuariosId1",
                        column: x => x.ProyectoUsuariosId1,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoUsuario_ProyectoUsuariosId1",
                table: "ProyectoUsuario",
                column: "ProyectoUsuariosId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RolUsuario_Roles_RolesId",
                table: "RolUsuario",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolUsuario_Roles_RolesId",
                table: "RolUsuario");

            migrationBuilder.DropTable(
                name: "ProyectoUsuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Rol");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rol",
                table: "Rol",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProyectoUsuarios",
                columns: table => new
                {
                    ProyectoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoUsuarios", x => new { x.ProyectoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ProyectoUsuarios_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoUsuarios_UsuarioId",
                table: "ProyectoUsuarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolUsuario_Rol_RolesId",
                table: "RolUsuario",
                column: "RolesId",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
