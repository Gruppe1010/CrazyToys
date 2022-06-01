using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class MailService : IMailService
    {
        public MailDTO CreateOrderConfirmation(Order order, List<ShoppingCartToyDTO> shoppingCartToyDTOs)
        {

            MailDTO mailDTO = null;

            if (order != null)
            {
                string subject = $"Din bestilling er modtaget! ({order.OrderNumber})";

                string addressBodyText = CreateAddressBodyText(order);
                string toyLinesBodyText = CreateToyLinesBodyText(shoppingCartToyDTOs);

                string body = $"<h1> Hej {order.Customer.FirstName}!</h1>" +
                $"<h2> Tak for din ordre!</h2>" +
                addressBodyText +
                "<h2> Følgende varer vil blive sendt til din leveringsadresse hurtigst muligt </h2>" + toyLinesBodyText;

                mailDTO = new MailDTO("gruppe1010@hotmail.com", order.Customer.Email, subject, body);
            }

            return mailDTO;
        }


        public MailDTO CreateShippingConfirmation(Order order, List<ShoppingCartToyDTO> shoppingCartToyDTOs)
        {
            MailDTO mailDTO = null;

            if (order != null)
            {
                string subject = $"Din ordre er sendt! ({order.OrderNumber}) ";

                string addressBodyText = CreateAddressBodyText(order);
                string toyLinesBodyText = CreateToyLinesBodyText(shoppingCartToyDTOs);

                string body = $"<h1> Hej {order.Customer.FirstName}!</h1>" +
                $"<h2> Din ordre er nu sendt, og du kan glæde dig!</h2>" +
                $"<h3> Forventet levering: 2-3 dage </h3>" + addressBodyText + toyLinesBodyText;

                mailDTO = new MailDTO("gruppe1010@hotmail.com", order.Customer.Email, subject, body);
            }

            return mailDTO;
        }


        private string CreateAddressBodyText(Order order)
        {
            AddressDTO billingAddress = order.CreateBillingAddressDTO();
            AddressDTO shippingAddress = order.CreateShppingAddressDTO();

            string date = order.CreateDateString();

            return $"<h2>Oplysninger </h2>" +
            $"<h4> Ordrenummer: {order.OrderNumber} </h4>" +
            $"<h4> Ordredato: {date}</h4>" +
            $"<h4> Faktureringsadresse:</h4>" +
            $"<p>{order.Customer.FirstName} {order.Customer.LastName}</p>" +
            $"<p>{billingAddress.StreetAddress}</p>" +
            $"<p>{billingAddress.City}</p>" +
            $"<p>{billingAddress.Country}</p>" +

            $"<h4> Leveringssadresse:</h4>" +
            $"<p>{order.Customer.FirstName} {order.Customer.LastName}</p>" +
            $"<p>{shippingAddress.StreetAddress}</p>" +
            $"<p>{shippingAddress.City}</p>" +
            $"<p>{shippingAddress.Country}</p>";
        }


        private string CreateToyLinesBodyText(List<ShoppingCartToyDTO> shoppingCartToyDTOs)
        {
            string bodyText = "";
            double subTotal = 0;
            double totalPrice;

            foreach (ShoppingCartToyDTO toy in shoppingCartToyDTOs)
            {
                var subAmount = toy.CalculateTotalPrice();

                bodyText = bodyText + "<tr></tr>" + "<tr>" + "<td>" + "<img width='90' height='90' src='" + toy.Image + "'>" + "</td>" + "<td>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + toy.Name + "&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + toy.Quantity + " stk.&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td align='right'>" + subAmount + " DKK" + "</td>" + "</tr>";
                subTotal = subTotal + subAmount;
            }

            string freightPrice = "39 DKK";
            if (subTotal > 499)
            {
                totalPrice = subTotal;
                freightPrice = "Gratis levering";
            }
            else
            {
                totalPrice = subTotal + 39;
            }

            return "<h2> Din bestilling: </h2>" +
            "<table>" +
                "<tr> <th align='left'> Produkt </th> <th> </th> <th> Antal </th> <th align='right'> Pris </th></tr>" +
                $"<tbody>{bodyText}</tbody>" +
                $"<tr><td>&nbsp;</td></tr>" +
                $"<tr><th align='left'> Subtotal </th> <th> </th> <th> </th> <th align='right'> {subTotal} DKK" + "</th></tr>" +
                $"<tr><th align='left'> Fragt </th> <th> </th> <th> </th> <th align='right'>{freightPrice} </th></tr>" +
                 "<tr><td>&nbsp;</td></tr>" +
                $"<tr><th align='left'> Total </th> <th> </th> <th> </th> <th align='right'>{ totalPrice} DKK" + "</th></tr>" +
            "</table>";
        }
    }
}
