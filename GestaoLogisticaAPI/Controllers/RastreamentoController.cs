using Microsoft.AspNetCore.Mvc;
using GestaoLogisticaAPI.Application.Interfaces;
using GestaoLogisticaAPI.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GestaoLogisticaAPI.Controllers;

[Authorize]
[Route("rastreamentos")]
public class RastreamentoController(IRastreamentoService service) : AbstractController
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
    public async Task<IActionResult> Create([FromBody] RastreamentoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        dto.CodRastreamento = 0;
        dto.CriadoEm = null;

        var result = await service.CreateAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });

        var created = result.Value!;
        return CreatedAtAction(nameof(Get), new { id = created.CodRastreamento }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RastreamentoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != dto.CodRastreamento)
            return BadRequest(new { message = "Id do rastreamento diferente do id no body." });

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