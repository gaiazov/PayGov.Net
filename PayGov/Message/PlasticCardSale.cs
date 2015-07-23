using System.Xml.Serialization;

namespace PayGov.Message
{
    [XmlType("PCSale")]
    public class PlasticCardSale
    {
        // <agency_tracking_id>85869603</agency_tracking_id>
        [XmlElement("agency_tracking_id")]
        public string AgencyTrackingId;

        // <transaction_amount>20.0000</transaction_amount>
        [XmlElement("transaction_amount")]
        public decimal TransactionAmount;

        // <account_number>5555555555555557</account_number>
        [XmlElement("account_number")]
        public string AccountNumber;

        // <credit_card_expiration_date>2020-10</credit_card_expiration_date>
        [XmlElement("credit_card_expiration_date")]
        public string CreditCardExpirationDate;

        // <first_name>Leonid</first_name>
        [XmlElement("first_name")]
        public string FirstName;

        // <middle_initial />
        [XmlElement("middle_initial")]
        public string MiddleInitial;

        // <last_name>Gaiazov</last_name>
        [XmlElement("last_name")]
        public string LastName;

        // <card_security_code>111</card_security_code>
        [XmlElement("card_security_code")]
        public string CardSecurityCode;

        // <billing_address>1040 Brandon Ave</billing_address>
        [XmlElement("billing_address")]
        public string BillingAddress;

        // <billing_address_2>Apt 1</billing_address_2>
        [XmlElement("billing_address_2")]
        public string BillingAddress2;

        // <billing_city>Norfolk</billing_city>
        [XmlElement("billing_city")]
        public string BillingCity;

        // <billing_state>VA</billing_state>
        [XmlElement("billing_state")]
        public string BillingState;

        // <billing_zip>23507</billing_zip>
        [XmlElement("billing_zip")]
        public string BillingZip;

        // <billing_country>840</billing_country>
        [XmlElement("billing_country")]
        public string BillingCountry;

        // <order_id>85869603</order_id>
        [XmlElement("order_id")]
        public string OrderId;

        // <custom_fields />
        [XmlElement("custom_fields")]
        public PayGovCustomFields CustomFields = new PayGovCustomFields();

        // <account_holder_email_address>gaiazov@gmail.com</account_holder_email_address>
        [XmlElement("account_holder_email_address")]
        public string AccountHolderEmailAddress;

        ////////////////////////////
        // Response fields
        ////////////////////////////

        // <tcs:masked_account_number>************5557</tcs:masked_account_number>
        [XmlElement("masked_account_number")]
        public string MaskedAccountNumber;

        // <tcs:return_code>4051</tcs:return_code>
        [XmlElement("return_code")]
        public string ReturnCode;

        // <tcs:return_detail>The value supplied for the agency_tracking_id is not unique.</tcs:return_detail>
        [XmlElement("return_detail")]
        public string ReturnDetail;

        // <tcs:transaction_status>Failed</tcs:transaction_status>
        [XmlElement("transaction_status")]
        public string TransactionStatus;

        // <tcs:transaction_date xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
        [XmlElement("transaction_date")]
        public string TransactionDate;

        // <tcs:approval_code/>
        [XmlElement("approval_code")]
        public string ApprovalCode;

        // <tcs:auth_response_code/>
        [XmlElement("auth_response_code")]
        public string AuthResponseCode;

        // <tcs:auth_response_text/>
        [XmlElement("auth_response_text")]
        public string AuthResponseText;

        // <tcs:avs_response_code/>
        [XmlElement("avs_response_code")]
        public string AvsResponseCode;

        // <tcs:csc_result/>
        [XmlElement("csc_result")]
        public string CscResult;

        // <tcs:authorized_amount xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
        [XmlElement("authorized_amount")]
        public string AuthorizedAmount;

        // <tcs:remaining_balance xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
        [XmlElement("remaining_balance")]
        public string RemainingBalance;
    }

}
