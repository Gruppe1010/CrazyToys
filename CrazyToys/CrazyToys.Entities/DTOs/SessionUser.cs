using System.Collections.Generic;

namespace CrazyToys.Entities.Entities
{
    public class SessionUser
    {
        // Dict indeholder toyID og quantity
        public HashSet<string> Wishlist { get; set; }
        public Dictionary<string, int> Cart { get; set; }
        
        public SessionUser()
        {
            Wishlist = new HashSet<string>();
            Cart = new Dictionary<string, int>();
        }
    }
}
