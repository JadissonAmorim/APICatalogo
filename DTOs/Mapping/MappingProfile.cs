using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){

            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();

        }
    }
}
