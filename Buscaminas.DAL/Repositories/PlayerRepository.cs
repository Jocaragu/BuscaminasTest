using Dapper;
using Buscaminas.DAL.Context;
using BuscaMinas.Models;
using System.Data;

namespace Buscaminas.DAL.Repositories
{
	public class PlayerRepository
	{
        private readonly DapperContext _context;
		public PlayerRepository(DapperContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Player>> GetAllPlayersAsync()
		{
			var sqlQuery = "SELECT * FROM Participants";
			using(var connection = _context.Connect())
			{
				var participants = await connection.QueryAsync<Player>(sqlQuery);
				return participants.ToList();
			}
		}
		public async Task<Player> GetPlayerByIdAsync(int searchedId)
		{
			var sqlQuery = "SELECT * FROM Participants WHERE player_id=@playerId";
			using(var connection = _context.Connect())
			{
				var foundPlayer = await connection.QueryFirstOrDefaultAsync<Player>(sqlQuery, new { playerId = searchedId });
				return foundPlayer;
			}
		}
        public async Task<int> AddNewPlayerAsync(Player newPlayer)
        {
            var sqlQuery = "INSERT INTO Participants(player_name,highest_score) OUTPUT INSERTED.[player_id] VALUES (@PlayerName,@HighestScore)";
            var parameters = new DynamicParameters();
			parameters.Add("@PlayerName", newPlayer.Name, DbType.String);
			parameters.Add("@HighestScore", newPlayer.HighestScore, DbType.Int32);
            using (var connection = _context.Connect())
            {
                var newPlayerId = await connection.ExecuteScalarAsync<int>(sqlQuery, parameters);
                return newPlayerId;
            }
        }
		public async Task UpdateHighestScoreAsync(Player updatingPlayer)
		{
			var sqlQuery = "UPDATE Participants SET highest_score=@HighestScore WHERE id=@UpdatedId";
            var parameters = new DynamicParameters();
			parameters.Add("@UpdatedId", updatingPlayer.Id, DbType.Int32);
			parameters.Add("@HighestScore", updatingPlayer.HighestScore, DbType.Int32);
            using (var connection = _context.Connect())
			{
				await connection.ExecuteAsync(sqlQuery);
			}
		}
		public async Task PartWaysAsync(int id)
		{
            var sqlQuery = "DELETE FROM Participants WHERE id=@GoodByeId";
            using (var connection = _context.Connect())
            {
                await connection.ExecuteAsync(sqlQuery, new { GoodByeId = id });
            }
        }
    }
}
