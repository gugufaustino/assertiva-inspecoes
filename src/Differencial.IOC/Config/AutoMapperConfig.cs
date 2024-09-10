using AutoMapper;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities; 

namespace Differencial.Infra
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapper()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Operador, UsuarioLogadoDTO>();
            });


        }

        public static void ResetAutoMapper()
        { 
            Mapper.Reset(); 
        }
    }
}