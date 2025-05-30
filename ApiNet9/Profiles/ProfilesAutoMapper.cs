using ApiNet9.Dto.usuario;
using ApiNet9.Models;
using AutoMapper;

namespace ApiNet9.Profiles
{
    public class ProfilesAutoMapper: Profile
    {
        public ProfilesAutoMapper()
        {
            CreateMap<UsuarioCriacaoDto, UsuarioModel>().ReverseMap();
            
        }
    }
}
