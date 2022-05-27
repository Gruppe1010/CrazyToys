using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IPaymentService
    {

        string CreatePaymentLink(string orderId, string currency);




    }
}
