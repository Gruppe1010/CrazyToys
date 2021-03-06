using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Error
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Exception { get; set; }
        public string DateString { get; set; }

        public Error(string exception, string dateString)
        {
            Exception = exception;
            DateString = dateString;
        }
    }
}
