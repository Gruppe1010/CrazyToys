using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class WishlistToyDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }

        public WishlistToyDTO(string id, string name, double price, string image, int stock)
        {
            ID = id;
            Name = name;
            Price = price;
            Image = image;
            Stock = stock;
        }
    }
}
