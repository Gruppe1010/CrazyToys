using CrazyToys.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.Entities
{
    public class SessionUser
    {
        public List<SelectedToy> WishList { get; set; }
        public List<SelectedToy> Cart { get; set; }
        public double TotalPriceCart
        {
            get
            {
                double total = 0;

                foreach (var selectedToy in Cart)
                {
                    total += selectedToy.Toy.Price * selectedToy.Quantity;
                }
                return total;
            }
            set { }
        }

        public SessionUser()
        {
            WishList = new List<SelectedToy>();
            Cart = new List<SelectedToy>();
        }
    }
}
