namespace GestaoLogisticaAPI.Domain.Entities;

public class Produto
{
    public int CodProduto { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal? PesoUnitKg { get; set; }
    public decimal? VolumeUnitM3 { get; set; }
    public bool Ativo { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<EntregaProduto> Entregaproduto { get; set; } = new List<EntregaProduto>();
    public virtual ICollection<Estoque> Estoque { get; set; } = new List<Estoque>();
}
