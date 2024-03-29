﻿using BackOfficeEngine.Model;
using MarketTester.ViewModel;
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

namespace MarketTester.UI.Usercontrol
{
    /// <summary>
    /// Interaction logic for UserControlOrderHistoryFix.xaml
    /// </summary>
    public partial class UserControlOrderHistoryFix : UserControl
    {
        public UserControlOrderHistoryFix(Order order)
        {
            InitializeComponent();
            ((ViewModelOrderHistoryFix)this.DataContext).Order = order;
        }
    }
}
