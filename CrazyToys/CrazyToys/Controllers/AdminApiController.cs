using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Services.ProductDbServices;
using CrazyToys.Services.SalesDbServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Web.Controllers
{
    [Route("api/admin/[action]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly OrderDbService _orderDbService;
        private readonly StatusTypeDbService _statusTypeDbService;
        private readonly IPaymentService _paymentService;




        public AdminApiController(
            OrderDbService orderDbService,
            StatusTypeDbService statusTypeDbService,
            IPaymentService paymentService)
        {
            _orderDbService = orderDbService;
            _statusTypeDbService = statusTypeDbService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult> ShipOrder([FromQuery(Name = "orderId")] string orderId)
        {
            if(orderId != null)
            {
                Order order = await _orderDbService.GetById(orderId);
                StatusType statusType = await _statusTypeDbService.GetByStatusCode(4);
                order.Statuses.Add(new Status(statusType));
                await _orderDbService.Update(order);

                // TRÆK PENGEEEEEE
                await _paymentService.CapturePayment(order.PaymentID, order.CalculateTotalPrice());

                return Ok();
            }

            return StatusCode(500);
        }
    }
}
