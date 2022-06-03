using CrazyToys.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs.OrderDTOs
{
    // TODO SLET
    public class AdminOrderDTO
    {
        public List<OrderDTO> OrderDTOs { get; set; }

        public AdminOrderDTO(List<OrderDTO> orderDTOs)
        {
            OrderDTOs = orderDTOs;
        }


        public AdminOrderDTO(List<Order> orders)
        {
            OrderDTOs = new List<OrderDTO>();
            foreach (Order order in orders)
            {
                OrderDTOs.Add(order.ConvertToOrderDTO());
            }
        }

    }
}
