using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class EnderecoProfile : Profile
{
    public EnderecoProfile() { CreateMap<Endereco, EnderecoDto>().ReverseMap(); }
}