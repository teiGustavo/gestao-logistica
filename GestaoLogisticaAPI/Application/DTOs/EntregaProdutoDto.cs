namespace GestaoLogisticaAPI.Application.DTOs;

public class EntregaProdutoDto
{
    public int CodEntregaProduto { get; set; }
    public int CodEntrega { get; set; }
    public int CodProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal? PesoUnitKg { get; set; }
    public decimal? VolumeUnitM3 { get; set; }
    public DateTime? CriadoEm { get; set; }
}