using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProyectosRepository : BaseRepository, IProyectosRepository
    {
        private readonly DbContext _context;

        public ProyectosRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Obtener todos los proyectos
        public async Task<IList<Proyecto>> GetAllProyectos()
        {
            // Obtener todos los proyectos con tareas y usuarios asociados
            return await context.Proyectos
                .Include(p => p.Tareas)  // Incluir las tareas relacionadas
                .Include(p => p.Usuarios)  // Incluir los usuarios asociados a cada proyecto
                .ToListAsync();  // Obtener los resultados
        }


        // Obtener proyecto por ID
        public async Task<Proyecto> GetProyectoByID(Guid id)
        {
            return await context.Proyectos
                .Include(p => p.Tareas)               // Incluir tareas relacionadas
                .Include(p => p.Usuarios)     // Incluir usuarios asociados
               
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Crear un nuevo proyecto
        public async Task CreateProyecto(Proyecto proyecto)
        {
            await context.Proyectos.AddAsync(proyecto);
            await context.SaveChangesAsync();
        }

        // Actualizar un proyecto existente
        public async Task UpdateProyecto(Proyecto proyecto)
        {
            try
            {
                // Adjudicar el proyecto al contexto, marcarlo como modificado
                context.Proyectos.Entry(proyecto).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el proyecto: {ex.Message}");
            }
        }


        // Eliminar un proyecto
        public async Task DeleteProyecto(Guid id)
        {
            var proyecto = await GetProyectoByID(id);
            if (proyecto != null)
            {
                context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
