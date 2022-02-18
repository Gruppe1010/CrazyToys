using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Colour
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<Toy> Toys { get; set; }

        public Colour(string name)
        {
            Name = name;
        }

        public Colour(string iD, string name)
        {
            ID = iD;
            Name = name;
        }
    }


}