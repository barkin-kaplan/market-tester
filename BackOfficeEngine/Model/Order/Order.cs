﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Data.SQLite;

using FixHelper;

using BackOfficeEngine.Helper;
using BackOfficeEngine.MessageEnums;
using BackOfficeEngine.ParamPacker;
using BackOfficeEngine.DB.SQLite;

namespace BackOfficeEngine.Model
{
    public class Order : BaseOrder,IDataBaseWritable
    {

        public static ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
       


        protected decimal cumulativeQty = 0;
        public decimal CumulativeQty
        {
            get { return cumulativeQty; }
            private set { cumulativeQty = value; NotifyPropertyChanged(nameof(CumulativeQty)); }
        }
        protected decimal lastQty = 0;
        public decimal LastQty
        {
            get { return lastQty; }
            private set { lastQty = value; NotifyPropertyChanged(nameof(LastQty)); }
        }
        protected decimal lastPx = 0;
        public decimal LastPx
        {
            get { return lastPx; }
            private set { lastPx = value; NotifyPropertyChanged(nameof(LastPx)); }
        }
        protected decimal avgPx = 0;
        public decimal AvgPx
        {
            get { return avgPx; }
            private set { avgPx = value; NotifyPropertyChanged(nameof(AvgPx)); }
        }
        protected string date = Util.GetNowString();
        public string Date
        {
            get { return date; }
        }
        protected OrdStatus ordStatus = OrdStatus.InitialPending;
        public OrdStatus OrdStatus
        {
            get { return ordStatus; }
            private set { ordStatus = value; NotifyPropertyChanged(nameof(OrdStatus)); }
        }
        public ObservableCollection<IMessage> m_messages { get; set; } = new ObservableCollection<IMessage>();
        

        public string TableName 
        {
            get
            {
                return "Orders";
            }
        }

        public Dictionary<string,TableField> Fields { get; } = new Dictionary<string,TableField> {
            {nameof(DatabaseID), new TableField(nameof(DatabaseID),typeof(string),"PRIMARY KEY",18) },
            {nameof(nonProtocolID), new TableField(nameof(nonProtocolID),typeof(string),"",18) },
            {nameof(price), new TableField(nameof(price),typeof(string),"",30) },
            {nameof(orderQty), new TableField(nameof(orderQty),typeof(string),"",30) },
            {nameof(account), new TableField(nameof(account),typeof(string),"",30) },
            {nameof(symbol), new TableField(nameof(symbol),typeof(string),"",25) },
            {nameof(clOrdID), new TableField(nameof(clOrdID),typeof(string),"",30)},
            {nameof(origClOrdID), new TableField(nameof(origClOrdID),typeof(string),"",30) },
            {nameof(side), new TableField(nameof(side),typeof(string),"",10) },
            {nameof(ordType), new TableField(nameof(ordType),typeof(string),"",10) },
            {nameof(timeInForce), new TableField(nameof(timeInForce),typeof(string),"",10) },
            {nameof(protocolType), new TableField(nameof(protocolType),typeof(string),"",30) },
            {nameof(cumulativeQty), new TableField(nameof(cumulativeQty),typeof(string),"",30) },
            {nameof(lastPx), new TableField(nameof(lastPx),typeof(string),"",30) },
            {nameof(lastQty), new TableField(nameof(lastQty),typeof(string),"",30) },
            {nameof(avgPx), new TableField(nameof(avgPx),typeof(string),"",30) },
            {nameof(ordStatus), new TableField(nameof(ordStatus),typeof(string),"",30) },
            {nameof(date), new TableField(nameof(date),typeof(string),"",8) }
                };

        public Dictionary<string, object> Values
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {nameof(DatabaseID), DatabaseID },
                    {nameof(nonProtocolID),nonProtocolID },
                    {nameof(price),price },
                    {nameof(orderQty),orderQty },
                    {nameof(account),account },
                    {nameof(symbol),symbol },
                    {nameof(clOrdID),clOrdID },
                    {nameof(origClOrdID),origClOrdID },
                    {nameof(side),side },
                    {nameof(ordType),ordType },
                    {nameof(timeInForce),timeInForce },
                    {nameof(protocolType),protocolType },
                    {nameof(cumulativeQty),cumulativeQty },
                    {nameof(lastPx),lastPx },
                    {nameof(lastQty),lastQty },
                    {nameof(avgPx),avgPx },
                    {nameof(ordStatus),ordStatus },
                    {nameof(date),date }

                };
            }
        }

        private void ConstructorCommonWork()
        {
            Orders.Add(this);
        }
        internal Order(SQLiteDataReader reader)
        {
            nonProtocolID = reader[nameof(nonProtocolID)].ToString();
            price = decimal.Parse(reader[nameof(price)].ToString(),CultureInfo.CurrentCulture);
            orderQty = decimal.Parse(reader[nameof(orderQty)].ToString(),CultureInfo.CurrentCulture);
            account = Account.GetInstance(reader[nameof(account)].ToString());
            symbol = reader[nameof(symbol)].ToString();
            clOrdID = reader[nameof(clOrdID)].ToString();
            origClOrdID = reader[nameof(origClOrdID)].ToString();
            side = new StringToEnumConverter<Side>().Convert(reader[nameof(side)].ToString());
            ordType = new StringToEnumConverter<OrdType>().Convert(reader[nameof(ordType)].ToString());
            timeInForce = new StringToEnumConverter<TimeInForce>().Convert(reader[nameof(timeInForce)].ToString());
            protocolType = new StringToEnumConverter<ProtocolType>().Convert(reader[nameof(protocolType)].ToString());
            cumulativeQty = decimal.Parse(reader[nameof(cumulativeQty)].ToString(),CultureInfo.CurrentCulture);
            lastPx = decimal.Parse(reader[nameof(lastPx)].ToString(),CultureInfo.CurrentCulture);
            lastQty = decimal.Parse(reader[nameof(lastQty)].ToString(), CultureInfo.CurrentCulture);
            avgPx = decimal.Parse(reader[nameof(avgPx)].ToString(),CultureInfo.CurrentCulture);
            ordStatus = new StringToEnumConverter<OrdStatus>().Convert(reader[nameof(ordStatus)].ToString());
            date = reader[nameof(date)].ToString();
            ConstructorCommonWork();
        }

        internal Order(IMessage newOrderMessage, string nonProtocolID) : base(newOrderMessage, nonProtocolID)
        {
            ConstructorCommonWork();
        }

        /// <summary>
        /// dummy constructor for database writes should not be considered for application logic
        /// </summary>
        internal Order() { }

        protected Order(BaseOrder other) : base(other)
        {
            using (SQLiteHandler handler = new SQLiteHandler())
            {
                handler.Insert(this);
            }
            ConstructorCommonWork();
        }
        internal static new(IMessage, Order) CreateNewOrder(NewMessageParameters prms, string nonProtocolPseudoID)
        {
            IMessage newOrderMessage;
            BaseOrder baseOrder;
            (newOrderMessage, baseOrder) = BaseOrder.CreateNewOrder(prms, nonProtocolPseudoID);
            Order order = new Order(baseOrder);
            order.m_messages.Add(newOrderMessage);
            return (newOrderMessage, order);
        }
        

        public string DatabaseID
        {
            get
            {
                return nonProtocolID;
            }
        }

        public string DatabaseIDColumnName
        {
            get { return nameof(nonProtocolID); }
        }        

        internal void AddMessage(IMessage msg)
        {
            m_messages.Add(msg);
            switch (msg.GetMsgType())
            {
                case MsgType.PendingNew:
                    OrdStatus = OrdStatus.PendingNew;
                    break;
                case MsgType.PendingReplace:
                    OrdStatus = OrdStatus.PendingReplace;
                    break;
                case MsgType.PendingCancel:
                    OrdStatus = OrdStatus.PendingCancel;
                    break;
                case MsgType.AckNew:
                    OrdStatus = OrdStatus.New;
                    ClOrdID = msg.GetClOrdID();
                    break;
                case MsgType.AckReplace:
                    switch (cumulativeQty)
                    {
                        case 0:
                            ordStatus = OrdStatus.New;
                            break;
                        default:
                            ordStatus = OrdStatus.PartialFilled;
                            break;
                    }
                    OrderQty = msg.GetOrderQty();
                    Price = msg.GetPrice();
                    OrigClOrdID = msg.GetOrigClOrdID();
                    ClOrdID = msg.GetClOrdID();
                    break;
                case MsgType.AckCancel:
                    OrdStatus = OrdStatus.Canceled;
                    ClOrdID = msg.GetClOrdID();
                    break;
                case MsgType.New:
                    break;
                case MsgType.Replace:
                    break;
                case MsgType.Cancel:
                    break;
                case MsgType.Reject:
                    IMessage requestMsg = FindRequestOfReject(msg);
                    switch (requestMsg.GetMsgType())
                    {
                        case MsgType.New:
                            OrdStatus = OrdStatus.Rejected;
                            break;
                    }
                    break;
                case MsgType.Trade:
                    decimal lastShares = msg.GetLastQty();
                    decimal lastPx = msg.GetLastPx();
                    if (msg.IsSetLastPx())
                        LastPx = msg.GetLastPx();
                    if (msg.IsSetLastQty())
                        LastQty = msg.GetLastQty();
                    if (msg.IsSetOrdStatus())
                        OrdStatus = msg.GetOrdStatus();
                    if (msg.IsSetGenericField(QuickFix.Fields.Tags.CumQty))
                        CumulativeQty = decimal.Parse(msg.GetGenericField(QuickFix.Fields.Tags.CumQty),CultureInfo.InvariantCulture);
                    if (msg.IsSetAvgPx())
                        AvgPx = msg.GetAvgPx();
                    Account.AddTrade(new TradeParameters(side, lastShares, lastPx, symbol));
                    break;
            }
            using(SQLiteHandler handler = new SQLiteHandler())
            {
                handler.Update(this);
            }
        }

        private IMessage FindRequestOfReject(IMessage rejectMsg)
        {
            string rejectClOrdID = rejectMsg.GetClOrdID();
            return m_messages.First((o) => o.GetClOrdID() == rejectClOrdID);
        }

        

    }



}