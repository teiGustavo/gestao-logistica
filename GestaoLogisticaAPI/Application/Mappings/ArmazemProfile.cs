using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class ArmazemProfile : Profile
{
    public ArmazemProfile() { CreateMap<Armazem, ArmazemDto>().ReverseMap(); }
}