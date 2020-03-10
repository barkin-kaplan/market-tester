﻿using System;
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

namespace MarketTester.UI.Usercontrol
{
    /// <summary>
    /// Interaction logic for UserControlUpperBar.xaml
    /// </summary>
    public partial class UserControlUpperBar : UserControl
    {
        public UserControlUpperBar()
        {
            InitializeComponent();
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);
            w.WindowState = WindowState.Minimized;
        }

        private void ButtonNormalToggle_Click(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);
            if (w.WindowState == WindowState.Maximized)
            {
                w.WindowState = WindowState.Normal;
            }
            else
            {
                w.WindowState = WindowState.Maximized;
            }

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);
            w.Close();

        }
    }
}