using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IPaymentService
    {

        Task<int> CreatePayment(string orderId, string currency);


        Task<string> CreatePaymentLink(int orderNumber, int paymentId, double totalPrice);

        Task<bool> CapturePayment(int paymentId, double totalPrice);


    }
}
