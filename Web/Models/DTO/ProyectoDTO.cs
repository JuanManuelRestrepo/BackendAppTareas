using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSampleFinal.Models.DTO
{
    public class ProyectoDTO
    {
        // El ID del proyecto, necesario para algunas operaciones (como actualizar o eliminar).
        public Guid? Id { get; set; }

        // El nombre del proyecto.
        public string Nombre { get; set; }
     
        public List<Guid> UsuariosId { get; set; } = new List<Guid>();
    }

 
}
