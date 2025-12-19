using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class RotaProfile : Profile
{
    public RotaProfile() { CreateMap<Rota, RotaDto>().ReverseMap(); }
}