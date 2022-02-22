using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
