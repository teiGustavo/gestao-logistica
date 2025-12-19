using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class EntregaProfile : Profile
{
    public EntregaProfile() { CreateMap<Entrega, EntregaDto>().ReverseMap(); }
}