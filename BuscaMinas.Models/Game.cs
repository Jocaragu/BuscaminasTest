using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Game
    {
        internal static Gamestate currentstate { get; set; } = Gamestate.Launching;
        public Board GameBoard { get; set; }
        public Game(int width, int height)
        {
            GameBoard = new Board(width, height);
            GameBoard.MineDetonated += Game_Over;
            currentstate = Gamestate.Playing;
        }

        private void Game_Over(object? sender, EventArgs e)
        {
            for (int i = 0; i < GameBoard.BoardCells.Count; i++)
            {
                var cell = GameBoard.BoardCells[i];
                {
                    if()
                }
            }
            currentstate = Gamestate.Over;
        }
    }
    internal enum Gamestate
    {
        Launching,
        Playing,
        Over,
    }
    //easy 9x9 and 15.625%
    //medium 16x16 and 16.40625%
    //hard 24x24 and 17.1875%
}
