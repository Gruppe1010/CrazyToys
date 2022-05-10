using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public StatusType StatusType { get; set; }
        public DateTime TimeStamp { get; set; }

        public Status()
        {
        }

        public Status(StatusType statusType, DateTime timeStamp)
        {
            StatusType = statusType;
            TimeStamp = timeStamp;
        }
    }
}
