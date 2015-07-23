using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PayGov.Message;

namespace PayGov.Tests
{
    [TestFixture]
    public class PayGovSingleServiceTests
    {
        private readonly string _url;
        private readonly string _agencyId;
        private readonly string _applicationId;
        private readonly X509Certificate2 _cert;

        public PayGovSingleServiceTests()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _agencyId = appSettings["AgencyId"];
            _applicationId = appSettings["ApplicationId"];
            _url = appSettings["TCSSingleServiceUrl"];

            var certData = File.ReadAllBytes(appSettings["CertificatePath"]);
            _cert = new X509Certificate2(certData, appSettings["CertificatePassword"]);
        }

        [Test]
        public async Task ProcessPlasticCardSale_2002_SuccessulSubmissionOfSale()
        {
            var soap = new PlasticCardSaleRequest()
            {
                AgencyId = _agencyId,
                ApplicationId = _applicationId,
                Request = new PlasticCardSale()
                {
                    AgencyTrackingId = RandomString(),
                    OrderId = "85869603",

                    TransactionAmount = 20.00m,
                    AccountNumber = "5555555555555557",
                    CreditCardExpirationDate = "2020-10",
                    FirstName = "Leonid",
                    MiddleInitial = "",
                    LastName = "Gaiazov",
                    CardSecurityCode = "111",
                    BillingAddress = "123 Test Street",
                    BillingAddress2 = "Apt 10",
                    BillingCity = "City",
                    BillingState = "VA",
                    BillingZip = "20001",
                    BillingCountry = "840",
                    AccountHolderEmailAddress = "gaiazov@gmail.com",
                    CustomFields = new PayGovCustomFields()
                    {
                        CustomField1 = "Hello World!"
                    }
                }
            };

            var processor = new PayGovSingleService(_url, _cert);
            var response = await processor.ProcessPlasticCardSale(soap);

            response.Should().NotBeNull();
            response.Response.Should().NotBeNull();

            var saleResponse = response.Response;

            saleResponse.ReturnCode.Should().NotBeNullOrEmpty().And.Be("2002");
            saleResponse.ReturnDetail.Should()
                .NotBeNullOrEmpty()
                .And.Contain("Successful submission").And.Contain("PC Sale");
            saleResponse.PayGovTrackingId.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task ProcessPlasticCardSale_4051_AgencyTrackingId_NotUnique()
        {
            var soap = new PlasticCardSaleRequest()
            {
                AgencyId = _agencyId,
                ApplicationId = _applicationId,
                Request = new PlasticCardSale()
                {
                    AgencyTrackingId = "12345678",
                    OrderId = "85869603",

                    TransactionAmount = 20.00m,
                    AccountNumber = "5555555555555557",
                    CreditCardExpirationDate = "2020-10",
                    FirstName = "Leonid",
                    MiddleInitial = "",
                    LastName = "Gaiazov",
                    CardSecurityCode = "111",
                    BillingAddress = "123 Test Street",
                    BillingAddress2 = "Apt 10",
                    BillingCity = "City",
                    BillingState = "VA",
                    BillingZip = "20001",
                    BillingCountry = "840",
                    AccountHolderEmailAddress = "gaiazov@gmail.com",
                    CustomFields = new PayGovCustomFields()
                    {
                        CustomField1 = "Hello World!"
                    }
                }
            };

            var processor = new PayGovSingleService(_url, _cert);
            var response = await processor.ProcessPlasticCardSale(soap);

            response.Should().NotBeNull();
            response.Response.Should().NotBeNull();

            var saleResponse = response.Response;

            saleResponse.ReturnCode.Should().NotBeNullOrEmpty().And.Be("4051");
            saleResponse.ReturnDetail.Should()
                .NotBeNullOrEmpty()
                .And.Contain("agency_tracking_id").And.Contain("is not unique");
            saleResponse.PayGovTrackingId.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task ProcessPlasticCardSale_4019_NoAgencyExists()
        {
            var soap = new PlasticCardSaleRequest()
            {
                AgencyId = "1234",
                ApplicationId = "DOESNOTEXIST",
                Request = new PlasticCardSale()
                {
                    AgencyTrackingId = "12345678",
                    OrderId = "85869603",

                    TransactionAmount = 20.00m,
                    AccountNumber = "5555555555555557",
                    CreditCardExpirationDate = "2020-10",
                    FirstName = "Leonid",
                    MiddleInitial = "",
                    LastName = "Gaiazov",
                    CardSecurityCode = "111",
                    BillingAddress = "123 Test Street",
                    BillingAddress2 = "Apt 10",
                    BillingCity = "City",
                    BillingState = "VA",
                    BillingZip = "20001",
                    BillingCountry = "840",
                    AccountHolderEmailAddress = "gaiazov@gmail.com",
                    CustomFields = new PayGovCustomFields()
                    {
                        CustomField1 = "Hello World!"
                    }
                }
            };

            var processor = new PayGovSingleService(_url, _cert);
            var response = await processor.ProcessPlasticCardSale(soap);

            response.Should().NotBeNull();
            response.Response.Should().NotBeNull();

            var saleResponse = response.Response;

            saleResponse.ReturnCode.Should().NotBeNullOrEmpty().And.Be("4019");
            saleResponse.ReturnDetail.Should().NotBeNullOrEmpty().And.Contain("No agency application found");
            saleResponse.PayGovTrackingId.Should().BeNullOrEmpty();
        }

        private static string RandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}