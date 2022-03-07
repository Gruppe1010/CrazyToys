using CrazyToys.Entities.Entities;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.SolrModels
{
    public class SolrPriceGroup
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }
        [SolrField("interval")]
        public string Interval { get; set; }

        public SolrPriceGroup()
        {
        }

        public SolrPriceGroup(PriceGroup priceGroup)
        {
            Id = priceGroup.ID;
            Interval = priceGroup.Interval;
        }
    }
}
