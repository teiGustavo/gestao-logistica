namespace GestaoLogisticaAPI.Domain.Entities;

public class Entrega
{
    public int CodEntrega { get; set; }
    public string? CodigoExterno { get; set; }
    public int? CodClienteRemetente { get; set; }
    public int? CodClienteDestinatario { get; set; }
    public int? CodTransportadora { get; set; }
    public int? CodVeiculo { get; set; }
    public int? CodMotorista { get; set; }
    public int? CodRota { get; set; }
    public DateTime? DataPedido { get; set; }
    public DateOnly? DataPrevisaoEntrega { get; set; }
    public decimal? PesoTotalKg { get; set; }
    public decimal? VolumeTotalM3 { get; set; }
    public decimal? ValorFrete { get; set; }
    public string? StatusEntrega { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Cliente? CodClienteDestinatarioNavigation { get; set; }
    public virtual Cliente? CodClienteRemetenteNavigation { get; set; }
    public virtual Motorista? CodMotoristaNavigation { get; set; }
    public virtual Rota? CodRotaNavigation { get; set; }
    public virtual Transportadora? CodTransportadoraNavigation { get; set; }
    public virtual Veiculo? CodVeiculoNavigation { get; set; }
    public virtual ICollection<EntregaProduto> Entregaproduto { get; set; } = new List<EntregaProduto>();
    public virtual ICollection<Rastreamento> Rastreamento { get; set; } = new List<Rastreamento>();
}
