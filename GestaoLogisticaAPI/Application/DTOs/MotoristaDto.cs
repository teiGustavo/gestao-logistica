namespace GestaoLogisticaAPI.Application.DTOs;

public class MotoristaDto
{
    public int CodMotorista { get; set; }
    public string Nome { get; set; } = null!;
    public string? Cnh { get; set; }
    public string? Telefone { get; set; }
    public int? CodTransportadora { get; set; }
    public bool? Ativo { get; set; }
    public DateTime? CriadoEm { get; set; }
}