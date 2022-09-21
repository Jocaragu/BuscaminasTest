namespace BuscaMinas.Models
{
    public class Cell
    {
        internal bool locked = false;
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
            if (!locked)
            {
                CellWasClickedArgs ClickArgs = new(this);
                CellWasClicked?.Invoke(this, ClickArgs);
                if (!SteppedOn)
                {
                    SteppedOn = true;
                    RevealCell();
                }
            }
        }
        public void RightClick()
        {
            if (!locked)
            {
                if (!Revealed)
                {
                    Flagged = !Flagged;
                    CellFlagToggledArgs ToggleArgs = new(this);
                    CellFlagToggled?.Invoke(this, ToggleArgs);
                }
            }
        }
        internal void RevealCell()
        {
            if (!Revealed)
            {
                Revealed = true;
                CellWasRevealed?.Invoke(this, EventArgs.Empty);
            }
        }
        internal event EventHandler<CellWasClickedArgs>? CellWasClicked;
        internal event EventHandler? CellWasRevealed;
        internal event EventHandler<CellFlagToggledArgs>? CellFlagToggled;
    }
    internal class CellWasClickedArgs : EventArgs
    {
        internal Cell ClickedCell { get; set; }
        internal CellWasClickedArgs(Cell cellBeingClicked)
        {
            ClickedCell = cellBeingClicked;
        }
    }
    internal class CellFlagToggledArgs : EventArgs
    {
        internal Cell cellToggled { get; set; }
        internal CellFlagToggledArgs(Cell cellBeingToggled)
        {
            cellToggled = cellBeingToggled;
        }
    }
}
