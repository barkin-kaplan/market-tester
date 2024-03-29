﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MarketTester.Enumeration;
using MarketTester.Events;

namespace MarketTester.Helper
{
    public class Settings
    {
        private static Settings instance;

        private Settings()
        {
            Language = ELanguage.English;
        }

        public static Settings GetInstance()
        {
            if(instance == null)
            {
                instance = new Settings();
                
            }
            return instance;
        }

        public event LanguageChangedEventHandler LanguageChangedEventHandler;
        #region settings properties
        private ELanguage language;
        public ELanguage Language
        {
            get
            {
                return language;
            }
            set
            {
                if(value != language)
                {
                    language = value;
                    Application.Current.Resources.MergedDictionaries.RemoveAt(0);
                    string uri = @"\Resource\";
                    switch (language)
                    {
                        case ELanguage.Turkish:
                            uri += "ResourceString_tur.xaml";
                            break;
                        case ELanguage.English:
                            uri += "ResourceString_eng.xaml";
                            break;
                    }
                    Application.Current.Resources.MergedDictionaries.Insert(0, (new ResourceDictionary { Source = new Uri(uri, UriKind.Relative) }));
                    LanguageChangedEventHandler?.Invoke();
                }
            }
        }

        private bool toolTipEnabled;

        public bool ToolTipEnabled
        {
            get { return toolTipEnabled; }
            set
            {
                toolTipEnabled = value;
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();

                foreach (Window window in Application.Current.Windows)
                {
                    if (value)
                    {
                        window.Resources.Add(typeof(ToolTip), style); //hide
                    }
                    else
                    {
                        window.Resources.Remove(typeof(ToolTip)); //show
                    }
                }

            }
        }

        #endregion

        #region helpers
        private void SetToolTipService(Style style,bool value)
        {
            style.Setters.Add(new Setter(ToolTipService.IsEnabledProperty, value));
        }
        #endregion
    }
}
