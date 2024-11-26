using ApiSampleFinal.Models.DTO;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;

namespace ApiSampleFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioService;
        private readonly IMapper _mapper;   

        public UsuarioController(IUsuarioServices usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper =mapper ;
        }

        [HttpGet("GetAllUsuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();

            // Mapea la lista de usuarios a una lista de UsuarioDTO
            var usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);

            return Ok(usuariosDTO); // Devuelve la lista de UsuarioDTO
        }



        [HttpGet ("GetUsuariosProyectos")]

        public async Task<IActionResult> GetUsuariosProyectos()
        {
            IList<Usuario> usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(_mapper.Map<IList<UsuarioIdDTO>>(usuarios));

        }

        // GET: api/usuario/GetUsuarioById/{id}
        [HttpGet("GetUsuarioById/{id}")]
        public async Task<IActionResult> GetUsuarioById(Guid id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            return Ok(_mapper.Map<UsuarioDTO>(usuario));
        }


        [HttpPost("crear")]
        public IActionResult CreateUsuario([FromBody] UsuarioDTO usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest("El cuerpo de la solicitud está vacío.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Aquí se genera un nuevo GUID si no se pasa uno
                var usuarioEntity = _mapper.Map<Usuario>(usuario);
                usuarioEntity.Id = usuarioEntity.Id == Guid.Empty ? Guid.NewGuid() : usuarioEntity.Id;

                _usuarioService.CreateUsuario(usuarioEntity);

                return Ok(new { message = "Usuario creado" });
            }
            catch (Exception ex)
            {
                // Manejar cualquier error inesperado
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }




        [HttpGet("Proyecto/{proyectoId}/Usuarios")]
        public async Task<IActionResult> ObtenerUsuariosPorProyecto(Guid proyectoId)
        {
            var usuarios = await _usuarioService.GetUsuarioByProyectoId(proyectoId);

            var usuariosDTO = _mapper.Map<List<UsuarioIdDTO>>(usuarios);

            return Ok(usuariosDTO);
        }



        [HttpPut("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario(Guid id,[FromBody] UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return BadRequest("Datos del usuario no válidos.");
            }

            try
            {
                // Mapear el UsuarioDTO a la entidad Usuario
                var usuario = _mapper.Map<Usuario>(usuarioDTO);

                // Llamar al servicio para actualizar el usuario
                var usuarioActualizado =  _usuarioService.UpdateUsuario(id, usuario);

                if (usuarioActualizado == null)
                {
                    return NotFound("El usuario no existe.");
                }

                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el usuario: {ex.Message}");
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _usuarioService.GetUsuarioByIdAsync(id);
            if (user != null)
            {
                _usuarioService.DeleteUsuario(user);
                return Ok(new { message = "Usuario eliminado con éxito" });
            }
            return NotFound(new { message = "Usuario no encontrado" });
        }

        // POST: api/usuario/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var usuario = await _usuarioService.LoginUsuarioAsync(loginRequest.Email, loginRequest.Password);
            if (usuario == null) return Unauthorized();

            // Respuesta exitosa con formato JSON
            return Ok(new { success = true, message = "Inicio Exitoso" });
        }

    }
}
