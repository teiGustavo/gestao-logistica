namespace GestaoLogisticaAPI.Application.DTOs;

public class EstoqueDto
{
    public int CodEstoque { get; set; }
    public int CodArmazem { get; set; }
    public int CodProduto { get; set; }
    public int Quantidade { get; set; }
    public string? Lote { get; set; }
    public DateOnly? DataEntrada { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}