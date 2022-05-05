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
        public OrderedToy OrderedToy { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalDiscount { get; set; }

        public OrderLine(string iD, OrderedToy orderedToy, int quantity, double unitPrice, double totalDiscount)
        {
            ID = iD;
            OrderedToy = orderedToy;
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
