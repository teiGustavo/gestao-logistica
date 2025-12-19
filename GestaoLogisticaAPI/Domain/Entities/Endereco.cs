namespace GestaoLogisticaAPI.Domain.Entities;

public class Endereco
{
    public int CodEndereco { get; set; }
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual ICollection<Armazem> Armazem { get; set; } = new List<Armazem>();
    public virtual ICollection<Cliente> Cliente { get; set; } = new List<Cliente>();
}
