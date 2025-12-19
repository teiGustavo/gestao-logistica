using Microsoft.AspNetCore.Mvc;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using GestaoLogisticaAPI.Domain.Enums;

namespace GestaoLogisticaAPI.Controllers;

[Authorize(Roles = nameof(RoleUsuarioEnum.Admin))]
[Route("usuarios")]
public class UsuarioController(IUsuarioService service) : AbstractController
{
    [HttpGet]
    public async Task<IActionResult> GetAll() 
    {
        var result = await service.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { error = result.ErrorMessage });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.GetAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
            
        return NotFound(new { message = result.ErrorMessage ?? "Usuário não encontrado!" });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CadastroUsuarioDto dto)
    {
        var result = await service.CreateAsync(dto);
            
        if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });
            
        var created = result.Value!;
        return CreatedAtAction(nameof(Get), new { id = created.CodUsuario }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto dto)
    {
        if (id != dto.CodUsuario)
            return BadRequest(new { message = "Id do usuário diferente do id no body." });

        var result = await service.UpdateAsync(dto); 
            
        if (result.IsSuccess)
            return Ok(dto);

        return NotFound(new { message = result.ErrorMessage });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.DeleteAsync(id); 
            
        if (result.IsSuccess)
            return NoContent();

        return NotFound(new { message = result.ErrorMessage });
    }
}