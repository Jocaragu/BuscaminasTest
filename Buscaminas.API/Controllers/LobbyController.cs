using Buscaminas.DAL.Repositories;
using BuscaMinas.Models;
using Microsoft.AspNetCore.Mvc;

namespace Buscaminas.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LobbyController : ControllerBase
{
    private readonly LobbyRepository _lobbyRepo;
    public LobbyController(LobbyRepository lobbyRepo)
    {
        _lobbyRepo = lobbyRepo;
    }

    [HttpGet]
    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _lobbyRepo.GetAllPlayersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetByIdAsync(int id)
    {
        try
        {
            var summonedPlayer = await _lobbyRepo.FindPlayerAsync(id);
            if (summonedPlayer == null)
            {
                return NotFound();
            }
            return summonedPlayer;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> PostAsync([FromBody] Player newPlayer)
    {
        try
        {
            var newPlayerId = await _lobbyRepo.AddNewPlayerAsync(newPlayer);
            return newPlayerId;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] Player putPlayer)
    {
        try
        {
            var playerToUpdate = await _lobbyRepo.FindPlayerAsync(id);
            if (playerToUpdate == null)
            {
                return NotFound();
            }
            await _lobbyRepo.UpdatePlayerAsync(putPlayer);
            return Ok(playerToUpdate);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var playerToDelete = await _lobbyRepo.FindPlayerAsync(id);
            if (playerToDelete == null)
            {
                return NotFound();
            }
            await _lobbyRepo.DismissPlayerAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
