namespace GestaoLogisticaAPI.Application.DTOs;

public class ArmazemDto
{
    public int CodArmazem { get; set; }
    public string Nome { get; set; } = null!;
    public int? CodEndereco { get; set; }
    public DateTime? CriadoEm { get; set; }
}