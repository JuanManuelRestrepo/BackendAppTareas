using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Tarea
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(500)]
        public string Descripcion { get; set; }

        // Relación muchos a muchos con los usuarios responsables de la tarea
       
        public List<TareaUsuario> TareaUsuarios { get; set; } = new List<TareaUsuario>();

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Fecha de creación por defecto al momento de instanciar la tarea
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación con el proyecto al que pertenece la tarea
        public Guid ProyectoId { get; set; } // Clave foránea
        [JsonIgnore]
        public Proyecto Proyecto { get; set; } // Propiedad de navegación

        // Estado de la tarea con valor por defecto
        public EstadoTarea Estado { get; set; } = EstadoTarea.Abierta;

        public enum EstadoTarea
        {
            Abierta,
            Suspendida,
            Cerrada
        }
    }
}
