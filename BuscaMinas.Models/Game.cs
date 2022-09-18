using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Game
    {
        public Gamestate Currentstate { get; private set; } = Gamestate.Launching;
        public Board GameBoard { get; internal set; }

        public Game(int width, int height)
        {
            GameBoard = new Board(width, height);
            GameBoard.MineDetonated += Game_Over;
            Currentstate = Gamestate.Playing;
        }

        private void Game_Over(object? sender, EventArgs e)
        {
            for (int i = 0; i < GameBoard.BoardCells.Count; i++)
            {
                var cell = GameBoard.BoardCells[i];
                if (cell.Mined & !cell.Flagged || cell.Flagged & !cell.Mined)
                {
                    cell.Revealed = true;
                }
                cell.locked = true;
            }
            Currentstate = Gamestate.Over;
        }
    }
    public enum Gamestate
    {
        Launching,
        Playing,
        Over,
    }
    //easy 9x9 and 15.625%
    //medium 16x16 and 16.40625%
    //hard 24x24 and 17.1875%
}
