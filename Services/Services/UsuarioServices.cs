using Domain;
using Infrastructure.Repositories;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioServices: IUsuarioServices
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioServices(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IList<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllUsuariosAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(Guid id)
        {

          var Usuario= await _usuarioRepository.GetUsuarioByIdAsync(id);

            return Usuario;
        }


        public async Task<IList<Usuario>> GetUsuarioByProyectoId(Guid id)
        {
            return await _usuarioRepository.ObtenerUsuariosPorProyectoAsync(id);

        }

        public void CreateUsuario(Usuario usuario)
        {
            if (!ContraseñaServices.SeguridadContraseña(usuario.Contraseña))
            {
                throw new Exception("La contraseña no cumple con los requisitos de seguridad");
            }

            usuario.Contraseña = ContraseñaServices.HashearContraseñaConSalt(usuario.Contraseña);
            _usuarioRepository.CreateUsuario(usuario);
        }

        // Servicio
        public async Task UpdateUsuario(Guid id, Usuario usuarioDto)
        {
            if (usuarioDto == null)
            {
                throw new ArgumentNullException(nameof(usuarioDto), "El usuario no puede ser nulo");
            }

            // Obtener el usuario existente de manera asincrónica

           // Usuario usuarioExistente= await GetUsuarioByIdAsync(id);
        Usuario usuarioExistente = await _usuarioRepository.GetUsuarioByIdAsync(id);
           // if (usuarioExistente == null)
           // {
           //     throw new Exception("Usuario no encontrado");
           // }

            

            // Validar la contraseña si es proporcionada
            if (!string.IsNullOrEmpty(usuarioDto.Contraseña) && !ContraseñaServices.SeguridadContraseña(usuarioDto.Contraseña))
            {
                throw new Exception("La contraseña no cumple con los requisitos de seguridad.");
            }

            // Actualizar solo las propiedades que han cambiado
            usuarioExistente.Name = !string.IsNullOrEmpty(usuarioDto.Name) ? usuarioDto.Name : usuarioExistente.Name;
            usuarioExistente.Email = !string.IsNullOrEmpty(usuarioDto.Email) ? usuarioDto.Email : usuarioExistente.Email;

            // Si se proporciona una nueva contraseña, actualizarla
            if (!string.IsNullOrEmpty(usuarioDto.Contraseña))
            {
                usuarioDto.Contraseña = ContraseñaServices.HashearContraseñaConSalt(usuarioDto.Contraseña);
            }



            // Llamar al repositorio para actualizar el usuario
            _usuarioRepository.UpdateUsuarioAsync(usuarioDto);
        }

        public void DeleteUsuario(Usuario usuario)
        {
            _usuarioRepository.DeleteUsuario(usuario);
        }

        public void DeleteUsuarioById(Guid id)
        {
            _usuarioRepository.DeleteUsuarioById(id);
        }

        public async Task<Usuario> LoginUsuarioAsync(string email, string password)
        {

            var contraseñaEncriptada = ContraseñaServices.HashearContraseñaConSalt(password);
            Console.WriteLine(contraseñaEncriptada);
            return await _usuarioRepository.LoginUsuarioAsync(email, contraseñaEncriptada);
        }


        private bool VerifyPassword(string password, string passwordHash)
        {
            // Implementa la lógica de verificación de la contraseña
            return password == passwordHash; // Asegúrate de implementar la lógica de hashing aquí
        }

    }
}
