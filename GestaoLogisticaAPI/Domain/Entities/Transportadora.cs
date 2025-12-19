namespace GestaoLogisticaAPI.Domain.Entities;

public class Transportadora
{
    public int CodTransportadora { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Cnpj { get; set; }
    public string? Contato { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<Entrega> Entrega { get; set; } = new List<Entrega>();
    public virtual ICollection<Motorista> Motorista { get; set; } = new List<Motorista>();
}
