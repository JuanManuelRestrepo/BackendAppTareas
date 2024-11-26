using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ITareasRepository
    {
        Task<IList<Tarea>> GetAllTarea();
        Task<Tarea> GetTareaById(Guid id);
        Task CreateTareaAsync(Tarea tarea);
        Task UpdateTareaAsync(Tarea tarea);
        Task DeleteTareaAsync(Guid id);
        Task AsignarResponsablesATareaAsync(Guid tareaId, List<Guid> responsablesid);
        Task<IList<Tarea>> GetTareaProyectoById(Guid proyectoId);
        Task AddAsync(TareaUsuario tarea);
      
}
}
