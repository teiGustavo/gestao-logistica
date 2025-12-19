namespace GestaoLogisticaAPI.Application.DTOs;

public class ProdutoDto
{
    public int CodProduto { get; set; }
    public string Nome { get; set; } = null!;
    public string? Sku { get; set; }
    public decimal? PesoUnitKg { get; set; }
    public decimal? VolumeUnitM3 { get; set; }
    public DateTime? CriadoEm { get; set; }
}