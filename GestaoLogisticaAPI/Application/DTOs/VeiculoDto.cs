namespace GestaoLogisticaAPI.Application.DTOs;

public class VeiculoDto
{
    public int CodVeiculo { get; set; }
    public string Placa { get; set; } = null!;
    public string? Modelo { get; set; }
    public decimal? CapacidadeKg { get; set; }
    public int? CodTransportadora { get; set; }
    public string? Status { get; set; }
    public DateTime? CriadoEm { get; set; }
}