using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class EntregaProdutoProfile : Profile
{
    public EntregaProdutoProfile() { CreateMap<EntregaProduto, EntregaProdutoDto>().ReverseMap(); }
}