using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class TransportadoraProfile : Profile
{
    public TransportadoraProfile() { CreateMap<Transportadora, TransportadoraDto>().ReverseMap(); }
}