namespace GestaoLogisticaAPI.Domain.Entities;

public class Cliente
{
    public int CodCliente { get; set; }
    public int? CodEndereco { get; set; }
    public string? Nome { get; set; }
    public string? Documento { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Endereco? CodEnderecoNavigation { get; set; }
    public virtual ICollection<Entrega> EntregacodClienteDestinatarioNavigation { get; set; } = new List<Entrega>();
    public virtual ICollection<Entrega> EntregacodClienteRemetenteNavigation { get; set; } = new List<Entrega>();
}