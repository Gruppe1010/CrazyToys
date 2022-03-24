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
        public int QuantityToAdd { get; set; }
        public int OldAvailableQuantity { get; set; }

        public SelectedToy(string toyId, int quantityToAdd, int oldAvailableQuantity)
        {
            ToyId = toyId;
            QuantityToAdd = quantityToAdd;
            OldAvailableQuantity = oldAvailableQuantity;
        }
    }
}
