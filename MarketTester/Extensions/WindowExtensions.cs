﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MarketTester.Extensions
{
    public static class WindowExtensions
    {
        public static System.Windows.Forms.Screen GetScreen(this Window window)
        {
            return System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }
    }
}
