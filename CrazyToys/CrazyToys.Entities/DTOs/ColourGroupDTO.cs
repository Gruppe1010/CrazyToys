namespace CrazyToys.Entities.DTOs
{
    public class ColourGroupDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ColourCode { get; set; }

        public ColourGroupDTO(string id, string name, string colourCode)
        {
            ID = id;
            Name = name;
            ColourCode = colourCode;
        }
    }
}
