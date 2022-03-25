namespace CrazyToys.Entities.DTOs
{
    public class ShoppingCartToyDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }

        public ShoppingCartToyDTO(string id, string name, double price, int quantity, string image, int stock)
        {
            ID = id;
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
