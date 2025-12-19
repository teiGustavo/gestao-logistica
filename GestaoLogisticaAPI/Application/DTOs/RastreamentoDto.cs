namespace GestaoLogisticaAPI.Application.DTOs;

public class RastreamentoDto
{
    public int CodRastreamento { get; set; }
    public int CodEntrega { get; set; }
    public DateTime? DataHora { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? LocalizacaoTexto { get; set; }
    public string? Observacao { get; set; }
    public DateTime? CriadoEm { get; set; }
}