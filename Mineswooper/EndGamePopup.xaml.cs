﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for EndGamePopup.xaml
    /// </summary>
    public partial class EndGamePopup : Popup
    {
        public EndGamePopup()
        {
            InitializeComponent();
            grid.Background = new SolidColorBrush(Colors.Green) { Opacity = 0.25 };
        }
    }
}
