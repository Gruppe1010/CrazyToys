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

        public SelectedToy(string toyId, int quantity)
        {
            ToyId = toyId;
            Quantity = quantity;
        }
    }
}
