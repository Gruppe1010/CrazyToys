using CrazyToys.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class SelectedToy
    {
        public string ToyId { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }

        public SelectedToy(string toyId, int quantity, int stock)
        {
            ToyId = toyId;
            Quantity = quantity;
            Stock = stock;
        }
    }
}
