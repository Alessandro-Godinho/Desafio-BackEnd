using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DesafioBackEnd.Dtos;
using DesafioBackEnd.Models;

namespace DesafioBackEnd.Helpers
{
    public class AutoMapperProfiles : Profile
    {
      
        public AutoMapperProfiles ()
        {       
             CreateMap<Medico, MedicoDto>().ForMember(dest => dest.Especialidades, opt => opt.MapFrom(so => so.Especialidades.Select(t=>t.Descricao).ToList()));                    
        }

        
    }
}