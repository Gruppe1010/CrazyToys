namespace CrazyToys.Entities.DTOs.FacetDTOs
{
    public class BrandDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FoundAmount { get; set; }

        public BrandDTO(string iD, string name, string foundAmount)
        {
            ID = iD;
            Name = name;
            FoundAmount = foundAmount;
        }
    }
}
