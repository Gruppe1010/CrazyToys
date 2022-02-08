using CrazyToys.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Models.Entities
{
    public class AgeGroup
    {

        [Column("IdAgeGroup")]
        public string Id { get; set; }
        public string Interval { get; set; }
        public ICollection<Toy> Toys { get; set; }



        public AgeGroupDTO convertToDTO()
        {
            return new AgeGroupDTO(Id, Interval);
        }

    }
}