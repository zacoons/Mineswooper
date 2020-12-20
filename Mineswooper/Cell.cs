namespace Mineswooper
{
    public class Cell
    {
        public int Row { get; }
        public int Col { get; }

        public bool HasBomb;

        bool _isRevealed;
        public bool IsRevealed
        {
            get => _isRevealed;
            set
            {
                _isRevealed = value;
                if (_isRevealed)
                    IsFlagged = false;
            }
        }
        public bool IsFlagged;

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   Row == cell.Row &&
                   Col == cell.Col;//&&
                   //HasBomb == cell.HasBomb &&
                   //IsRevealed == cell.IsRevealed;
        }

        public override int GetHashCode()
        {
            var hashCode = 1633773240;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Col.GetHashCode();
            //hashCode = hashCode * -1521134295 + HasBomb.GetHashCode();
            //hashCode = hashCode * -1521134295 + IsRevealed.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Cell({Row}, {Col})";
        }

        public CellKey ToKey() => new CellKey(this);
    }

    public class CellKey
    {
        readonly Cell _cell;
        public CellKey(Cell cell) => _cell = cell;

        public override bool Equals(object obj) =>
            obj is CellKey cellKey &&
                   _cell.Row == cellKey._cell.Row &&
                   _cell.Col == cellKey._cell.Col;

        public override int GetHashCode()
        {
            var hashCode = 1633773240;
            hashCode = hashCode * -1521134295 + _cell.Row.GetHashCode();
            hashCode = hashCode * -1521134295 + _cell.Col.GetHashCode();
            return hashCode;
        }
    }
}
