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
        public bool SteppedOn { get; set; }
        public bool Mined { get; set; }
        public int NearbyMines { get; set; }
        public bool Flagged { get; set; }

        public Cell(int xValue, int yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }
}
