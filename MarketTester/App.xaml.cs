﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MarketTester.UI.window;
using MarketTester.Helper;

namespace MarketTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
            Util.CopyDirectoryAndSubDirectoriesToApplicationCommonPath(Util.STATIC_DIR_PATH);
            new MainWindowStarter();
        }
    }
}