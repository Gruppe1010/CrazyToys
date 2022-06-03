using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IMailService
    {
        MailDTO CreateOrderConfirmation(Order order, List<ShoppingCartToyDTO> shoppingCartToyDTOs);

        MailDTO CreateShippingConfirmation(Order order, List<ShoppingCartToyDTO> shoppingCartToyDTOs);

    }
}
