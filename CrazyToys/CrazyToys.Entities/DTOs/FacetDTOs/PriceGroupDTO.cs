
namespace CrazyToys.Entities.DTOs.FacetDTOs
{
    public class PriceGroupDTO
    {
        public string ID { get; set; }
        public string Interval { get; set; }
        public int FoundAmount { get; set; }

        public PriceGroupDTO(string iD, string interval, int foundAmount)
        {
            ID = iD;
            Interval = interval;
            FoundAmount = foundAmount;
        }
    }
}
