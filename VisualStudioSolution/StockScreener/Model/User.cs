﻿using System.Collections.Generic;
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementaion of a user
    /// </summary>
    class User : Notifyable, IUser
    {
        /// <summary>
        /// Constructor passing in the name
        /// </summary>
        /// <param name="name"></param>
        public User(string name)
        {
            _name = name;
            Settings = new Settings();
        }
        
        /// <summary>
        /// Empty constructor for deserializing
        /// </summary>
        public User() { }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public List<string> WatchedStocks
        {
            get
            {
                //todo
                return null;
            }
            set
            {
                //todo
            }
        }

        private ISettings _settings;
        public ISettings Settings
        {
            get
            {
                return _settings;
            }
            private set
            {
                _settings = value;
                RaisePropertyChanged();
            }
        }
    }
}
