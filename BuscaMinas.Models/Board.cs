using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Board
    {
        private bool _boardIsMined = false;
        private int _revealedCells;
        private int threshold;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Mines { get; private set; }
        public List<Cell> Cells { get; private set; } = new();

        public Board(int width, int height, int mines)
        {
            Width = width;
            Height = height;
            Mines = mines;
            //Cells = BoardingTheCells(Width, Height);
            Cells = Operator.Define(this);
            threshold = (Width * Height) - Mines;
        }
        //private List<Cell> BoardingTheCells(int width, int height)
        //{
        //    var boardedCells = new List<Cell>();
        //    for (int j = 0; j < height; j++)
        //    {
        //        for (int i = 0; i < width; i++)
        //        {
        //            var cell = new Cell(i + 1, j + 1);
        //            cell.CellWasClicked += Cell_WasClicked;
        //            cell.CellWasRevealed += Cell_WasRevealed;
        //            boardedCells.Add(cell);
        //        }
        //    }
        //    return boardedCells;
        //}
        internal void Cell_WasClicked(object? sender, CellWasClickedArgs e)
        {
            if (!_boardIsMined)
            {
                //MiningTheBoard(e.ClickedCell.XValue, e.ClickedCell.YValue);
                Cells = Operator.Arm(this, e.ClickedCell);
                Cells = Operator.Number(this);
                _boardIsMined = true;
            }

            if (e.ClickedCell.Mined)
            {
                Console.WriteLine("KA BOOM");
                MineDetonated?.Invoke(this, EventArgs.Empty);
            }
            else if (e.ClickedCell.NearbyMines < 1 && !e.ClickedCell.Mined)
            {
                //RevealPerimeter(e.ClickedCell.XValue, e.ClickedCell.YValue);
                Operator.Reveal(this, e.ClickedCell);
            }
        }
        internal void Cell_WasRevealed(object? sender, EventArgs e)
        {
            if (_revealedCells == threshold)
            {
                Console.WriteLine("All mines have been detected!");
                AllMinesDetectedArgs AllDetectedArgs = new(this);
                AllMinesDetected?.Invoke(this, AllDetectedArgs);
            }
            else
            {
                _revealedCells++;
            }
        }
        //private void MiningTheBoard(int xSeed, int ySeed)
        //{
        //    var safeZone = DefinePerimeter(xSeed, ySeed);
        //    List<int> potentialMineIndexes = new();
        //    for (int i = 0; i < Cells.Count; i++)
        //    {
        //        //if (Cells[i] != seed)
        //        //{
        //        //    potentialMineIndexes.Add(i);
        //        //}
        //        if (!safeZone.Contains(Cells[i]))//this approach ensures a bigger initial opening
        //        {
        //            potentialMineIndexes.Add(i);
        //        }
        //    }

        //    Shuffler.FisherYates(potentialMineIndexes);

        //    for (int i = 0; i < Mines; i++)
        //    {
        //        Cells[potentialMineIndexes[i]].Mined = true;
        //    }
        //    //_boardIsMined = true;
        //    NumberingNearbyMines();
        //}
        //private void NumberingNearbyMines()
        //{
        //    for (int i = 0; i < Cells.Count; i++)
        //    {
        //        var scoutingCell = Cells[i];
        //        if (!scoutingCell.Mined)
        //        {
        //            var perimenter = DefinePerimeter(scoutingCell.XValue, scoutingCell.YValue);
        //            for (int j = 0; j < perimenter.Count; j++)
        //            {
        //                var probedCell = perimenter[j];
        //                if (probedCell.Mined)
        //                {
        //                    scoutingCell.NearbyMines++;
        //                }
        //            }
        //        }
        //    }
        //}
        //private void RevealPerimeter(int x, int y)
        //{
        //    var perimeterToReveal = DefinePerimeter(x, y);
        //    var revealedEmptyCells = new List<Cell>();
        //    for (int i = 0; i < perimeterToReveal.Count; i++)
        //    {
        //        var perimeterCell = perimeterToReveal[i];
        //        if (perimeterCell != null && !perimeterCell.Revealed && !perimeterCell.Mined && !perimeterCell.Flagged)
        //        {
        //            perimeterCell.RevealCell();
        //            if (perimeterCell.NearbyMines < 1)
        //            {
        //                revealedEmptyCells.Add(perimeterCell);
        //            }
        //        }
        //    }
        //    foreach (var revealedEmptyCell in revealedEmptyCells)
        //    {
        //        //RevealPerimeter(revealedEmptyCell.XValue, revealedEmptyCell.YValue);
        //    }
        //}
        //private List<Cell> DefinePerimeter(int centerX, int centerY)
        //{
        //    var perimeter = new List<Cell>();
        //    var perimeterX = new[] { centerX - 1, centerX, centerX + 1 };
        //    var perimeterY = new[] { centerY - 1, centerY, centerY + 1 };
        //    for (int j = 0; j < perimeterY.Length; j++)
        //    {
        //        for (int i = 0; i < perimeterX.Length; i++)
        //        {
        //            var definedCell = Cells.Where(c => c != null && c.XValue == perimeterX[i] && c.YValue == perimeterY[j]).FirstOrDefault();
        //            if (definedCell != null)
        //            {
        //                perimeter.Add(definedCell);
        //            }
        //        }
        //    }
        //    return perimeter;
        //}

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
