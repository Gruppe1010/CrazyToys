namespace CrazyToys.Entities.DTOs.FacetDTOs
{
    public class ColourGroupDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ColourCode { get; set; }
        public int FoundAmount { get; set; }

        public ColourGroupDTO(string id, string name, string colourCode, int foundAmount)
        {
            ID = id;
            Name = name;
            ColourCode = colourCode;
            FoundAmount = foundAmount;

        }
    }
}
