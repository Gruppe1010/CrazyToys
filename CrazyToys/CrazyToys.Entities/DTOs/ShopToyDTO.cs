﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class ShopToyDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }

        public ShopToyDTO(string id, string name, int price, string imageUrl)
        {
            Id = id;
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
