using Application.Requests.Produto;
using Application.Responses.Produto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.Profiles
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile() 
        {
            CreateMap<CreateProdutoRequest, Produto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

            CreateMap<UpdateProdutoRequest, Produto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Produto, ProdutoResponse>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria));

            CreateMap<Produto, ProdutoByIdResponse>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria));
        }
    }
}
