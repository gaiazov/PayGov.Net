using System.Xml.Serialization;

namespace PayGov.Message
{
    [XmlType("PCSaleRequest")]
    public class PlasticCardSaleRequest
    {
        [XmlElement("agency_id")]
        public string AgencyId;

        [XmlElement("tcs_app_id")]
        public string ApplicationId;

        [XmlElement("PCSale")]
        public PlasticCardSale Request;
    }
}
