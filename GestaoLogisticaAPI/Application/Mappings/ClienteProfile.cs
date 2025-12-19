using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class ClienteProfile : Profile
{
    public ClienteProfile() { CreateMap<Cliente, ClienteDto>().ReverseMap(); }
}