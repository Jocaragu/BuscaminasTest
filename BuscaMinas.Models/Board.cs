using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BuscaMinas.Models.Cell;

namespace BuscaMinas.Models
{
    public class Board
    {
        private bool _boardIsMined = false;
        public List<Cell> BoardCells { get; set; }

        public Board(int width, int height)
        {
            BoardCells = BoardingTheCells(width, height);
        }

        public List<Cell> BoardingTheCells(int width, int height)
        {
            var boardedCells = new List<Cell>();
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    var cell = new Cell(i + 1, j + 1);
                    cell.CellWasRevealed += Cell_CellWasRevealed;
                    boardedCells.Add(cell);
                }
            }
            return boardedCells;
        }

        private void Cell_CellWasRevealed(object? sender, CellWasRevealedArgs e)
        {
            var xArg = e.XRevelationValue;
            var yArg = e.YRevelationValue;
            if (!_boardIsMined)
            {
                MiningTheBoard(xArg, yArg);
            }
            RevealPerimeter(xArg, yArg);
        }

        private void MiningTheBoard(int x, int y)
        {
            var miningPerimeter = DefinePerimeter(x, y);
            foreach(var boardedCell in BoardCells)
            {
                if (!miningPerimeter.Contains(boardedCell))
                {
                    boardedCell.Mined = true;
                }
            }
            _boardIsMined = true;
            NumberingNearbyMines();
        }

        private void NumberingNearbyMines()
        {
            foreach (var scoutingCell in BoardCells)
            {
                var perimeter = DefinePerimeter(scoutingCell.XValue, scoutingCell.YValue);
                foreach(var probedCell in perimeter)
                {
                    if(probedCell!=null && probedCell.Mined)
                    {
                        scoutingCell.NearbyMines++;
                    }
                }
            }
        }

        private void RevealPerimeter(int x, int y)
        {
            var revealedPerimeter = DefinePerimeter(x, y);
            foreach(var cellToReveal in BoardCells)
            {
                if (revealedPerimeter.Contains(cellToReveal) && !cellToReveal.Revealed && !cellToReveal.Mined && !cellToReveal.Flagged)
                {
                    cellToReveal.LeftClick();
                }
            }
        }

        private List<Cell> DefinePerimeter(int centerX, int centerY)
        {
            var perimeter = new List<Cell>();
            var perimeterX = new[] { centerX - 1, centerX, centerX + 1 };
            var perimeterY = new[] {centerY-1, centerY, centerY + 1 };
            for (int j = 0; j < perimeterY.Length; j++)
            {
                for (int i = 0; i < perimeterX.Length; i++)
                {
                    var definedCell = BoardCells.Where(c => c.XValue == perimeterX[i] && c.YValue == perimeterY[j]).FirstOrDefault();
                    if(definedCell != null)
                    {
                        perimeter.Add(definedCell);
                    }
                }
            }
            return perimeter;
        }
    }
}
