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
        public int Id { get; set; }
        public int XValue { get; set; }
        public int YValue { get; set; }
        public int NearbyMines { get; set; }
        //private bool _revealed = false;
        //public bool Revealed 
        //{ 
        //    get 
        //    {
        //        return _revealed; 
        //    }
        //    set 
        //    { 
        //        if(value != _revealed)
        //        {
        //            _revealed = value;
        //            NotifyPropertyChanged();
        //        }
        //    } 
        //}
        public bool Revealed { get; set; }
        public bool Mined { get; set; }
        public bool Flagged { get; set; }
        public bool Questioned { get; set; }
        public CellWasRevealedArgs RevealedArgs { get; set; }
        public Cell(int xValue, int yValue)
        {
            XValue = xValue;
            YValue = yValue;
            RevealedArgs = new(xValue, yValue);
        }
        public void LeftClick()
        {
            if(!Revealed && !Flagged)
            {
                Revealed = true;
                CellWasRevealed?.Invoke(this, RevealedArgs);
            }
        }
        public void RightClick()
        {
            if(!Revealed)
            {
                Flagged = !Flagged;
            }
        }
        public event EventHandler<CellWasRevealedArgs>? CellWasRevealed;
        //public event PropertyChangedEventHandler? PropertyChanged;
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }
    public class CellWasRevealedArgs : EventArgs
    {
        public int XRevelationValue { get; set; }
        public int YRevelationValue { get; set; }
        public CellWasRevealedArgs(int xEventValue, int yEventValue)
        {
            XRevelationValue = xEventValue;
            YRevelationValue = yEventValue;
        }
    }
}
