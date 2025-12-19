namespace GestaoLogisticaAPI.Application.DTOs;

public class UsuarioDto
{
    public int CodUsuario { get; set; }
    public required string Apelido { get; set; }
    public string? NomeCompleto { get; set; }
    public string? Role { get; set; }
    public bool? Ativo { get; set; }
    public DateTime? CriadoEm { get; set; }
}