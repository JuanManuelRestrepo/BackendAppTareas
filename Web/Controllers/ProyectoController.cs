using ApiSampleFinal.Models.DTO;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Threading.Tasks;

namespace ApiSampleFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoServices _proyectoServices;
        private readonly IMapper _mapper;

        // Inyección de dependencias
        public ProyectoController(IProyectoServices proyectoServices, IMapper mapper)
        {
            _proyectoServices = proyectoServices;
            _mapper = mapper;
        }

        // Obtener todos los proyectos
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProyectos()
        {
            try
            {
                // Obtener los proyectos utilizando el servicio
                var proyectos = await _proyectoServices.GetAllProyecto();

                // Mapear los proyectos a una lista de DTOs
                var proyectosDTO = _mapper.Map<IEnumerable<ProyectoDTO>>(proyectos);

                // Devolver los proyectos como respuesta
                return Ok(proyectosDTO);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al obtener los proyectos: {ex.Message}");
            }
        }

        // Obtener un proyecto por ID
        [HttpGet("ProyectoById/{id}")]
        public async Task<IActionResult> GetProyectoByID(Guid id)
        {
            try
            {
                // Obtener el proyecto utilizando el servicio
                var proyecto = await _proyectoServices.GetProyectoByID(id);

                if (proyecto != null)
                {
                    // Mapear el proyecto a un DTO y devolverlo
                    var proyectoDTO = _mapper.Map<ProyectoDTO>(proyecto);
                    return Ok(proyectoDTO);
                }

                return NotFound("Proyecto no encontrado.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al obtener el proyecto: {ex.Message}");
            }
        }

        // Crear un proyecto
        [HttpPost("CrearProyecto")]
        public async Task<IActionResult> CrearProyecto([FromBody] ProyectoDTO proyectoDTO)
        {
            if (proyectoDTO == null || string.IsNullOrWhiteSpace(proyectoDTO.Nombre) || proyectoDTO.UsuariosId == null || !proyectoDTO.UsuariosId.Any())
            {
                return BadRequest("El proyecto debe tener un nombre y al menos un usuario asociado.");
            }

            try
            {
                // Mapear el DTO al modelo de dominio
                var proyecto = _mapper.Map<Proyecto>(proyectoDTO);
                var proyectoUsuario = new ProyectoUsuario { usuariosId = proyectoDTO.UsuariosId };

                // Llamar al servicio para crear el proyecto
                var (success, message) = await _proyectoServices.CreateProyecto(proyecto, proyectoUsuario);

                if (success)
                {
                    return Ok(new { Message = "Proyecto creado exitosamente." });
                }
                else
                {
                    return BadRequest(new { Message = message });
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return StatusCode(500, new { Message = $"Error al crear el proyecto: {ex.Message}" });
            }
        }


        // Actualizar un proyecto
        [HttpPut("ActualizarProyecto/{id}")]
        public async Task<IActionResult> ActualizarProyecto(Guid id, ProyectoDTO proyectoDTO)
        {
            try
            {
                // Mapear el DTO al modelo de dominio automáticamente
                var proyecto = _mapper.Map<Proyecto>(proyectoDTO);
                proyecto.Id = id;

                // Llamar al servicio para actualizar el proyecto
                await _proyectoServices.UpdateProyecto(proyecto);

                return Ok("El proyecto fue actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al actualizar el proyecto: {ex.Message}");
            }
        }

        // Eliminar un proyecto
        [HttpDelete("Borrar/{id}")]
        public async Task<IActionResult> BorraProyecto(Guid id)
        {
            try
            {
                // Llamar al servicio para eliminar el proyecto
                await _proyectoServices.DeleteProyecto(id);

                return Ok("El proyecto fue eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al eliminar el proyecto: {ex.Message}");
            }
        }

    }
}
