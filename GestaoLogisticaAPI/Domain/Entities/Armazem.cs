namespace GestaoLogisticaAPI.Domain.Entities;

public class Armazem
{
    public int CodArmazem { get; set; }
    public int? CodEndereco { get; set; }
    public string? Nome { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Endereco? CodEnderecoNavigation { get; set; }
    
    public virtual ICollection<Estoque> Estoque { get; set; } = new List<Estoque>();
}
