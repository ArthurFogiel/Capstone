
using StockScreener.Interfaces;
using System.Xml;
using System.Xml.Schema;

namespace StockScreener.Model
{
    public class Settings : Notifyable, ISettings
    {
        //Default value is 5
        private float _priceMin = 5;
        public float PriceMin
        {
            get
            {
                return _priceMin;
            }
            set
            {
                _priceMin = value;
                RaisePropertyChanged();
            }
        }
        //Default to 10
        private float _priceMax = 10;
        public float PriceMax
        {
            get
            {
                return _priceMax;
            }
            set
            {
                _priceMax = value;
                RaisePropertyChanged();
            }
        }


        public float _marketCapMin = 500;
        public float MarketCapMin
        {
            get
            {
                return _marketCapMin;
            }
            set
            {
                _marketCapMin = value;
                RaisePropertyChanged();
            }
        }

        private float _marketCapMax = 1000;
        public float MarketCapMax
        {
            get
            {
                return _marketCapMax;
            }
            set
            {
                _marketCapMax = value;
                RaisePropertyChanged();
            }
        }

        private MarketCapUnitsEnum _marketCapUnitsEnum = MarketCapUnitsEnum.Millions;
        public MarketCapUnitsEnum MarketCapUnits
        {
            get { return _marketCapUnitsEnum; }
            set
            {
                _marketCapUnitsEnum = value;
                RaisePropertyChanged();
            }
        }

        private float _volumeMin = 500;
        public float VolumeMin
        {
            get
            {
                return _volumeMin;
            }
            set
            {
                _volumeMin = value;
                RaisePropertyChanged();
            }
        }

        private float _volumeMax = 1000;
        public float VolumeMax
        {
            get
            {
                return _volumeMax;
            }
            set
            {
                _volumeMax = value;
                RaisePropertyChanged();
            }
        }

        #region Serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                // Only detect start elements.
                if (reader.IsStartElement())
                {
                    // Get element name and switch on it.
                    switch (reader.Name)
                    {
                        case "PriceMin":
                            reader.Read();
                            PriceMin = float.Parse(reader.Value.Trim());
                            break;
                        case "PriceMax":
                            reader.Read();
                            PriceMax = float.Parse(reader.Value.Trim());
                            break;
                        case "MarketCapMin":
                            reader.Read();
                            MarketCapMin = float.Parse(reader.Value.Trim());
                            break;
                        case "MarketCapMax":
                            reader.Read();
                            MarketCapMax = float.Parse(reader.Value.Trim());
                            break;
                        case "VolumeMin":
                            reader.Read();
                            VolumeMin = float.Parse(reader.Value.Trim());
                            break;
                        case "VolumeMax":
                            reader.Read();
                            VolumeMax = float.Parse(reader.Value.Trim());
                            break;
                        case "MarketCapUnits":
                            reader.Read();
                            MarketCapUnits = (MarketCapUnitsEnum)int.Parse(reader.Value.Trim());
                            break;
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {

            writer.WriteStartElement("Setting");
            writer.WriteStartElement("PriceMin");
            writer.WriteValue(PriceMin);
            writer.WriteEndElement();

            writer.WriteStartElement("PriceMax");
            writer.WriteValue(PriceMax);
            writer.WriteEndElement();
            writer.WriteStartElement("MarketCapMin");
            writer.WriteValue(MarketCapMin);
            writer.WriteEndElement();
            writer.WriteStartElement("MarketCapMax");
            writer.WriteValue(MarketCapMax);
            writer.WriteEndElement();
            writer.WriteStartElement("VolumeMin");
            writer.WriteValue(VolumeMin);
            writer.WriteEndElement();
            writer.WriteStartElement("VolumeMax");
            writer.WriteValue(VolumeMax);
            writer.WriteEndElement();
            writer.WriteStartElement("MarketCapUnits");
            writer.WriteValue((int)MarketCapUnits);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        #endregion
    }
}
