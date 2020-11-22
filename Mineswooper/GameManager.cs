using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Mineswooper
{
    public class GameManager
    {
        int gameSize;

        public Cell[] cells;

        public void Start(int gameSize, bool createBombs = true)
        {
            this.gameSize = gameSize;
            int cellCount = gameSize * gameSize;
            cells = new Cell[cellCount];

            for (int i = 0; i < cellCount; i++)
                cells[i] = new Cell(i / gameSize, i % gameSize);

            if (createBombs)
            {
                var rnd = new Random();
                for (int i = 0; i < gameSize; i++)
                {
                    do
                    {
                        var index = rnd.Next(0, cells.Length);
                        if (cells[index].HasBomb == false)
                        {
                            cells[index].HasBomb = true;
                            break;
                        }
                    }
                    while (true);
                }
            }
        }

        public Cell GetCellAt(int row, int col)
        {
            return cells[row * gameSize + col];
        }

        public Cell[] GetAdjacentCells(Cell cell)
        {
            //Other
            if (cell.Row > 0 && cell.Row < gameSize - 1 &&
                cell.Col > 0 && cell.Col < gameSize - 1)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col - 1),
                    GetCellAt(cell.Row - 1, cell.Col),
                    GetCellAt(cell.Row - 1, cell.Col + 1),

                    GetCellAt(cell.Row, cell.Col - 1),
                    GetCellAt(cell.Row, cell.Col + 1),

                    GetCellAt(cell.Row + 1, cell.Col - 1),
                    GetCellAt(cell.Row + 1, cell.Col),
                    GetCellAt(cell.Row + 1, cell.Col + 1),
                };

            //Corners
            if (cell.Row == 0 && cell.Col == 0)
                return new[]
                {
                    GetCellAt(cell.Row, cell.Col + 1),

                    GetCellAt(cell.Row + 1, cell.Col),
                    GetCellAt(cell.Row + 1, cell.Col + 1),
                };
            if (cell.Row == gameSize - 1 && cell.Col == gameSize - 1)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col - 1),
                    GetCellAt(cell.Row - 1, cell.Col),

                    GetCellAt(cell.Row, cell.Col - 1),
                };
            if (cell.Row == gameSize - 1 && cell.Col == 0)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col),
                    GetCellAt(cell.Row - 1, cell.Col + 1),

                    GetCellAt(cell.Row, cell.Col + 1),
                };
            if (cell.Row == 0 && cell.Col == gameSize - 1)
                return new[]
                {
                    GetCellAt(cell.Row, cell.Col - 1),

                    GetCellAt(cell.Row + 1, cell.Col - 1),
                    GetCellAt(cell.Row + 1, cell.Col),
                };

            //Edges
            if (cell.Row > 0 && cell.Col == 0)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col),
                    GetCellAt(cell.Row - 1, cell.Col + 1),

                    GetCellAt(cell.Row, cell.Col + 1),

                    GetCellAt(cell.Row + 1, cell.Col),
                    GetCellAt(cell.Row + 1, cell.Col + 1),
                };
            if (cell.Row == 0 && cell.Col > 0)
                return new[]
                {
                    GetCellAt(cell.Row, cell.Col - 1),
                    GetCellAt(cell.Row, cell.Col + 1),

                    GetCellAt(cell.Row + 1, cell.Col - 1),
                    GetCellAt(cell.Row + 1, cell.Col),
                    GetCellAt(cell.Row + 1, cell.Col + 1),
                };
            if (cell.Row > 0 && cell.Col == gameSize - 1)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col - 1),
                    GetCellAt(cell.Row - 1, cell.Col),

                    GetCellAt(cell.Row, cell.Col - 1),

                    GetCellAt(cell.Row + 1, cell.Col - 1),
                    GetCellAt(cell.Row + 1, cell.Col),
                };
            if (cell.Row == gameSize - 1 && cell.Col > 0)
                return new[]
                {
                    GetCellAt(cell.Row - 1, cell.Col - 1),
                    GetCellAt(cell.Row - 1, cell.Col),
                    GetCellAt(cell.Row - 1, cell.Col + 1),

                    GetCellAt(cell.Row, cell.Col - 1),
                    GetCellAt(cell.Row, cell.Col + 1),
                };

            return null;
        }

        public Cell[] GetCellsToReveal(Cell cell)
        {
            if (hasNearbyBomb(cell))
                return new[] { cell };

            var cellsAlreadyChecked = new HashSet<Cell> { cell };
            var cellsToCheck = new Queue<Cell>();
            addCellsToCheck(GetAdjacentCells(cell));
            var cellsToReveal = new HashSet<Cell> { cell };
            while (cellsToCheck.Count > 0)
            {
                var c = cellsToCheck.Dequeue();
                cellsAlreadyChecked.Add(c);
                if (hasNearbyBomb(c) == false)
                {
                    cellsToReveal.Add(c);
                    addCellsToCheck(GetAdjacentCells(c));
                }
            }

            return cellsToReveal.ToArray();

            void addCellsToCheck(Cell[] _cells)
            {
                foreach (var c in _cells)
                    if (cellsAlreadyChecked.Contains(c) == false)
                        cellsToCheck.Enqueue(c);
            }
            bool hasNearbyBomb(Cell _c) =>
                GetAdjacentCells(_c).Where(c => c.HasBomb).Count() > 0;
        }

        public RoutedEventHandler AddHandler(int row, int col)
        {
            return (o, e) =>
            {
                var cellsToReveal = GetCellsToReveal(GetCellAt(row, col));

                foreach(var c in cellsToReveal)
                    c.IsRevealed = true;
            };
        }
    }
}
