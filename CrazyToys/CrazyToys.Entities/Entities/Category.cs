using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Category
    {
        [Column("CategoryId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        // TODO skal denne have en collection af Category?

    }
}