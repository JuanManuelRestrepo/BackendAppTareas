using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Repositories
{
    public  interface IUsuarioRepository
    {
        Task<IList<Usuario>> GetAllUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task<IList<Usuario>> ObtenerUsuariosPorProyectoAsync(Guid id);
        Task CreateUsuario(Usuario usuario);
        Task UpdateUsuarioAsync(Usuario usuario);
        Task<bool> EmailExistsAsync(string email);
        void DeleteUsuario(Usuario usuario);
        void DeleteUsuarioById(Guid id);
        Task <Usuario> GetUsuarioById(Guid id);
        Task<Usuario> LoginUsuarioAsync(string email, string password);
    }
}
