using CrazyToys.Entities.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Models.Entities
{
    public class AgeGroup
    {

        [Column("AgeGroupId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Interval { get; set; }
        public ICollection<Toy> Toys { get; set; }

        public AgeGroup(string interval)
        {
            Interval = interval;
        }

        public AgeGroupDTO convertToDTO()
        {
            return new AgeGroupDTO(ID, Interval);
        }

    }
}