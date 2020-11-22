using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mineswooper;

namespace MineswooperTests
{
    [TestClass]
    public class GetAdjacentCellsTests
    {
        [TestMethod]
        public void GetAdjacentCells()
        {
            var GM = new GameManager();
            GM.Start(4, false);

            var cells = GM.GetAdjacentCells(new Cell(0, 0));
            Assert.AreEqual(cells.Length, 3);
            Assert.AreEqual(cells[0], new Cell(0, 1));
            Assert.AreEqual(cells[1], new Cell(1, 0));
            Assert.AreEqual(cells[2], new Cell(1, 1));

            cells = GM.GetAdjacentCells(new Cell(0, 3));
            Assert.AreEqual(cells.Length, 3);
            Assert.AreEqual(cells[0], new Cell(0, 2));
            Assert.AreEqual(cells[1], new Cell(1, 2));
            Assert.AreEqual(cells[2], new Cell(1, 3));

            cells = GM.GetAdjacentCells(new Cell(3, 3));
            Assert.AreEqual(cells.Length, 3);
            Assert.AreEqual(cells[0], new Cell(2, 2));
            Assert.AreEqual(cells[1], new Cell(2, 3));
            Assert.AreEqual(cells[2], new Cell(3, 2));

            cells = GM.GetAdjacentCells(new Cell(3, 0));
            Assert.AreEqual(cells.Length, 3);
            Assert.AreEqual(cells[0], new Cell(2, 0));
            Assert.AreEqual(cells[1], new Cell(2, 1));
            Assert.AreEqual(cells[2], new Cell(3, 1));
        }
    }
}
