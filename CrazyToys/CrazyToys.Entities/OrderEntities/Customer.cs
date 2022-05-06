using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class Customer
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<Order> Orders { get; set; }
        public Address BillingAddress { get; set; }

        public Customer()
        {
        }

        public Customer(string email)
        {
            Email = email;
        }

        public Customer(string firstName, string lastName, string email, Address billingAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Orders = new List<Order>();
            BillingAddress = billingAddress;
        }
    }
}
