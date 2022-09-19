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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Mines { get; private set; }
        private bool _boardIsMined = false;
        private int _revealedCells = 0;

        public List<Cell> BoardCells { get; private set; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            BoardCells = BoardingTheCells(Width, Height);
        }

        private List<Cell> BoardingTheCells(int width, int height)
        {
            var boardedCells = new List<Cell>();
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    var cell = new Cell(i + 1, j + 1);
                    cell.CellWasClicked += Cell_WasClicked;
                    cell.CellWasRevealed += Cell_WasRevealed;
                    boardedCells.Add(cell);
                }
            }
            return boardedCells;
        }

        private void Cell_WasClicked(object? sender, CellWasClickedArgs e)
        {
            if (!_boardIsMined)
            {
                MiningTheBoard(e.ClickedCell.XValue, e.ClickedCell.YValue);
            }

            if (e.ClickedCell.Mined)
            {
                Console.WriteLine("KA BOOM");
                MineDetonated?.Invoke(this, EventArgs.Empty);
            }
            else if (e.ClickedCell.NearbyMines < 1 && !e.ClickedCell.Mined)
            {
                RevealPerimeter(e.ClickedCell.XValue, e.ClickedCell.YValue);
            }
        }
        private void Cell_WasRevealed(object? sender, EventArgs e)
        {
            _revealedCells++;
            var threshold = Width * Height - Mines;
            if(_revealedCells == threshold)
            {
                Console.WriteLine("All mines have been detected!");
                AllMinesDetectedArgs AllDetectedArgs = new(this);
                AllMinesDetected?.Invoke(this, AllDetectedArgs);
            }
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
            int maxNumberOfMines = (int)(Width * Height * 0.166);
            for (int i = 0; i < maxNumberOfMines; i++)
            {
                BoardCells[potentialMineIndexes[i]].Mined = true;
            }

            _boardIsMined = true;
            NumberingNearbyMines();
        }

        private void NumberingNearbyMines()
        {
            for (int i = 0; i < BoardCells.Count; i++)
            {
                var scoutingCell = BoardCells[i];
                if (!scoutingCell.Mined)
                {
                    var perimenter = DefinePerimeter(scoutingCell.XValue, scoutingCell.YValue);
                    for (int j = 0; j < perimenter.Count; j++)
                    {
                        var probedCell = perimenter[j];
                        if (probedCell.Mined)
                        {
                            scoutingCell.NearbyMines++;
                        }
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
                    //perimeterCell.Revealed = true;
                    perimeterCell.RevealCell();
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

        internal event EventHandler? MineDetonated;
        internal event EventHandler<AllMinesDetectedArgs>? AllMinesDetected;
    }
    internal class AllMinesDetectedArgs : EventArgs
    {
        internal int TotalCells { get; set; }
        internal int TotalMines { get; set; }
        internal AllMinesDetectedArgs(Board clearedBoard)
        {
            TotalCells = clearedBoard.Width * clearedBoard.Height;
            TotalMines = clearedBoard.Mines;
        }
    }
}
