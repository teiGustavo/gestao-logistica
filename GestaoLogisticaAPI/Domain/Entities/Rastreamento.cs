namespace GestaoLogisticaAPI.Domain.Entities;

public class Rastreamento
{
    public int CodRastreamento { get; set; }
    public int CodEntrega { get; set; }
    public string? Status { get; set; }
    public DateTime? DataHora { get; set; }

    public virtual Entrega CodEntregaNavigation { get; set; } = null!;
}
