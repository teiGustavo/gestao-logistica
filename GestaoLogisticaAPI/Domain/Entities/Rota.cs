namespace GestaoLogisticaAPI.Domain.Entities;

public class Rota
{
    public int CodRota { get; set; }
    public string? Origem { get; set; }
    public string? Destino { get; set; }
    public double? DistanciaKm { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<Entrega> Entrega { get; set; } = new List<Entrega>();
}
