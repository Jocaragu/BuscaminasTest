using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public int XValue { get; set; }
        public int YValue { get; set; }
        public int NearbyMines { get; set; }
        public bool Revealed { get; set; }
        public bool Mined { get; set; }
        public bool Flagged { get; set; }
        public bool Questioned { get; set; }
        public Cell(int xValue, int yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
        public void LeftClick()
        {
            if(!Revealed && !Flagged)
            {
                Revealed = true;
            }
        }
        public void RightClick()
        {
            if(!Revealed)
            {
                Flagged = !Flagged;
            }
        }
    }
}
