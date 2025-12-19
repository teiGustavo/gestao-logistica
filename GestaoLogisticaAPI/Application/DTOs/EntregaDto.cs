namespace GestaoLogisticaAPI.Application.DTOs;

public class EntregaDto
{
    public int CodEntrega { get; set; }
    public string? CodigoExterno { get; set; }
    public int? CodClienteRemetente { get; set; }
    public int? CodClienteDestinatario { get; set; }
    public int? CodTransportadora { get; set; }
    public int? CodVeiculo { get; set; }
    public int? CodMotorista { get; set; }
    public int? CodRota { get; set; }
    public DateTime? DataPedido { get; set; }
    public DateOnly? DataPrevisaoEntrega { get; set; }
    public decimal? PesoTotalKg { get; set; }
    public decimal? VolumeTotalM3 { get; set; }
    public decimal? ValorFrete { get; set; }
    public string? StatusEntrega { get; set; }
    public DateTime? CriadoEm { get; set; }
}