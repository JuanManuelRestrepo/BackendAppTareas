using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class ProyectoServices : IProyectoServices
    {
        private readonly IProyectosRepository _proyectoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ProyectoServices(IProyectosRepository proyectosRepository, IUsuarioRepository usuarioRepository)
        {
            _proyectoRepository = proyectosRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<IList<Proyecto>> GetAllProyecto()
        {

            return await _proyectoRepository.GetAllProyectos();

        }


        public async Task<Proyecto> GetProyectoByID(Guid id)
        {
            var Proyecto = await _proyectoRepository.GetProyectoByID(id);
            if (Proyecto != null)
            {
                return Proyecto;
            }

            else
            {
                throw new Exception("Proyecto no encontrado");
            }
        }



        public async Task<(bool, string)> CreateProyecto(Proyecto proyecto, ProyectoUsuario proyectoUsuario)
        {

            try
            {
                await _proyectoRepository.CreateProyecto(proyecto);
                foreach (var usuarioId in proyectoUsuario.usuariosId) {

                    var usuario =  await _usuarioRepository.GetUsuarioByIdAsync(usuarioId);
                    
                    proyecto.Usuarios.Add(usuario);
                    usuario.Proyectos.Add(proyecto);

                     _usuarioRepository.UpdateUsuarioAsync(usuario);
                
                }

                await _proyectoRepository.UpdateProyecto(proyecto);


                return (true, "Tarea creada exitosamente."); // Éxito
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear la tarea: {ex.Message}"); // Error con el mensaje
            }
        }
        public async Task UpdateProyecto(Proyecto proyecto)
        {
            try
            {
                var proyectoExistente = await _proyectoRepository.GetProyectoByID(proyecto.Id);

                if (proyectoExistente == null)
                {
                    throw new Exception("Proyecto no encontrado");
                }

                // Actualiza los campos del proyecto solo si han cambiado
                proyectoExistente.Nombre = proyecto.Nombre ?? proyectoExistente.Nombre;

                if (proyecto.Usuarios != null && proyecto.Usuarios.Count > 0)
                {
                    // Limpiar usuarios anteriores y agregar los nuevos
                    proyectoExistente.Usuarios.Clear();
                    foreach (var proyectoUsuario in proyecto.Usuarios)
                    {
                        proyectoExistente.Usuarios.Add(proyectoUsuario);
                    }
                }

                // Actualiza el proyecto con la nueva información
                await _proyectoRepository.UpdateProyecto(proyectoExistente);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el proyecto: {ex.Message}");
            }
        }




        public async Task DeleteProyecto(Guid id)
        {
            try
            {
                 await _proyectoRepository.DeleteProyecto(id);
            }

            catch (Exception ex) { throw new Exception("No fue posible eliminar el proyecto"+ ex.Message); }
        }



    }
}

