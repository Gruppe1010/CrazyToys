namespace CrazyToys.Entities.DTOs.FacetDTOs
{
    public class AgeGroupDTO
    {
        public string ID { get; set; }
        public string Interval { get; set; }
        public int FoundAmount { get; set; }

        public AgeGroupDTO(string iD, string interval, int foundAmount)
        {
            ID = iD;
            Interval = interval;
            FoundAmount = foundAmount;
        }
    }
}
