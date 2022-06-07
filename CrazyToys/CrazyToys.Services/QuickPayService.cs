using CrazyToys.Interfaces;
using QuickPay.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class QuickPayService : IPaymentService
    {
        private QuickPayClient CreateQuickPayClient()
        {
            string apiUserKey = "1472e7f97c46fcd7d4e9d450ab7f650bc63836d2a8de04bd5ee27e61376b7830";
            string privateKey = "4cb0257e7b52b9b07405a07ba9620285220e8f1838f83bb0e6ec5e86be1ba3d7";
            string userKey = "0aebd9aa01ce5b73ff3216a22c131da6d8987d9d4446e339ea3052516abc29ae";

            return new QuickPayClient(apiUserKey, privateKey, userKey);
        }

        public async Task<int> CreatePayment(string orderId, string currency)
        {
            int newPaymentId = 0;

            // hvis ordreId'et har en gyldig længde (fra 4-20)
            if (orderId.Length > 3 && orderId.Length < 21)
            {
                var quickPayClient = CreateQuickPayClient();
                var payment = await quickPayClient.Payments.Create(currency, orderId, null).ConfigureAwait(false);

                newPaymentId = payment.Id;
            }

            return newPaymentId;
        }

        public async Task<string> CreatePaymentLink(int orderNumber, int paymentId, double totalPrice)
        {
            int totalPriceInInt = (int)(totalPrice * 100);

            string url = null;
         
            string urlPath = Environment.GetEnvironmentVariable("UrlPath");
            string continueUrl = $"{urlPath}/order-confirmation?orderNumber={orderNumber}";
            string cancelUrl = $"{urlPath}/shopping-cart";
            string callbackUrl = $"{urlPath}/CALLBACK"; // TODO ret

            var quickPayClient = CreateQuickPayClient();

            // constructor for paymentLink: int paymentId, int amount, bool autoCapture, bool autoFee, string language, string paymentMethods, string continueUrl, string cancelUrl, string callbackUrl, bool framed
            var link = await quickPayClient.Payments.CreateOrUpdatePaymentLink(paymentId, totalPriceInInt, false, false, "da", null, continueUrl, cancelUrl, callbackUrl, false).ConfigureAwait(false);

            url = link.Url;

            return url;
        }

        public async Task<bool> CapturePayment(int paymentId, double totalPrice)
        {
            int totalPriceInInt = (int)(totalPrice * 100);

            var quickPayClient = CreateQuickPayClient();

            var capture = await quickPayClient.Payments.Capture(paymentId, totalPriceInInt).ConfigureAwait(false);

            return capture.Accepted;
        }
    }
}
