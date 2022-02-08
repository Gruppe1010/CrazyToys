namespace CrazyToys.Web.Models.DTOs
{
    public class AgeGroupDTO
    {
        public string Id { get; set; }
        public string Interval { get; set; }

        public AgeGroupDTO(string id, string interval)
        {
            Id = id;
            Interval = interval;
        }
    }
}