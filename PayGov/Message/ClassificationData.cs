using System.Xml.Serialization;

namespace PayGov.Message
{
    [XmlType("classification_data")]
    public class ClassificationData
    {
        [XmlAttribute("classification_id")]
        public string ClassificationId;

        [XmlAttribute("amount")]
        public decimal Amount;
    }
}