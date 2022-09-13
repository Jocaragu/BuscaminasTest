using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Board
    {
        public List<Cell> BoardCells { get; set; }
        public Board(int width, int height)
        {
            BoardCells = BoardingTheCells(width, height);
        }
        public List<Cell> BoardingTheCells(int width, int height)
        {
            var boardedCells = new List<Cell>();
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    boardedCells.Add(new Cell(i+1, j+1));
                }
            }
            return boardedCells;
        }
    }
}
