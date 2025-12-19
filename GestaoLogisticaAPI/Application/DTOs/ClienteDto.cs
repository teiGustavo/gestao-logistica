namespace GestaoLogisticaAPI.Application.DTOs;

public class ClienteDto
{
    public int CodCliente { get; set; }
    public string Nome { get; set; } = null!;
    public string? Documento { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public int? CodEndereco { get; set; }
    public DateTime? CriadoEm { get; set; }
}