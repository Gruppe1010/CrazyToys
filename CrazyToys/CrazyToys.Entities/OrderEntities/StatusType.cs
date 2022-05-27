using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class StatusType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public int StatusCode { get; set; }

        public StatusType()
        {
        }

        public StatusType(int statusCode)
        {
            StatusCode = statusCode;
        }

        public StatusType(string name, int statusCode)
        {
            Name = name;
            StatusCode = statusCode;
        }

    }
}
