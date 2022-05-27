using CrazyToys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class QuickPayService : IPaymentService
    {
        public async Task<string> CreatePaymentLink(string orderId, string currency, double totalPrice)
        {
            string url = null;

            // hvis ordreId'et har en gyldig længde (fra 4-20)
            if (orderId.Length > 3 && orderId.Length < 21)
            {
                int totalPriceInInt = (int)(totalPrice * 100);

                string apiUserKey = "1472e7f97c46fcd7d4e9d450ab7f650bc63836d2a8de04bd5ee27e61376b7830";
                string privateKey = "4cb0257e7b52b9b07405a07ba9620285220e8f1838f83bb0e6ec5e86be1ba3d7";
                string userKey = "0aebd9aa01ce5b73ff3216a22c131da6d8987d9d4446e339ea3052516abc29ae";

                var quickPayClient = new QuickPay.SDK.QuickPayClient(apiUserKey, privateKey, userKey);
                var payment = await quickPayClient.Payments.Create(currency, orderId, new Dictionary<string, string> { { "Hej", "12345" } }).ConfigureAwait(false);
                
                string urlPath = Environment.GetEnvironmentVariable("UrlPath");
                string continueUrl = $"{urlPath}/order-confirmation?orderNumber={orderId}";
                string cancelUrl = $"{urlPath}/shopping-cart";
                string callbackUrl = $"{urlPath}/CALLBACK"; // TODO ret

                // constructor for paymentLink: int paymentId, int amount, bool autoCapture, bool autoFee, string language, string paymentMethods, string continueUrl, string cancelUrl, string callbackUrl, bool framed
                var link = await quickPayClient.Payments.CreateOrUpdatePaymentLink(payment.Id, totalPriceInInt, false, false, "Danish", null, continueUrl, cancelUrl, callbackUrl, false).ConfigureAwait(false);

                url = link.Url;
            }

            return url;
        }



        //metode til hæv penge
        //CaptureZeMoney()

        // TODO find lige ud af hvad vi skal bruge denne på
        //var capture = await quickPayClient.Payments.Capture(payment.Id, totalPriceInInt).ConfigureAwait(false);



    }
}
