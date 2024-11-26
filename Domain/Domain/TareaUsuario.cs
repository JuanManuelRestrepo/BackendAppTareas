using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   
        public class TareaUsuario
        {
            public Guid TareaId { get; set; }
            public Tarea Tarea { get; set; }

            public Guid UsuarioId { get; set; }
            public Usuario Usuario { get; set; }
        }

    }

