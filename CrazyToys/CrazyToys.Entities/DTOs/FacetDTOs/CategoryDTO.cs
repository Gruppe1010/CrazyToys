using System.Collections.Generic;

namespace CrazyToys.Entities.DTOs.FacetDTOs
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
