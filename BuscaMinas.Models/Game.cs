using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Game
    {
        public static Gamestate currentstate { get; set; }
        public Board GameBoard { get; set; }
        public Game(int width, int height)
        {
            GameBoard = new Board(width, height);
            currentstate = Gamestate.Launching;
        }
    }
    public enum Gamestate
    {
        Launching,
        Playing,
        Over,
        Exiting,
    }
}
