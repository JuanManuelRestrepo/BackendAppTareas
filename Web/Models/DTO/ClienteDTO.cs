using System.ComponentModel.DataAnnotations;

namespace ApiSampleFinal.Models.MilkModels
{
    public class ClienteDTO
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        public string CorreoElctronico { get; set; }

        public string Telefono { get; set; }

        public DateTime FechaRegistro { get; set; }


    }
}
