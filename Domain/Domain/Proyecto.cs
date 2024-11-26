using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Proyecto
    {
        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        // Relación con las tareas del proyecto
        public List<Tarea> Tareas { get; set; } = new List<Tarea>();

        // Relación con los usuarios involucrados en el proyecto a través de la tabla intermedia ProyectoUsuario
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
