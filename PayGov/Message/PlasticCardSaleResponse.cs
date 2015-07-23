using System.Xml.Serialization;

namespace PayGov.Message
{
    [XmlType("PCSaleResponse")]
    public class PlasticCardSaleResponse
    {
        [XmlElement("agency_id")]
        public string AgencyId;

        [XmlElement("tcs_app_id")]
        public string ApplicationId;

        [XmlElement("PCSale")]
        public PlasticCardSale Response;
    }
}
