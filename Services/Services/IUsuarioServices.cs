using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUsuarioServices
    {
        Task<IList<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task<IList<Usuario>> GetUsuarioByProyectoId(Guid id);
        void CreateUsuario(Usuario usuario);
        Task UpdateUsuario(Guid id, Usuario usuarioDto);
        void DeleteUsuario(Usuario usuario);
        void DeleteUsuarioById(Guid id);
        Task<Usuario> LoginUsuarioAsync(string email, string password);

    }
}
