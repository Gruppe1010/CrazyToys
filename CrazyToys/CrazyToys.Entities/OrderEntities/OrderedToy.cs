using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class OrderedToy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string ProductId { get; set; }

        public OrderedToy()
        {
        }

        public OrderedToy(string iD, string productId)
        {
            ID = iD;
            ProductId = productId;
        }
    }
}
