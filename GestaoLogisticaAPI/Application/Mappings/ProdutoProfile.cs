using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class ProdutoProfile : Profile
{
    public ProdutoProfile() { CreateMap<Produto, ProdutoDto>().ReverseMap(); }
}