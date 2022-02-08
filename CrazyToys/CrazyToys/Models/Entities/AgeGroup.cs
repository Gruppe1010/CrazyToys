using CrazyToys.Web.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class AgeGroup
    {

        [Column("AgeGroupId")]
        public string ID { get; set; }
        public string Interval { get; set; }
        public ICollection<Toy> Toys { get; set; }



        public AgeGroupDTO convertToDTO()
        {
            return new AgeGroupDTO(ID, Interval);
        }

    }
}