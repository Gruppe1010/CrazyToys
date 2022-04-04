using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class CategoryDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<SubCategoryDTO> SubCategoryDTOs { get; set; }
        public int FoundAmount { get; set; }

        public CategoryDTO(string iD, string name, int foundAmount)
        {
            ID = iD;
            Name = name;
            FoundAmount = foundAmount;
            SubCategoryDTOs = new List<SubCategoryDTO>();
        }

     

    }
}
