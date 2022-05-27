using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class Order
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }
        public Customer Customer { get; set; }
        public IList<Status> Statuses { get; set; }
        public Address ShippingAddress { get; set; }
        public IList<OrderLine> OrderLines { get; set; }

        public Order()
        {
            Statuses = new List<Status>();
        }

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

        public OrderConfirmationDTO ConvertToOrderConfirmationDTO(List<ShoppingCartToyDTO> shoppingCartToyDTOs)
        {
            if(Statuses.Count > 0)
            {
                AddressDTO billingAddress = Customer.BillingAddress.ConvertToAddressDTO();
                AddressDTO shippingAddress = ShippingAddress.ConvertToAddressDTO();

                string date = Statuses[0].TimeStamp.ToString("dddd, dd MMMM yyyy", new CultureInfo("da-DK"));
                date = char.ToUpper(date[0]) + date.Substring(1);

                return new OrderConfirmationDTO(OrderNumber, Customer.FirstName, Customer.LastName, Customer.Email, Statuses[0].StatusType.Name, date, billingAddress, shippingAddress, shoppingCartToyDTOs);
            }
            return null;
        }

     

     
    }
}
