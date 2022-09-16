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
    //easy 9x9 and 15.625%
    //medium 16x16 and 16.40625%
    //hard 24x24 and 17.1875%
}
