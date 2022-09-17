using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Cell
    {
        //public int Id { get; set; }
        public int XValue { get; set; }
        public int YValue { get; set; }
        public bool Flagged { get; set; } = false;
        public bool SteppedOn { get; set; } = false;
        public bool Revealed { get; set; } = false;
        public bool Mined { get; set; } = false;
        public int NearbyMines { get; set; }

        //public bool Questioned { get; set; }
        internal Cell(int xValue, int yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
        public void LeftClick()
        {
            CellWasClickedArgs ClickArgs = new(this);
            CellWasClicked?.Invoke(this, ClickArgs);
            if (!SteppedOn)
            {
                SteppedOn = true;
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
        internal event EventHandler<CellWasClickedArgs>? CellWasClicked;
    }
    internal class CellWasClickedArgs : EventArgs
    {
        internal Cell ClickedCell { get; set; }
        internal CellWasClickedArgs(Cell cellBeingClicked)
        {
            ClickedCell = cellBeingClicked;
        }
    }
}
