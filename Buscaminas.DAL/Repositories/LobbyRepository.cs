using Dapper;
using Buscaminas.DAL.Context;
using BuscaMinas.Models;
using System.Data;

namespace Buscaminas.DAL.Repositories;

public class LobbyRepository
{
    private readonly DapperContext _context;
    public LobbyRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Player>> GetAllPlayersAsync()
    {
        var sqlQuery = "SELECT * FROM Lobby";
        using (var connection = _context.Connect())
        {
            var lobby = await connection.QueryAsync<Player>(sqlQuery);
            return lobby.ToList();
        }
    }
    public async Task<Player> FindPlayerAsync(int searchedId)
    {
        var sqlQuery = "SELECT * FROM Lobby WHERE id=@playerId";
        using (var connection = _context.Connect())
        {
            return await connection.QueryFirstOrDefaultAsync<Player>(sqlQuery, new { playerId = searchedId });
        }
    }
    public async Task<int> AddNewPlayerAsync(Player newPlayer)
    {
        var sqlQuery = "INSERT INTO Lobby(name,highScore) OUTPUT INSERTED.[id] VALUES (@NewName,@NewScore)";
        var parameters = new DynamicParameters();
        parameters.Add("@NewName", newPlayer.Name, DbType.String);
        parameters.Add("@NewScore", newPlayer.HighScore, DbType.Int32);
        using (var connection = _context.Connect())
        {
            return await connection.ExecuteScalarAsync<int>(sqlQuery, parameters);
        }
    }
    public async Task UpdatePlayerAsync(Player updatingPlayer)
    {
        var sqlQuery = "UPDATE Lobby SET highScore=@HighScore WHERE id=@Id";
        using (var connection = _context.Connect())
        {
            await connection.ExecuteAsync(sqlQuery, new {Id = updatingPlayer.Id, HighScore = updatingPlayer.HighScore});
        }
    }
    public async Task DismissPlayerAsync(int id)
    {
        var sqlQuery = "DELETE FROM Lobby WHERE id=@GoodByeId";
        using (var connection = _context.Connect())
        {
            await connection.ExecuteAsync(sqlQuery, new { GoodByeId = id });
        }
    }
}
