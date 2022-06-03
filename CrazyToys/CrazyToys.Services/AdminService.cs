using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Services.SalesDbServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class AdminService
    {
        private readonly OrderDbService _orderDbService;

        public AdminService(OrderDbService orderDbService)
        {
            _orderDbService = orderDbService;
        }
        
        public async Task<List<OrderDTO>> GetAdminOrderDTO()
        {
            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            List<Order> orders = await _orderDbService.GetAllWithRelations();

            foreach(Order order in orders)
            {
                orderDTOs.Add(order.ConvertToOrderDTO());
            }

            return orderDTOs;
        }


    }
}
