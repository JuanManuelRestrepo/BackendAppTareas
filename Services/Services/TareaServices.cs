using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Application.Services;

namespace Services
{
    public class TareaServices : ITareasService
    {
        private readonly ITareasRepository _tareasRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProyectosRepository _proyectoRepository;

        public TareaServices(ITareasRepository tareaRepository, IUsuarioRepository usuarioRepository, IProyectosRepository proyectosRepository)
        {
            _tareasRepository = tareaRepository;
            _usuarioRepository = usuarioRepository;
            _proyectoRepository = proyectosRepository;
        }

        public async Task<IList<Tarea>> GetAllTareasAsync()
        {
            return await _tareasRepository.GetAllTarea();
        }

        public async Task<Tarea> GetTareaByIdAsync(Guid id)
        {
            return await _tareasRepository.GetTareaById(id);
        }

        public async Task<(bool, string)> CreateTareaAsync(Tarea tarea)
        {
            try
            {
                // Verificar si existe el proyecto al que se desea asociar la tarea
                var proyecto = await _proyectoRepository.GetProyectoByID(tarea.ProyectoId);
                if (proyecto == null)
                {
                    return (false, "El proyecto especificado no existe.");
                }

                // Crear la instancia de Tarea con los datos del DTO
                var nuevaTarea = new Tarea
                {
                    Id = Guid.NewGuid(),
                    Nombre = tarea.Nombre,
                    Descripcion = tarea.Descripcion,
                    FechaInicio = tarea.FechaInicio,
                    FechaFin = tarea.FechaFin,
                    ProyectoId = tarea.ProyectoId,
                    Proyecto = proyecto,
                    Estado = tarea.Estado
                };

                // Guardar la tarea en el repositorio
                await _tareasRepository.CreateTareaAsync(nuevaTarea);

                // Ahora que la tarea está creada, asignamos los usuarios responsables
                if (tarea.TareaUsuarios != null && tarea.TareaUsuarios.Any())
                {
                    // Obtener la lista de responsables de la tarea
                    var responsablesIds = tarea.TareaUsuarios.Select(tu => tu.UsuarioId).ToList();

                    // Llamar al repositorio para asignar responsables
                    await _tareasRepository.AsignarResponsablesATareaAsync(nuevaTarea.Id, responsablesIds);
                }

                return (true, "Tarea creada y responsables asignados exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear la tarea: {ex.Message}");
            }
        }
        public async Task<(bool success, string message)> AsignarResponsables(Guid tareaId, List<Guid> responsablesIds)
        {
            try
            {
                // Obtener la tarea por su ID
                var tarea = await _tareasRepository.GetTareaById(tareaId);
                if (tarea == null)
                {
                    return (false, "La tarea especificada no existe.");
                }

                // Obtener los usuarios responsables por sus IDs
                var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
                if (usuarios.Count != responsablesIds.Count)
                {
                    return (false, "Algunos de los usuarios no existen.");
                }

                // Crear las relaciones entre tarea y usuarios responsables
                foreach (var usuario in usuarios)
                {
                    var tareaUsuario = new TareaUsuario
                    {
                        TareaId = tareaId,
                        UsuarioId = usuario.Id
                    };

                    // Insertar la relación en la base de datos
                    await _tareasRepository.AddAsync(tareaUsuario);
                }

                return (true, "Responsables asignados exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al asignar responsables: {ex.Message}");
            }
        }


        public async Task UpdateTareaAsync(Guid id, Tarea tarea)
        {
            try
            {
                var tareaExistente = await _tareasRepository.GetTareaById(id);
                if (tareaExistente != null)
                {
                    // Actualizamos las propiedades directamente
                    tareaExistente.Nombre = tarea.Nombre ?? tareaExistente.Nombre;
                    tareaExistente.Descripcion = tarea.Descripcion ?? tareaExistente.Descripcion;
                    tareaExistente.FechaInicio = tarea.FechaInicio != default ? tarea.FechaInicio : tareaExistente.FechaInicio;
                    tareaExistente.FechaFin = tarea.FechaFin != default ? tarea.FechaFin : tareaExistente.FechaFin;
                    tareaExistente.Estado = tarea.Estado;

                    // Actualizamos TareaUsuarios si es necesario
                    if (tarea.TareaUsuarios != null)
                    {
                        tareaExistente.TareaUsuarios = tarea.TareaUsuarios;
                    }

                    // Guardamos los cambios
                    await _tareasRepository.UpdateTareaAsync(tareaExistente);
                }
                else
                {
                    throw new Exception("No existe la tarea");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<Tarea>> GetTareaProyectoById(Guid proyectoId)
        {

            return await _tareasRepository.GetTareaProyectoById(proyectoId);
        }

        public async Task DeleteTareaAsync(Guid id)
        {
            try
            {
                await _tareasRepository.DeleteTareaAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
