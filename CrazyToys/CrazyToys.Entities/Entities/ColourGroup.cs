using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class ColourGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string ColourCode { get; set; }
        public ICollection<Toy> Toys { get; set; }
        [Column("SortingKeywords")]
        public string _sortingKeywords { get; set; }
        [NotMapped]
        public string[] SortingKeywords
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sortingKeywords))
                {
                    return new string[] { };
                }
                return _sortingKeywords.Split("%");
            }
            set
            {
                _sortingKeywords = string.Join("%", value);
            }
        }





        public ColourGroup(string name)
        {
            Name = name;
        }

        public ColourGroup(string name, string[] sortingKeyWords, string colourCode)
        {
            Name = name;
            SortingKeywords = sortingKeyWords;
            ColourCode = colourCode;
        }

        public ColourGroup(string iD, string name)
        {
            ID = iD;
            Name = name;
        }
    }


}