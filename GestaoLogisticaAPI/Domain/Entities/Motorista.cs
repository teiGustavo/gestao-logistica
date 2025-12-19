namespace GestaoLogisticaAPI.Domain.Entities;

public class Motorista
{
    public int CodMotorista { get; set; }
    public string Nome { get; set; } = null!;
    public string? Cnh { get; set; }
    public string? Telefone { get; set; }
    public int? CodTransportadora { get; set; }
    public bool? Ativo { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Transportadora? CodTransportadoraNavigation { get; set; }
    public virtual ICollection<Entrega> Entrega { get; set; } = new List<Entrega>();
}
