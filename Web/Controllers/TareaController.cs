using ApiSampleFinal.Models.DTO;
using ApiSampleFinal.Models.DTO.ApiSampleFinal.Models.DTO;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiSampleFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly ITareasService _tareaServices;
        private readonly IMapper _mapper;

        // Inyección de dependencias
        public TareaController(ITareasService tareaServices, IMapper mapper)
        {
            _tareaServices = tareaServices;
            _mapper = mapper;
        }

        // Obtener todas las tareas
        [HttpGet("GetAllTareas")]   
        public async Task<IActionResult> GetAllTareas()
        {
            try
            {
                // Obtener los proyectos utilizando el servicio
                var tareas = await _tareaServices.GetAllTareasAsync();

                // Devolver los proyectos como respuesta
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al obtener los proyectos: {ex.Message}");
            }
        }

        // Obtener tarea por ID
        [HttpGet("GetTareaById")]
        public async Task<IActionResult> GetTareaByID(Guid id)
        {
            try
            {
                // Obtener la tarea utilizando el servicio
                var tarea = await _tareaServices.GetTareaByIdAsync(id);

                if (tarea != null)
                {
                    // Mapear la tarea a un DTO y devolverlo
                    return Ok(_mapper.Map<TareaDTO>(tarea));
                }

                return NotFound("Tarea no encontrada.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al obtener la tarea: {ex.Message}");
            }
        }

        // Crear una nueva tarea
        [HttpPost("Crear")]
        public async Task<IActionResult> CreateTarea(TareaDTO tareaDTO)
        {
            try
            {

                // Llamar al servicio para crear la tarea
                await _tareaServices.CreateTareaAsync(_mapper.Map<Tarea>(tareaDTO));


                return Ok("La tarea fue creada exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al crear la tarea: {ex.Message}");
            }
        }
        [HttpPost("asignarResponsables")]
        public async Task<IActionResult> AsignarResponsables([FromBody] AsignarResponsablesRequest request)
        {
            if (request == null || request.UsuarioIds == null || request.UsuarioIds.Count == 0)
            {
                return BadRequest("Debe proporcionar al menos un usuario responsable.");
            }

            try
            {
                var resultado = await _tareaServices.AsignarResponsables(request.TareaId, request.UsuarioIds);

                if (!resultado.Item1)
                {
                    return BadRequest(resultado.Item2); // Mensaje de error de la asignación
                }

                return Ok(resultado.Item2); // Mensaje de éxito
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }



        [HttpGet("TareaProyecto")]
        public async Task<IActionResult> ObtenerTareasPorProyecto(Guid proyectoId)
        {
  
            var tareas = await _tareaServices.GetTareaProyectoById(proyectoId);

            if (tareas == null || tareas.Count == 0)
            {
                return NotFound("No se encontraron tareas para este proyecto.");
            }

            var tareasDto = _mapper.Map<List<TareaDTO>>(tareas);

            return Ok(tareasDto);
        }


        [HttpPut("actualizar")]
        public async Task<IActionResult> UpdateTarea(Guid id, TareaDTO tareaDTO)
        {
            try
            {
                // Mapea el DTO a una entidad Tarea
                var tarea = _mapper.Map<Tarea>(tareaDTO);

                // Llama al servicio para actualizar la tarea
                await _tareaServices.UpdateTareaAsync(id, tarea);

                return Ok("La tarea fue actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al actualizar la tarea: {ex.Message}");
            }
        }


        // Eliminar una tarea
        [HttpDelete("BorrarTarea/{id}")]
        public async Task<IActionResult> DeleteTarea(Guid id)
        {
            try
            {
                // Llamar al servicio para eliminar la tarea
                await _tareaServices.DeleteTareaAsync(id);

                return Ok("La tarea fue eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones y retorno de mensaje de error
                return BadRequest($"Error al eliminar la tarea: {ex.Message}");
            }
        }

       
    }
}
