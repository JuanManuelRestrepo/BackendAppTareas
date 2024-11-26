using ApiSampleFinal.Models.DTO;
using ApiSampleFinal.Models.MilkModels;
using AutoMapper;
using Domain;

namespace ApiSampleFinal.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            TareaMapper();
            UsuarioMapper();
            ProyectoMapper();
        }

        private void UsuarioMapper()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioIdDTO>(); // Solo mapear de Usuario a UsuarioIdDTO
        }

        private void TareaMapper()
        {
            CreateMap<Tarea, TareaDTO>().ReverseMap();

        }

        private void ProyectoMapper()
        {
            CreateMap<Proyecto, ProyectoDTO>().ReverseMap();
        }
    }
}