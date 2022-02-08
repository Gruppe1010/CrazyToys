using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Models.Entities
{
    public class Colour
    {
        [Column("ColourId")]
        public string ID { get; set; }
        public string Name { get; set; }
    }
}