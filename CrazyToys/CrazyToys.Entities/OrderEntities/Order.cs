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
        public IList<Status> statuses { get; set; }
        public Address ShippingAddress { get; set; }
        public IList<OrderLine> OrderLines { get; set; }

        public Order(string iD, int orderNumber, Customer customer, IList<Status> statuses, Address shippingAddress, IList<OrderLine> orderLines)
        {
            ID = iD;
            OrderNumber = orderNumber;
            Customer = customer;
            this.statuses = statuses;
            ShippingAddress = shippingAddress;
            OrderLines = orderLines;
        }
    }
}
