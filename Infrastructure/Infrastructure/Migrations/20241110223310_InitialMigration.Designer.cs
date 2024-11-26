﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241110223310_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Proyecto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("Domain.ProyectoUsuario", b =>
                {
                    b.Property<Guid>("ProyectoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProyectoId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ProyectoUsuarios");
                });

            modelBuilder.Entity("Domain.Rol", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rol");
                });

            modelBuilder.Entity("Domain.Tarea", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ProyectoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProyectoId");

                    b.ToTable("Tareas");
                });

            modelBuilder.Entity("Domain.TareaUsuario", b =>
                {
                    b.Property<Guid>("TareaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TareaId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TareaUsuarios");
                });

            modelBuilder.Entity("Domain.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("RolUsuario", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuariosId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesId", "UsuariosId");

                    b.HasIndex("UsuariosId");

                    b.ToTable("RolUsuario");
                });

            modelBuilder.Entity("Domain.ProyectoUsuario", b =>
                {
                    b.HasOne("Domain.Proyecto", "Proyecto")
                        .WithMany("ProyectoUsuarios")
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Usuario", "Usuario")
                        .WithMany("ProyectoUsuarios")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proyecto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Domain.Tarea", b =>
                {
                    b.HasOne("Domain.Proyecto", "Proyecto")
                        .WithMany("Tareas")
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proyecto");
                });

            modelBuilder.Entity("Domain.TareaUsuario", b =>
                {
                    b.HasOne("Domain.Tarea", "Tarea")
                        .WithMany("TareaUsuarios")
                        .HasForeignKey("TareaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Usuario", "Usuario")
                        .WithMany("TareaUsuarios")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tarea");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RolUsuario", b =>
                {
                    b.HasOne("Domain.Rol", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Proyecto", b =>
                {
                    b.Navigation("ProyectoUsuarios");

                    b.Navigation("Tareas");
                });

            modelBuilder.Entity("Domain.Tarea", b =>
                {
                    b.Navigation("TareaUsuarios");
                });

            modelBuilder.Entity("Domain.Usuario", b =>
                {
                    b.Navigation("ProyectoUsuarios");

                    b.Navigation("TareaUsuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
