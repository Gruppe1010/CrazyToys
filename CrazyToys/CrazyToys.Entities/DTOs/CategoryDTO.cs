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
        public List<SubCategoryDTO> subCategoryDTOs { get; set; }
        public int FoundAmount { get; set; }

        public CategoryDTO(string iD, string name, List<SubCategoryDTO> subCategoryDTOs, int foundAmount)
        {
            ID = iD;
            Name = name;
            this.subCategoryDTOs = subCategoryDTOs;
            FoundAmount = foundAmount;
        }
    }
}
