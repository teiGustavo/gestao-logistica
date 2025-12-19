using AutoMapper;
using GestaoLogisticaAPI.Domain.Entities;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Mappings;

public class VeiculoProfile : Profile
{
    public VeiculoProfile() { CreateMap<Veiculo, VeiculoDto>().ReverseMap(); }
}