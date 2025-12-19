namespace GestaoLogisticaAPI.Application.DTOs;

public class TransportadoraDto
{
    public int CodTransportadora { get; set; }
    public string? Cnpj { get; set; }
    public string NomeFantasia { get; set; } = null!;
    public string? Contato { get; set; }
    public int? CodEndereco { get; set; }
    public DateTime? CriadoEm { get; set; }
}