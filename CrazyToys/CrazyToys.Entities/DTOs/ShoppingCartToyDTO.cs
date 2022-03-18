using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class ShoppingCartToyDTO
    {
        public string ID;
        public string Name;
        public double Price;
        public int Quantity;
        public string Image;
        public int Stock;

        public ShoppingCartToyDTO(string iD, string name, double price, int quantity, string image, int stock)
        {
            ID = iD;
            Name = name;
            Price = price;
            Quantity = quantity;
            Image = image;
            Stock = stock;
        }

        public double CalculateTotalPrice()
        {
            return Price * Quantity;
        }
    }
}
