namespace GestaoLogisticaAPI.Domain.Entities;

public class Rota
{
    public int CodRota { get; set; }
    public int Origem { get; set; }
    public int Destino { get; set; }
    public decimal? DistanciaKm { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<Entrega> Entrega { get; set; } = new List<Entrega>();
}
