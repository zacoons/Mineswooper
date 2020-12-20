using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mineswooper
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : UserControl
    {
        public static readonly DependencyProperty GameSizeProperty =
            DependencyProperty.Register(
                nameof(GameSize), typeof(int), typeof(GameMenu),
                new PropertyMetadata(GameManager.DEFAULT_GAMESIZE));

        public static readonly RoutedEvent PlayEvent = EventManager.RegisterRoutedEvent(
            nameof(Play), RoutingStrategy.Bubble, typeof(EventHandler), typeof(GameMenu));

        public int GameSize
        {
            get => (int)GetValue(GameSizeProperty);
            set => SetValue(GameSizeProperty, value);
        }

        public event EventHandler Play
        {
            add => AddHandler(PlayEvent, value);
            remove => RemoveHandler(PlayEvent, value);
        }

        public GameMenu()
        {
            InitializeComponent();
            gameSizes.ItemsSource = new[]
            {
                8,9,10,11,12,13,14,15,16,
            };

            gameSizes.SetBinding(
                ComboBox.SelectedItemProperty,
                new Binding(GameSizeProperty.Name)
                {
                    Source = this,
                    Mode = BindingMode.TwoWay,
                });

            playButton.Click += (o, e) => RaiseEvent(new RoutedEventArgs(PlayEvent));
        }
    }
}
