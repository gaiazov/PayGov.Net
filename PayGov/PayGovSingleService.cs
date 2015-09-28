using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PayGov.Message;

namespace PayGov
{
    public class PayGovSingleService
    {
        private readonly string _url;
        private readonly X509Certificate2 _cert;

        public PayGovSingleService(string url, X509Certificate2 certificate)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url), "url cannot be null");
            }

            if (!url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException($"{nameof(url)} must start with http:// or https://");
            }

            _url = url;
            _cert = certificate;
        }

        public async Task<PlasticCardSaleResponse> ProcessPlasticCardSale(PlasticCardSaleRequest plasticCardSale)
        {
            var request = CreateWebRequest(_url, _cert, "ProcessPCSale");

            var soap = new SoapWrapper(plasticCardSale);

            WriteSoapToRequest(request, soap);

            WebResponse response;

            try
            {
                response = await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                if (errorResponse == null)
                {
                    throw new ApplicationException("Error establishing connection.", ex);
                }

                var errorStream = errorResponse.GetResponseStream();
                if (errorStream == null)
                {
                    throw new ApplicationException("Error establishing connection.", ex);
                }

                using (var rd = new StreamReader(errorStream))
                {
                    var soapResult = rd.ReadToEnd();
                    Console.WriteLine(soapResult);
                }
                throw new ApplicationException("Error occured in Pay.Gov.", ex);
            }

            if (response == null)
            {
                throw new ApplicationException("Received bad response from Pay.Gov.");
            }

            var responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                throw new ApplicationException("Reponse Stream is null.");
            }

            using (var rd = new StreamReader(responseStream))
            {
                var deSerializer = new XmlSerializer(typeof (SoapWrapper));
                var soapResponse = (SoapWrapper)deSerializer.Deserialize(rd);
                if (soapResponse == null)
                {
                    throw new ApplicationException("Response is not a SoapWrapper object.");
                }
                var saleResponse = (PlasticCardSaleResponse)soapResponse.Body[0];
                response.Dispose();
                return saleResponse;
            }

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
                <soapenv:Body>
                    <tcs:PCSaleResponse xmlns:tcs="http://fms.treas.gov/tcs/schemas">
                    <tcs:agency_id>_____</tcs:agency_id>
                    <tcs:tcs_app_id>______</tcs:tcs_app_id>
                    <tcs:PCSale>
                        <tcs:paygov_tracking_id/>
                        <tcs:agency_tracking_id>12345678</tcs:agency_tracking_id>
                        <tcs:transaction_amount>20.00</tcs:transaction_amount>
                        <tcs:masked_account_number>************5557</tcs:masked_account_number>
                        <tcs:return_code>4051</tcs:return_code>
                        <tcs:return_detail>The value supplied for the agency_tracking_id is not unique.</tcs:return_detail>
                        <tcs:transaction_status>Failed</tcs:transaction_status>
                        <tcs:transaction_date xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
                        <tcs:approval_code/>
                        <tcs:auth_response_code/>
                        <tcs:auth_response_text/>
                        <tcs:avs_response_code/>
                        <tcs:csc_result/>
                        <tcs:authorized_amount xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
                        <tcs:remaining_balance xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
                    </tcs:PCSale>
                </tcs:PCSaleResponse>
                </soapenv:Body>
            </soapenv:Envelope>
            */ 
        }

        #region Private Static methods
        private static HttpWebRequest CreateWebRequest(string url, X509Certificate cert, string soapAction)
        {
            // Create the web request
            var webRequest = WebRequest.Create(url);

            var request = webRequest as HttpWebRequest;

            if (request == null)
            {
                throw new ArgumentException("web request is not a HttpWebRequest. This shouldn't happen");
            }

            request.ContentType = "text/xml; charset=utf-8";
            request.Accept = "text/xml";
            request.Method = "POST";
            request.Headers.Add("SOAPAction", "urn:" + soapAction);
            request.ClientCertificates.Add(cert);

            return request;
        }

        private static void WriteSoapToRequest<T>(WebRequest request, T soap)
        {
            using (var stream = request.GetRequestStream())
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("s11", "http://schemas.xmlsoap.org/soap/envelope/");
                var xser = new XmlSerializer(typeof(T));
                xser.Serialize(stream, soap, ns);

#if DEBUG
                xser.Serialize(Console.Out, soap, ns);
#endif
            }
        }
        #endregion
    }
}
