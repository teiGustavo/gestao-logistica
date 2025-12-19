using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class RastreamentoProfile : Profile
{
    public RastreamentoProfile() { CreateMap<Rastreamento, RastreamentoDto>().ReverseMap(); }
}