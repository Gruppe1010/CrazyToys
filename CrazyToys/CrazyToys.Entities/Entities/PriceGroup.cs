using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class PriceGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // Hvis man laver id-proppen om til int generer den fra 1 og op af
        public string ID { get; set; }
        public string Interval { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public PriceGroup(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            // hvis vi ikke giver en max-værdi med skal der stå fx 800+ og ikke 700-800
            Interval = Maximum != 0 
                ? Minimum + " - " + Maximum 
                : Minimum + "+";
        }
    }
}
