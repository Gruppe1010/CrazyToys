using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.Entities
{
    public class SimpleToy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string SupplierId { get; set; }
        public string ProductId { get; set; }
        public string OnMarket { get; set; }
        public string IcecatId { get; set; }
        public string DateString { get; set; }

        public SimpleToy(string iD, string supplierId, string productId, string onMarket, string icecatId, string dateString)
        {
            ID = iD;
            SupplierId = supplierId;
            ProductId = productId;
            OnMarket = onMarket;
            IcecatId = icecatId;
            DateString = dateString;
        }

        public SimpleToy(string supplierId, string productId, string onMarket, string icecatId, string dateString)
        {
            SupplierId = supplierId;
            ProductId = productId;
            OnMarket = onMarket;
            IcecatId = icecatId;
            DateString = dateString;
        }
    }
}
