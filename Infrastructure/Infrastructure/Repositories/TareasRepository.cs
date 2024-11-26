using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TareasRepository : BaseRepository, ITareasRepository
    {
        public TareasRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IList<Tarea>> GetAllTarea()
        {
            return await context.Tareas
                .Include(t => t.Proyecto)
                .Include(t => t.TareaUsuarios)
                    .ThenInclude(tu => tu.Usuario)
                .ToListAsync();
        }



        public async Task<Tarea> GetTareaById(Guid id)
        {
            // Obtiene la tarea por su ID, incluyendo Proyecto y TareaUsuarios con Usuario.
            return await context.Tareas
                .Include(t => t.Proyecto) // Incluye el Proyecto relacionado
                .Include(t => t.TareaUsuarios) // Incluye la relación con TareaUsuarios
                .ThenInclude(tu => tu.Usuario) // Incluye la entidad Usuario relacionada con TareaUsuario
                .FirstOrDefaultAsync(t => t.Id == id); // Filtra por ID
        }
        public async Task CreateTareaAsync(Tarea tarea)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                tarea.Id = Guid.NewGuid();

                // Asegurarse de que TareaUsuarios no sea nulo antes de agregar un nuevo TareaUsuario
                tarea.TareaUsuarios ??= new List<TareaUsuario>();  // Inicializar si es nulo

                // Agregar los TareaUsuarios a la lista de relación
                foreach (var usuarioId in tarea.TareaUsuarios.Select(tu => tu.UsuarioId))
                {
                    // Añadimos el TareaUsuario con la relación entre Tarea y Usuario
                    tarea.TareaUsuarios.Add(new TareaUsuario { TareaId = tarea.Id, UsuarioId = usuarioId });
                }

                await context.Tareas.AddAsync(tarea);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateTareaAsync(Tarea tarea)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var tareaExistente = await GetTareaById(tarea.Id);
                if (tareaExistente != null)
                {
                    // Actualizamos las propiedades directamente
                    tareaExistente.Nombre = tarea.Nombre ?? tareaExistente.Nombre;
                    tareaExistente.Descripcion = tarea.Descripcion ?? tareaExistente.Descripcion;

                    // Actualizamos TareaUsuarios si es necesario
                    if (tarea.TareaUsuarios != null)
                    {
                        tareaExistente.TareaUsuarios = tarea.TareaUsuarios;
                    }

                    tareaExistente.FechaInicio = tarea.FechaInicio != default ? tarea.FechaInicio : tareaExistente.FechaInicio;
                    tareaExistente.FechaFin = tarea.FechaFin != default ? tarea.FechaFin : tareaExistente.FechaFin;
                    tareaExistente.Estado = tarea.Estado;

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("No existe la tarea");
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTareaAsync(Guid id)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var tarea = await GetTareaById(id);

                if (tarea != null)
                {
                    context.Tareas.Remove(tarea);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task AsignarResponsablesATareaAsync(Guid tareaId, List<Guid> usuarioIds)
        {
            var tarea = await context.Tareas
                .FirstOrDefaultAsync(t => t.Id == tareaId);

            if (tarea == null)
            {
                throw new Exception("La tarea no existe.");
            }

            var usuarios = await context.Usuarios
                .Where(u => usuarioIds.Contains(u.Id))
                .ToListAsync();

            if (usuarios.Count != usuarioIds.Count)
            {
                throw new Exception("Algunos usuarios no existen.");
            }

            var tareaUsuarios = usuarios.Select(usuario => new TareaUsuario
            {
                TareaId = tarea.Id,
                UsuarioId = usuario.Id
            }).ToList();

            context.TareaUsuarios.AddRange(tareaUsuarios);
            await context.SaveChangesAsync();
        }



        public async Task<IList<Usuario>> GetUsuariosByTareaID(Guid tareaId)
        {
            try
            {
                // Suponiendo que TareaUsuarios tiene una relación entre Tarea y Usuario
                var usuarios = await context.TareaUsuarios
                    .Where(tu => tu.TareaId == tareaId)
                    .Select(tu => tu.Usuario)  // Asumiendo que TareaUsuarios tiene una propiedad 'Usuario'
                    .ToListAsync();

                return usuarios;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los usuarios asociados con la tarea", ex);
            }
        }



        public async Task<IList<Tarea>> GetTareaProyectoById(Guid proyectoId)
        {
            // Filtra las tareas que pertenecen al proyecto con el id dado
            return await context.Tareas
                .Where(t => t.ProyectoId == proyectoId)
                .ToListAsync();
        }

        public async Task AddAsync(TareaUsuario tareaUsuario)
        {
            await context.TareaUsuarios.AddAsync(tareaUsuario);
            await context.SaveChangesAsync();
        }


    }

}