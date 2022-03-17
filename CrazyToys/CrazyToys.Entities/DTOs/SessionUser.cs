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
        // Dict indeholder toyID og quantity
        public Dictionary<string, int> WishList { get; set; }
        public Dictionary<string, int> Cart { get; set; }
        
        public SessionUser()
        {
            WishList = new Dictionary<string, int>();
            Cart = new Dictionary<string, int>();
        }
    }
}
