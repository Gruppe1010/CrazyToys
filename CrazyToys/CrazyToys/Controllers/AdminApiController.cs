using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Services;
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
        private readonly SalesService _salesService;
        private readonly MailService _mailService;
        private readonly IHangfireService _hangfireService;



        public AdminApiController(
            OrderDbService orderDbService,
            StatusTypeDbService statusTypeDbService,
            SalesService salesDataService,
            MailService mailService,
            IHangfireService hangfireService,
            IPaymentService paymentService)
        {
            _orderDbService = orderDbService;
            _statusTypeDbService = statusTypeDbService;
            _paymentService = paymentService;
            _salesService = salesDataService;
            _hangfireService = hangfireService;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult> ShipOrder([FromQuery(Name = "orderId")] string orderId)
        {
            if(orderId != null)
            {
                Order order = await _orderDbService.GetById(orderId);
                StatusType statusType = await _statusTypeDbService.GetByName("Shipped");
                order.Statuses.Add(new Status(statusType));
                await _orderDbService.Update(order);
                // send shipping mail

                // lav liste med toys som skal vises i orderConfirmation
                List<ShoppingCartToyDTO> orderConfirmationToyList = await _salesService.ConvertOrderLinesToShoppingCartToyDTOs(order.OrderLines);

                // Send email til bruger
                MailDTO mailDTO = _mailService.CreateShippingConfirmation(order, orderConfirmationToyList);

                _hangfireService.CreateMailJob(mailDTO);

                // træk pengene
                var capture = await _paymentService.CapturePayment(order.PaymentID, order.CalculateTotalPrice());


                return Ok();
            }

            return StatusCode(500);
        }
    }
}
