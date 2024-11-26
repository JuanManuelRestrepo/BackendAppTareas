using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Rol
    {

        public Guid Id { get; set; }   

        public string RolName { get; set; }

        public IList<Usuario> Usuarios { get; set; }
    }
}
