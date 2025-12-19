namespace GestaoLogisticaAPI.Application.DTOs;

public class CadastroUsuarioDto : UsuarioDto
{
    public required string Senha { get; set; }
}