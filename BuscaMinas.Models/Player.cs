using System.ComponentModel.DataAnnotations;

namespace BuscaMinas.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal HighScore { get; set; }
    }
}
