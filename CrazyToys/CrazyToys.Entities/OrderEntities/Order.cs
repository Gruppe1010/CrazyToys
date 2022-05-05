using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public int OrderNumber { get; set; }
        public Customer Customer { get; set; }
        public IList<Status> Statuses { get; set; }
        public Address ShippingAddress { get; set; }
        public IList<OrderLine> OrderLines { get; set; }

        public Order(int orderNumber, Customer customer, Address shippingAddress)
        {
            OrderNumber = orderNumber;
            Customer = customer;
            ShippingAddress = shippingAddress;
            Statuses = new List<Status>();
            OrderLines = new List<OrderLine>();
        }

        public double CalculateTotalPrice()
        {
            double totalPrice = 0;


            foreach (OrderLine orderLine in OrderLines)
            {
                totalPrice += orderLine.CalculateSubTotal();

            }



            return totalPrice;
        }
    }
}
