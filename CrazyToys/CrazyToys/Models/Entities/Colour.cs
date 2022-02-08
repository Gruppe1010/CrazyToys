using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class Colour
    {
        [Column("IdColour")]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}