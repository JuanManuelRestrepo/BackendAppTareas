using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProyectoServices
    {

        Task<IList<Proyecto>> GetAllProyecto();
        Task<Proyecto> GetProyectoByID(Guid id);
        Task<(bool, string)> CreateProyecto(Proyecto proyecto, ProyectoUsuario proyectousuario);
        Task UpdateProyecto(Proyecto proyecto);
        Task DeleteProyecto(Guid id);
    }
}
