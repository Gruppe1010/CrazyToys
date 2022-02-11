using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Models.Entities
{
    public class Colour
    {
        [Column("ColourId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Toy> Toys { get; set; }

        public Colour(string name)
        {
            Name = name;
        }

        public Colour(int iD, string name)
        {
            ID = iD;
            Name = name;
        }
    }


}