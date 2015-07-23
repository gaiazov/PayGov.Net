using System.Collections.Generic;
using System.Xml.Serialization;
using PayGov.Message;

namespace PayGov
{
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapWrapper
    {
        public SoapWrapper()
        {
            
        }

        public SoapWrapper(object message)
        {
            Body.Add(message);
        }

        [XmlArray]
        [XmlArrayItem(typeof(PlasticCardSaleRequest), Namespace = "http://fms.treas.gov/tcs/schemas")]
        [XmlArrayItem(typeof(PlasticCardSaleResponse), Namespace = "http://fms.treas.gov/tcs/schemas")]
        public List<object> Body = new List<object>();
    }
}
