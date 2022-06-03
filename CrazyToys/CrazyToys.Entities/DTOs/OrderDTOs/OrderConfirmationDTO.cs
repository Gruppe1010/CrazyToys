using System.Collections.Generic;

namespace CrazyToys.Entities.DTOs.OrderDTOs
{
    public class OrderConfirmationDTO
    {
        public int OrderNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        // TODO også en payment-method
        public AddressDTO BillingAddress { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public List<ShoppingCartToyDTO> ShoppingCartToyDTOs { get; set; }



        public OrderConfirmationDTO(int orderNumber, string firstName, string lastName, string email, string status, string date, AddressDTO billingAddress, AddressDTO shippingAddress, List<ShoppingCartToyDTO> shoppingCartToyDTOs)
        {
            OrderNumber = orderNumber;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Status = status;
            Date = date;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            ShoppingCartToyDTOs = shoppingCartToyDTOs;
        }
    }
}
