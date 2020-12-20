using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Mineswooper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameManager gm = new GameManager();
        readonly TextBlock endGameText = new TextBlock
        {
            FontSize = 32,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        readonly SolidColorBrush endGameBrush = new SolidColorBrush(Colors.Red);

        public MainWindow()
        {
            InitializeComponent();
            grid.SetBinding(
                WidthProperty,
                new Binding(ActualWidthProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });
            grid.SetBinding(
                HeightProperty,
                new Binding(ActualHeightProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });

            endGameText.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                Opacity = 0.5,
                BlurRadius = 15,
                ShadowDepth = 3,
            };
            overlay.Background = endGameBrush;
            endGameBrush.Opacity = 0.4;
            overlay.SetBinding(
                WidthProperty,
                new Binding(ActualWidthProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });
            overlay.SetBinding(
                HeightProperty,
                new Binding(ActualHeightProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });
            gameMenu.Play += (o, e) =>
            {
                gameMenu.Visibility = Visibility.Hidden;
                Play();
            };
            gameMenu.SetBinding(
                WidthProperty,
                new Binding(ActualWidthProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });
            gameMenu.SetBinding(
                HeightProperty,
                new Binding(ActualHeightProperty.Name)
                {
                    Source = canvas,
                    Mode = BindingMode.OneWay,
                });
            overlay.Children.Add(endGameText);
            Grid.SetRow(endGameText, 1);

            grid.Background = new SolidColorBrush(Color.FromRgb(53, 53, 69));
        }

        void Play()
        {
            int gameSize = gameMenu.GameSize;
            var cellButtons = new Dictionary<Cell, Button>();
            gm.Start(gameSize);

            for (int col = 0; col < gameSize; col++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int row = 0; row < gameSize; row++)
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int row = 0; row < gameSize; row++)
            {
                for (int col = 0; col < gameSize; col++)
                {
                    var button = new RoundedButton();
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    grid.Children.Add(button);
                    var cell = gm.GetCellAt(row, col);
                    //button.Background = Brushes.White;
                    button.Background = new SolidColorBrush(Color.FromRgb(71, 71, 92));
                    button.BorderBrush = new SolidColorBrush(Color.FromRgb(53, 53, 69));
                    if (button.IsMouseOver) { button.Background = new SolidColorBrush(Color.FromRgb(44, 44, 58)); }

                    cellButtons[cell] = button;
                    button.Click += gm.AddRevealHandler(row, col);
                    button.Click += (o, e) => UpdateButtonStyles();
                    button.MouseRightButtonDown += gm.AddFlagHandler(row, col);
                    button.MouseRightButtonDown += (o, e) => UpdateButtonStyles();
                }
            }
            void UpdateButtonStyles()
            {
                for (int row = 0; row < gameSize; row++)
                {
                    for (int col = 0; col < gameSize; col++)
                    {
                        var cell = gm.GetCellAt(row, col);
                        cellButtons[cell].Background = new SolidColorBrush(Color.FromRgb(71, 71, 92));
                        cellButtons[cell].Foreground = Brushes.White;
                        cellButtons[cell].FontWeight = FontWeights.Bold;
                        if (cell.IsRevealed)
                        {
                            cellButtons[cell].Background = new SolidColorBrush(Color.FromRgb(89, 89, 115));

                            if (cell.HasBomb)
                                cellButtons[cell].Content = new Ellipse { Fill = new SolidColorBrush(Color.FromRgb(34, 34, 34)), Width = 25, Height = 25 };
                            else
                            {
                                int adjacentBombs = gm.GetAdjacentCells(cell).Where(c => c.HasBomb).Count();
                                if (adjacentBombs > 0)
                                    cellButtons[cell].Content = $"{adjacentBombs}";
                            }
                        }
                        else if (cell.IsFlagged)
                            cellButtons[cell].Background = new SolidColorBrush(Color.FromRgb(34, 34, 34));

                        if (gm.HasWon())
                        {
                            overlay.Visibility = Visibility.Visible;
                            endGameBrush.Color = Colors.Green;
                            endGameText.Text = ":D YOU WON!";
                        }
                        else if (gm.HasLost())
                        {
                            overlay.Visibility = Visibility.Visible;
                            endGameBrush.Color = Colors.Red;
                            endGameText.Text = ":/ you lost.";
                        }
                    }
                }
            }
        }
    }
}
