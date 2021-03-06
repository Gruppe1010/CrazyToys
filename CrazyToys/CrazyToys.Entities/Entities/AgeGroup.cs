using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class AgeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // Hvis man laver id-proppen om til int generer den fra 1 og op af
        public string ID { get; set; }
        public string Interval { get; set; }
        public ICollection<Toy> Toys { get; set; }

        public AgeGroup(string interval)
        {
            Interval = interval;
        }
    }
}