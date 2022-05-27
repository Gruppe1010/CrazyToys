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
        public string CreatePaymentLink(string orderId, string currency)
        {

            string url = null;



            return url;





            string apiUserKey = "1472e7f97c46fcd7d4e9d450ab7f650bc63836d2a8de04bd5ee27e61376b7830";
            string privateKey = "4cb0257e7b52b9b07405a07ba9620285220e8f1838f83bb0e6ec5e86be1ba3d7";
            string userKey = "0aebd9aa01ce5b73ff3216a22c131da6d8987d9d4446e339ea3052516abc29ae";

            var quickPayClient = new QuickPay.SDK.QuickPayClient(apiUserKey, privateKey, userKey);
            var payment = await quickPayClient.Payments.Create("DKK", "order id", new Dictionary<string, string> { { "Hej", "12345" } }).ConfigureAwait(false);

            var link = await quickPayClient.Payments.CreateOrUpdatePaymentLink(payment.Id, 100).ConfigureAwait(false);

            var capture = await quickPayClient.Payments.Capture(payment.Id, 100).ConfigureAwait(false);

            return Redirect(link.Url);
        }
    }
}
