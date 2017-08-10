namespace MineSweeper
{
    public class MineField
    {               
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasBomb { get; set; }
        public int BombsNearMe { get; set; }        
        public bool HasFlag { get; set; }
        public bool IsOpenField { get; set; }

        public MineField()
        {
            X = 0;
            Y = 0;
            HasBomb = false;
            HasFlag = false;
            IsOpenField = false;
            BombsNearMe = -1;
        }

        public bool InvalidFlag()
        {
            return !HasBomb && HasFlag;
        }

        public bool HasExploded()
        {
            return HasBomb && IsOpenField;
        }        
    }
}