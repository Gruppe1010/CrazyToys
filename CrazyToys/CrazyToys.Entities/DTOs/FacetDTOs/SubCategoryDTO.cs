using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class SubCategoryDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int FoundAmount { get; set; }

        public SubCategoryDTO(string iD, string name, int foundAmount)
        {
            ID = iD;
            Name = name;
            FoundAmount = foundAmount;
        }
    }
}
