using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mineswooper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int gameSize = 5;
        GameManager gm = new GameManager();

        public MainWindow()
        {
            InitializeComponent();

            gm.Start(gameSize);

            var cellButtons = new Dictionary<Cell, Button>();
            for (int col = 0; col < gameSize; col++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int row = 0; row < gameSize; row++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int row = 0; row < gameSize; row++)
            {
                for (int col = 0; col < gameSize; col++)
                {
                    var button = new Button();
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    grid.Children.Add(button);
                    var cell = gm.GetCellAt(row, col);
                    button.Content = cell.HasBomb ? "*" : "";
                    //button.Background = Brushes.Red;

                    cellButtons[cell] = button;
                    if (!cell.HasBomb)
                    {
                        int adjacentBombs = gm.GetAdjacentCells(cell).Where(c => c.HasBomb).Count();
                        if (adjacentBombs > 0)
                        {
                            button.Content = $"{adjacentBombs}";
                            //button.Background = Brushes.LightGray;
                        }
                        //else
                            //button.Background = Brushes.White;
                    }
                    //button.Background = gm.GetCellAt(row, col).HasBomb ? Brushes.Red : null;
                    button.Click += gm.AddHandler(row, col);
                    button.Click += (o, e) =>  UpdateButtonStyles();
                }
            }

            void UpdateButtonStyles()
            {
                for (int row = 0; row < gameSize; row++)
                {
                    for (int col = 0; col < gameSize; col++)
                    {
                        var cell = gm.GetCellAt(row, col);
                        if (cell.IsRevealed)
                            cellButtons[cell].Background = Brushes.Pink;
                    }
                }
            }
        }
    }
}
