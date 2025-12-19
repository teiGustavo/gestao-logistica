using GestaoLogisticaAPI.Domain.Enums;

namespace GestaoLogisticaAPI.Domain.Entities;

public class Usuario
{
    public int CodUsuario { get; set; }          // chave primária
    public string Apelido { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? NomeCompleto { get; set; }
    public RoleUsuarioEnum Role { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}