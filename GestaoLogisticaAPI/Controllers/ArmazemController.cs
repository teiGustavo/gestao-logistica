using Microsoft.AspNetCore.Mvc;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GestaoLogisticaAPI.Controllers;

[Authorize]
[Route("armazens")]
public class ArmazemController(IArmazemService service) : AbstractController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAllAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(new { error = result.ErrorMessage });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.GetAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(new { message = result.ErrorMessage });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ArmazemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // garante que cliente não envie id (id será gerado pelo BD/repositório)
        dto.CodArmazem = 0;
        // ideal: deixar criado_em nulo para o Service preencher com DateTime.UtcNow
        dto.CriadoEm = null;

        var result = await service.CreateAsync(dto);

        if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });

        var created = result.Value!;
        return CreatedAtAction(nameof(Get), new { id = created.CodArmazem }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ArmazemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != dto.CodArmazem)
            return BadRequest(new { message = "Id da rota diferente do id no body." });

        var result = await service.UpdateAsync(dto);

        if (result.IsSuccess) return Ok(dto);
        return NotFound(new { message = result.ErrorMessage });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.DeleteAsync(id);
        if (result.IsSuccess) return NoContent();
        return NotFound(new { message = result.ErrorMessage });
    }

}