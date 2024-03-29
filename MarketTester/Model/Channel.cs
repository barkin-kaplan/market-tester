﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BackOfficeEngine;
using BackOfficeEngine.ParamPacker;

using MarketTester.Base;
using MarketTester.Helper;
namespace MarketTester.Model
{
    public class Channel :BaseNotifier,IEquatable<Channel>
    {
        public BISTCredentialParams credentialParams { get; set; }
        public string ConfigFilePath { get; set; }
        public ProtocolType ProtocolType { get; set; }
        public string Name { get; private set; }
        public string ConnectorName { get; set; } = "";
        public bool IsConfigured { get; set; }
        private bool isConnectingDisconnecting;

        public bool IsConnectingDisconnecting
        {
            get { return isConnectingDisconnecting; }
            set
            {
                isConnectingDisconnecting = value;
                NotifyPropertyChanged(nameof(IsConnectingDisconnecting));
            }
        }

        private bool isConnected;
        public bool IsConnected 
        { 
            get 
            { 
                return isConnected; 
            } 
            set 
            { 
                if(value != isConnected)
                {
                    isConnected = value; NotifyPropertyChanged(nameof(IsConnected));
                }
                
            } 
        }
        private object activeSessionsLock = new object();
        public ObservableCollection<string> ActiveSessions { get; set; } = new ObservableCollection<string>();
        private object inactiveSessionsLock = new object();
        public ObservableCollection<string> InactiveSessions { get; set; } = new ObservableCollection<string>();
        public Channel (string configFilePath,ProtocolType protocolType)
        {
            Name = MarketTesterUtil.GetFileNameWithoutExtensionFromFullPath(configFilePath);
            ConfigFilePath = configFilePath;
            ProtocolType = protocolType;

            BindingOperations.EnableCollectionSynchronization(ActiveSessions, activeSessionsLock);
            BindingOperations.EnableCollectionSynchronization(InactiveSessions, inactiveSessionsLock);
            Settings.GetInstance().LanguageChangedEventHandler += OnLanguageChange;
        }

        private void OnLanguageChange()
        {
            NotifyPropertyChanged(nameof(IsConnected));
        }

        public void AddActive(string sessionID)
        {
            lock (activeSessionsLock)
            {
                if (!ActiveSessions.Contains(sessionID))
                    ActiveSessions.Add(sessionID);
            }
        }

        public void RemoveActive(string sessionID)
        {
            lock (activeSessionsLock)
            {
                if (ActiveSessions.Contains(sessionID))
                    ActiveSessions.Remove(sessionID);
            }
        }

        public void AddInactive(string sessionID)
        {
            lock (activeSessionsLock)
            {
                if(!InactiveSessions.Contains(sessionID))
                InactiveSessions.Add(sessionID);
            }
        }

        public void RemoveInactive(string sessionID)
        {
            lock (activeSessionsLock)
            {
                if (InactiveSessions.Contains(sessionID))
                    InactiveSessions.Remove(sessionID);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool Equals(Channel other)
        {
            return this.Name == other.Name;
        }
    }
}
