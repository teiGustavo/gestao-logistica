using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class EstoqueProfile : Profile
{
    public EstoqueProfile() { CreateMap<Estoque, EstoqueDto>().ReverseMap(); }
}