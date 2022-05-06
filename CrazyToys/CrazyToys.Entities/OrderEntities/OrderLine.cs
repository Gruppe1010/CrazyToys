using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class OrderLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string OrderedToyId { get; set; } // Denne svarer til Toy ID
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalDiscount { get; set; }

        public OrderLine()
        {
        }

        public OrderLine(string orderedToyId, int quantity, double unitPrice, double totalDiscount)
        {
            OrderedToyId = orderedToyId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalDiscount = totalDiscount;
        }

        public double CalculateSubTotal()
        {
            return (UnitPrice * Quantity) - TotalDiscount;
        }
    }
}
