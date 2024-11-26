using Swashbuckle.AspNetCore.Annotations;
using System;

namespace ApiSampleFinal.Models.DTO
{

    using Swashbuckle.AspNetCore.Annotations;

    namespace ApiSampleFinal.Models.DTO
    {
 
        public class TareaDTO
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public Guid ProyectoId { get; set; }
            public List<Guid> ResponsablesIds { get; set; } = new List<Guid>();

            public EstadoTarea Estado { get; set; } = EstadoTarea.Abierta;

            public enum EstadoTarea
            {
                Abierta,
                Suspendida,
                Cerrada
            }
        }
    }

}
