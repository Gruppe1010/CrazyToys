using CrazyToys.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public string ID { get; set; }
        public int OrderNumber { get; set; }
        public IList<Status> Statuses { get; set; }

        public OrderDTO(string iD, int orderNumber, IList<Status> statuses)
        {
            ID = iD;
            OrderNumber = orderNumber;
            Statuses = statuses;
        }


        public bool HasStatus(string statusTypeName)
        {
            foreach(var status in Statuses)
            {
                if(status.StatusType.Name.Equals(statusTypeName))
                {
                    return true;
                }
            }
            return false;
        }

        public Status FindLatestStatus()
        {
            if(Statuses != null)
            {
                Statuses = Statuses.OrderByDescending(o => o.TimeStamp).ToList();
                return Statuses[0];
            }

            return null;
        }
    }
}
