using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class MotoristaProfile : Profile
{
    public MotoristaProfile() { CreateMap<Motorista, MotoristaDto>().ReverseMap(); }
}