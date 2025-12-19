namespace GestaoLogisticaAPI.Domain.Entities;

public class Veiculo
{
    public int CodVeiculo { get; set; }
    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<Entrega> Entrega { get; set; } = new List<Entrega>();
}
