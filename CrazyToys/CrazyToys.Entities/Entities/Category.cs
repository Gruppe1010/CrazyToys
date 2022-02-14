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

        [Column("SortingKeywords")]
        public string _sortingKeywords { get; set; }
        [NotMapped]
        public string[] SortingKeywords
        {
            get {
                if (string.IsNullOrWhiteSpace(_sortingKeywords))
                {
                    return new string[] {};
                }
                return _sortingKeywords.Split("%"); 
            }
            set
            {
                _sortingKeywords = string.Join("%", value);
            }
        }
        public ICollection<SubCategory> SubCategories { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        public Category(string name, string[] sortingKeywords)
        {
            Name = name;
            SortingKeywords = sortingKeywords;
        }

        // TODO skal denne have en collection af Category?

    }
}