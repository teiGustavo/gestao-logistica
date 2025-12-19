namespace GestaoLogisticaAPI.Domain.Entities;

public class Estoque
{
    public int CodEstoque { get; set; }
    public int CodProduto { get; set; }
    public int CodArmazem { get; set; }
    public int Quantidade { get; set; }
    // public DateTime? atualizado_em { get; set; } // Não mapeado no AppDbContext

    public virtual Armazem CodArmazemNavigation { get; set; } = null!;
    public virtual Produto CodProdutoNavigation { get; set; } = null!;
}
