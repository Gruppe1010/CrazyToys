using System.Collections.Generic;

namespace CrazyToys.Entities.DTOs
{
    public class SessionUser
    {
        public HashSet<string> Wishlist { get; set; }
        // Dict indeholder toyID og quantity
        // TODO denne skal nu ændres til ProductId
        public Dictionary<string, int> Cart { get; set; }

        public SessionUser()
        {
            Wishlist = new HashSet<string>();
            Cart = new Dictionary<string, int>();
        }
    }
}
