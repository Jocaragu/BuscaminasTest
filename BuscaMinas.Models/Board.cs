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
        //public event EventHandler<string> GameCellsAddingNewEvent;

        public BindingList<Cell> GameCells { get; set; }
        //public List<Cell> BoardCells { get; set; }
        public Board(int width, int height)
        {
            //BoardCells = BoardingTheCells(width, height);
            GameCells = BoardingGameCells(width,height);
        }

        //public List<Cell> BoardingTheCells(int width, int height)
        //{
        //    var boardedCells = new List<Cell>();
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            boardedCells.Add(new Cell(i + 1, j + 1));
        //        }
        //    }
        //    return boardedCells;
        //}
        public BindingList<Cell> BoardingGameCells(int width, int height)
        {
            var boardingGameCells = new BindingList<Cell>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell cell = new(i + 1, j + 1);
                    cell.CellWasRevealed += Cell_CellWasRevealed;
                    boardingGameCells.Add(cell);
                }
            }
            //boardingGameCells.ListChanged += BoardingGameCells_ListChanged;
            return boardingGameCells;
        }

        //private void BoardingGameCells_ListChanged(object? sender, ListChangedEventArgs e)
        //{
        //    Console.WriteLine("CHANGE CHANGE CHANGE");
        //}
        private void Cell_CellWasRevealed(object? sender, CellWasRevealedArgs e)
        {
            var xArg = e.XRevelationValue;
            var yArg = e.YRevelationValue;
            Console.WriteLine("REVELATION REVELATION REVELATION");
        }

        //public void MineTheCells(Cell seed)
        //{
        //    for (int i = 0; i < BoardCells.Count; i++)
        //    {
        //        var cell = BoardCells[i];
        //        if (seed.XValue != cell.XValue && seed.YValue != cell.YValue)
        //        {
        //            cell.Mined = true;
        //        }
        //    }
        //}
    }
}
