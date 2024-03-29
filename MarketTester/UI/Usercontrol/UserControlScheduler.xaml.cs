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


using BackOfficeEngine.MessageEnums;
namespace MarketTester.UI.Usercontrol
{
    /// <summary>
    /// Interaction logic for UserControlScheduler.xaml
    /// </summary>
    public partial class UserControlScheduler : UserControl
    {
        public UserControlScheduler()
        {
            InitializeComponent();
            SetComboItemSource();
        }

        private void SetComboItemSource()
        {
            List<TimeInForce> itemsTimeInForce = CastIEnumerableToList(Enum.GetValues(typeof(TimeInForce)).Cast<TimeInForce>());
            itemsTimeInForce.RemoveAt(0);
            List<OrdType> itemsOrdType = CastIEnumerableToList(Enum.GetValues(typeof(OrdType)).Cast<OrdType>());
            itemsOrdType.RemoveAt(0);
            List<Side> itemsSide = CastIEnumerableToList(Enum.GetValues(typeof(Side)).Cast<Side>());
            itemsSide.RemoveAt(0);
            List<MsgType> itemsMsgTypes = new List<MsgType>()
            {
                MsgType.New,MsgType.Replace,MsgType.Cancel
            };
            COmboBoxTimeInForce.ItemsSource = itemsTimeInForce;
            ComboBoxOrdType.ItemsSource = itemsOrdType;
            ComboBoxSide.ItemsSource = itemsSide;
            ComboBoxMsgType.ItemsSource = itemsMsgTypes;
        }

        private List<T> CastIEnumerableToList<T>(IEnumerable<T> e)
        {
            return new List<T>(e);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridTagValues.UnselectAll();
            DataGridSchedule.UnselectAll();
        }

        private void DataGridSchedule_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "SchedulerOrderID")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }
    }
}
