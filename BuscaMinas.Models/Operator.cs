using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    internal static class Operator
    {
        internal static List<Cell> Define(Board definingBoard)
        {
            for (int j = 0; j < definingBoard.Height; j++)
            {
                for (int i = 0; i < definingBoard.Width; i++)
                {
                    var cell = new Cell(i + 1, j + 1);
                    cell.CellWasClicked += definingBoard.Cell_WasClicked;
                    cell.CellWasRevealed += definingBoard.Cell_WasRevealed;
                    definingBoard.Cells.Add(cell);
                }
            }
            return definingBoard.Cells;
        }
        internal static List<Cell> Probe(Board probingBoard, Cell scout)
        {
            var perimeterX = new[] { scout.XValue - 1, scout.XValue, scout.XValue + 1 };
            var perimeterY = new[] { scout.YValue - 1, scout.YValue, scout.YValue + 1 };
            var perimeter = new List<Cell>();
            for (int j = 0; j < perimeterY.Length; j++)
            {
                for (int i = 0; i < perimeterX.Length; i++)
                {
                    var definedCell = probingBoard.Cells.Where(c => c.XValue == perimeterX[i] && c.YValue == perimeterY[j]).First();
                    perimeter.Add(definedCell);
                }
            }
            return perimeter;
        }
        internal static List<Cell> Arm(Board armingBoard, Cell seed)
        {
            var safeZone = Probe(armingBoard, seed);
            List<int> potentialMineIndexes = new();
            for (int i = 0; i < armingBoard.Cells.Count; i++)
            {
                //if (Cells[i] != seed)
                //{
                //    potentialMineIndexes.Add(i);
                //}
                if (!safeZone.Contains(armingBoard.Cells[i]))//this approach ensures a bigger initial opening
                {
                    potentialMineIndexes.Add(i);
                }
            }
            Shuffler.FisherYates(potentialMineIndexes);
            for (int i = 0; i < armingBoard.Mines; i++)
            {
                armingBoard.Cells[potentialMineIndexes[i]].Mined = true;
            }
            return armingBoard.Cells;
        }
        internal static List<Cell> Number(Board numberingBoard)
        {
            for (int i = 0; i < numberingBoard.Cells.Count; i++)
            {
                var numberingCell = numberingBoard.Cells[i];
                if (!numberingCell.Mined)
                {
                    var perimenter = Probe(numberingBoard, numberingCell);
                    for (int j = 0; j < perimenter.Count; j++)
                    {
                        var probedCell = perimenter[j];
                        if (probedCell.Mined)
                        {
                            numberingCell.NearbyMines++;
                        }
                    }
                }
            }
            return numberingBoard.Cells;
        }
        internal static List<Cell> Reveal(Board revealingBoard, Cell cell)
        {
            var perimeterToReveal = Probe(revealingBoard, cell);
            var revealedEmptyCells = new List<Cell>();
            for (int i = 0; i < perimeterToReveal.Count; i++)
            {
                var perimeterCell = perimeterToReveal[i];
                if (perimeterCell != null && !perimeterCell.Revealed && !perimeterCell.Mined && !perimeterCell.Flagged)
                {
                    perimeterCell.RevealCell();
                    if (perimeterCell.NearbyMines < 1)
                    {
                        revealedEmptyCells.Add(perimeterCell);
                    }
                }
            }
            for(int i = 0; i < revealedEmptyCells.Count; i++)
            {
                var revealedCell = revealedEmptyCells[i];
                Reveal(revealingBoard, revealedCell);
            }
            return revealingBoard.Cells;
        }
        internal static List<Cell> Freeze(List<Cell> freezingCells, Gamestate freezingState)
        {
            for (int i = 0; i < freezingCells.Count; i++)
            {
                var cell = freezingCells[i];
                if (freezingState == Gamestate.Over)
                {
                    if (cell.Mined & !cell.Flagged || cell.Flagged & !cell.Mined)
                    {
                        cell.Revealed = true;
                    }
                }
                else if (freezingState == Gamestate.Won)
                {
                    if (cell.Mined)
                    {
                        cell.Flagged = true;
                    }
                }
                cell.locked = true;
            }
            return freezingCells;
        }
    }
}
