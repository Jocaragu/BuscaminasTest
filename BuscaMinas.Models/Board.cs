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
        private readonly int _width;
        private readonly int _height;
        public List<Cell> BoardCells { get; set; }

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            BoardCells = BoardingTheCells(_width, _height);
        }

        public List<Cell> BoardingTheCells(int width, int height)
        {
            var boardedCells = new List<Cell>();
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    var cell = new Cell(i + 1, j + 1);
                    cell.ClickedCell += Cell_CellWasClicked;
                    boardedCells.Add(cell);
                }
            }
            return boardedCells;
        }

        private void Cell_CellWasClicked(object? sender, ClickedCellArgs e)
        {
            var xArg = e.CellBeingClicked.XValue;
            var yArg = e.CellBeingClicked.YValue;
            if (!_boardIsMined)
            {
                //MiningTheBoard(e.CellBeingClicked);
                MiningTheBoard(xArg, yArg);
            }
            if (e.CellBeingClicked.NearbyMines < 1)
            {
                RevealPerimeter(xArg, yArg);
            }
            Console.WriteLine($"you clicked at {xArg} and {yArg}");
        }
        private void MiningTheBoard(int xSeed, int ySeed)
        {
            var safeZone = DefinePerimeter(xSeed, ySeed);
            List<int> potentialMineIndexes = new();
            for (int i = 0; i < BoardCells.Count; i++)
            {
                //if (BoardCells[i] != seed)
                //{
                //    potentialMineIndexes.Add(i);
                //}
                if (!safeZone.Contains(BoardCells[i]))//this approach ensures a bigger initial opening
                {
                    potentialMineIndexes.Add(i);
                }
            }
            Shuffler.FisherYates(potentialMineIndexes);
            int maxNumberOfMines = (int)(_width * _height * 0.15);
            for (int i = 0; i < maxNumberOfMines; i++)
            {
                BoardCells[potentialMineIndexes[i]].Mined = true;
            }

            _boardIsMined = true;
            NumberingNearbyMines();
        }
        //private void MiningTheBoard(Cell seed)
        //{
        //    List<int> potentialMineIndexes = new();
        //    for (int i = 0; i < BoardCells.Count; i++)
        //    {
        //        if (BoardCells[i] != seed)
        //        {
        //            potentialMineIndexes.Add(i);
        //        }
        //    }
        //    Shuffler.FisherYates(potentialMineIndexes);
        //    int maxNumberOfMines = (int)(_width * _height * 0.15);
        //    for (int i = 0; i < maxNumberOfMines; i++)
        //    {
        //        BoardCells[potentialMineIndexes[i]].Mined = true;
        //    }

        //    _boardIsMined = true;
        //    NumberingNearbyMines();
        //}

        //private void MiningTheBoard(int xSeedValue, int ySeedValue)//for testing purposes only
        //{
        //    int[] mines = { 5, 7, 10, 12, 13, 19, 61, 63, 69, 99 };
        //    var miningPerimeter = DefinePerimeter(xSeedValue, ySeedValue);
        //    for (int i = 0; i < mines.Length; i++)
        //    {
        //        var minePlace = mines[i];
        //        BoardCells[minePlace].Mined = true;
        //    }
        //    _boardIsMined = true;
        //    NumberingNearbyMines();
        //}

        private void NumberingNearbyMines()
        {
            foreach (var scoutingCell in BoardCells)
            {
                var perimeter = DefinePerimeter(scoutingCell.XValue, scoutingCell.YValue);
                foreach (var probedCell in perimeter)
                {
                    if (probedCell != null && probedCell.Mined)
                    {
                        scoutingCell.NearbyMines++;
                    }
                }
            }
        }

        private void RevealPerimeter(int x, int y)
        {
            var perimeterToReveal = DefinePerimeter(x, y);
            var revealedEmptyCells = new List<Cell>();
            for (int i = 0; i < perimeterToReveal.Count; i++)
            {
                var perimeterCell = perimeterToReveal[i];
                if (perimeterCell != null && !perimeterCell.Revealed && !perimeterCell.Mined && !perimeterCell.Flagged)
                {
                    perimeterCell.Revealed = true;
                    if (perimeterCell.NearbyMines < 1)
                    {
                        revealedEmptyCells.Add(perimeterCell);
                    }
                }
            }
            foreach (var revealedEmptyCell in revealedEmptyCells)
            {
                RevealPerimeter(revealedEmptyCell.XValue, revealedEmptyCell.YValue);
            }
        }

        private List<Cell> DefinePerimeter(int centerX, int centerY)
        {
            var perimeter = new List<Cell>();
            var perimeterX = new[] { centerX - 1, centerX, centerX + 1 };
            var perimeterY = new[] { centerY - 1, centerY, centerY + 1 };
            for (int j = 0; j < perimeterY.Length; j++)
            {
                for (int i = 0; i < perimeterX.Length; i++)
                {
                    var definedCell = BoardCells.Where(c => c != null && c.XValue == perimeterX[i] && c.YValue == perimeterY[j]).FirstOrDefault();
                    if (definedCell != null)
                    {
                        perimeter.Add(definedCell);
                    }
                }
            }
            return perimeter;
        }
    }
}
