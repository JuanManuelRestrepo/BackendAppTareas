using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITareasService
    {
        Task<IList<Tarea>> GetAllTareasAsync();
        Task<Tarea> GetTareaByIdAsync(Guid id);
        Task<(bool, string)> CreateTareaAsync(Tarea tarea);
        Task UpdateTareaAsync(Guid id,Tarea tarea);
        Task DeleteTareaAsync(Guid id);

        Task<IList<Tarea>> GetTareaProyectoById(Guid proyectoId);
        Task<(bool success, string message)> AsignarResponsables(Guid tareaId, List<Guid> usuarioIds);

    }
}
