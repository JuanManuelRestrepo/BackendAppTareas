using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {

        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<IList<Usuario>> GetAllUsuariosAsync()
        {
            return await context.Usuarios
                .Include(u => u.TareaUsuarios) // Incluye las tareas en las que el usuario es responsable
                    .ThenInclude(tu => tu.Tarea) // Incluye la entidad Tarea para cada TareaUsuario
                        .ThenInclude(t => t.Proyecto) // Incluye el proyecto de cada tarea
                .Include(u => u.Proyectos) // Incluye los proyectos en los que el usuario es miembro
                .ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(Guid id)
        {

            Usuario user = 
             await context.Usuarios
                .Include(u => u.TareaUsuarios) // Carga la relación con TareaUsuarios
                .Include(u => u.Proyectos) // Carga la relación con ProyectoUsuarios
                .FirstOrDefaultAsync(u => u.Id == id); 
            return user;
            
            // Filtra por el ID del usuario
        }



        // Método asincrónico para obtener varios usuarios por sus Ids
        public async Task <Usuario> GetUsuarioById(Guid id)
        {
            Usuario user = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            return user; //await context.Usuarios.FirstOrDefaultAsync(x=>x.Id == id);
        }


        public async Task<IList<Usuario>> ObtenerUsuariosPorProyectoAsync(Guid proyectoId)
        {
            var proyecto = await context.Proyectos
                .Include(p => p.Usuarios) // Incluir la relación Usuarios
                .FirstOrDefaultAsync(p => p.Id == proyectoId);

            if (proyecto == null)
            {
                throw new Exception("Proyecto no encontrado.");
            }

            return proyecto.Usuarios.ToList(); // Devolver la lista de usuarios relacionados
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task CreateUsuario(Usuario usuario)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                await context.Usuarios.AddAsync(usuario);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {

           await Task.Run(() => context.Usuarios.Update(usuario));
            //context.Entry(usuario).State = EntityState.Modified;
            //await context.SaveChangesAsync();
        }

        public void DeleteUsuario(Usuario usuario)
        {
            var usuarioExistente = context.Usuarios.Find(usuario.Id);
            if (usuarioExistente != null)
            {
                context.Usuarios.Remove(usuarioExistente);
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Usuario no encontrado.");
            }
        }

        public void DeleteUsuarioById(Guid id)
        {
            var usuarioExistente = context.Usuarios.Find(id);
            if (usuarioExistente != null)
            {
                context.Usuarios.Remove(usuarioExistente);
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Usuario no encontrado.");
            }
        }

        public async Task<Usuario> LoginUsuarioAsync(string email, string password)
        {
            // Busca el usuario en la base de datos por el email proporcionado
            var usuario = await context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
    
            // Verifica si el usuario existe y si la contraseña es correcta
            if (usuario == null || !VerifyPassword(password, usuario.Contraseña))
            {
                return null; // Devuelve null si no se encuentra el usuario o la contraseña es incorrecta
            }

            // Devuelve el usuario encontrado si las credenciales son correctas
            return usuario;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
        return password == passwordHash; // Asegúrate de implementar la lógica de hashing aquí
        }


    }
}

