using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using StockScreener.Interfaces;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementaion of a user
    /// </summary>
    public class User : Notifyable, IUser
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
        public User()
        {
            Settings = new Settings();
        }
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

        private List<string> _watchedStocks = new List<string>();
        public List<string> WatchedStocks
        {
            get
            {
                return _watchedStocks;
            }
            set
            {
                _watchedStocks = value;
                RaisePropertyChanged();
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

        #region serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            WatchedStocks.Clear();
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    // Get element name and switch on it.
                    switch (reader.Name)
                    {
                        case "Name":
                            reader.Read();
                            Name = reader.Value.Trim();
                            break;
                        //watched stocks contain sub elements so read subgtree
                        case "WatchedStocks":
                            XmlReader inner = reader.ReadSubtree();
                            while (inner.Read())
                            {
                                // Only detect start elements.
                                if (inner.IsStartElement())
                                {
                                    // Get element name and switch on it.
                                    switch (inner.Name)
                                    {
                                        case "Ticker":
                                            inner.Read();
                                            WatchedStocks.Add(inner.Value.Trim());
                                            break;
                                    }
                                }
                            }
                            break;
                            //setting is a category of sub elements so read subtree and pass on
                        case "Setting":
                            XmlReader inner2 = reader.ReadSubtree();
                                ((Settings)Settings).ReadXml(inner2);
                            break;
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("User");
            writer.WriteStartElement("Name");
            writer.WriteString(Name);
            writer.WriteEndElement();
            writer.WriteStartElement("WatchedStocks");
            foreach (var watch in WatchedStocks)
            {
                writer.WriteStartElement("Ticker");
                writer.WriteString(watch);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            Settings.WriteXml(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}
