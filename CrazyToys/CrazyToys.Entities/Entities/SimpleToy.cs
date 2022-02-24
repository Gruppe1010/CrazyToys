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
        public Brand Brand { get; set; } // nav prop
        public string BrandId { get; set; } // foreign key
        public string OnMarket { get; set; }
        public string IcecatId { get; set; }
        public string DateString { get; set; }
        public IList<Error> Errors { get; set; } // nav prop
        public bool SuccessfullyRetrievedAsJson { get; set; }

        public SimpleToy(string iD, string brandId, string onMarket, string icecatId, string dateString)
        {
            ID = iD;
            BrandId = brandId;
            OnMarket = onMarket;
            IcecatId = icecatId;
            DateString = dateString;
            Errors = new List<Error>();
            SuccessfullyRetrievedAsJson = true;
        }

        public SimpleToy(string brandId, string onMarket, string icecatId, string dateString)
        {
            BrandId = brandId;
            OnMarket = onMarket;
            IcecatId = icecatId;
            DateString = dateString;
            Errors = new List<Error>();
            SuccessfullyRetrievedAsJson = true;
        }

        public void UpdateValuesToAnotherToysValues(SimpleToy simpleToy)
        {
            BrandId = simpleToy.BrandId;
            OnMarket = simpleToy.OnMarket;
            IcecatId = simpleToy.IcecatId;
            DateString = simpleToy.DateString;
        }
    }
}
