using GestaoLogisticaAPI.Application.DTOs;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GestaoLogisticaAPI.Controllers;

[Route("auth")]
public class AuthController(IAuthService auth, IUsuarioService service) : AbstractController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await auth.LoginAsync(dto.Apelido, dto.Senha);
        
        if (token == null)
            return Unauthorized(new { message = "Apelido ou senha inválidos" });

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CadastroUsuarioDto dto)
    {
        dto.Ativo = true;
        dto.Role = nameof(RoleUsuarioEnum.Operador);
        
        var result = await service.CreateAsync(dto);
        
        if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });
      
        return Ok(new { message = "Usuário criado com sucesso" });
    }
}