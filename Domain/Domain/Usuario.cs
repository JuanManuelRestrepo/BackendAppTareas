using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        // Relación con los roles del usuario
        public IList<Rol> Roles { get; set; } = new List<Rol>();

        // Relación con las tareas asignadas al usuario a través de la tabla intermedia TareaUsuario
        public IList<TareaUsuario> TareaUsuarios { get; set; } = new List<TareaUsuario>();

        // Relación con los proyectos en los que participa el usuario a través de la tabla intermedia ProyectoUsuario
        public IList<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}
