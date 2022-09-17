using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Cell// : INotifyPropertyChanged
    {
        //public int Id { get; set; }
        public int XValue { get; set; }
        public int YValue { get; set; }
        public int NearbyMines { get; set; }
        public bool Revealed { get; set; }
        public bool Mined { get; set; }
        public bool Flagged { get; set; }
        //public bool Questioned { get; set; }
        public Cell(int xValue, int yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
        public void LeftClick()
        {
            ClickedCellArgs ClickArgs = new(this);
            ClickedCell?.Invoke(this, ClickArgs);

            if (!Revealed && !Flagged)
            {
                Revealed = true;
            }
        }
        public void RightClick()
        {
            if (!Revealed)
            {
                Flagged = !Flagged;
            }
        }
        public event EventHandler<ClickedCellArgs>? ClickedCell;
    }
    public class ClickedCellArgs : EventArgs
    {
        public Cell CellBeingClicked { get; set; }
        public ClickedCellArgs(Cell cellBeingClicked)
        {
            CellBeingClicked = cellBeingClicked;
        }
    }
}
