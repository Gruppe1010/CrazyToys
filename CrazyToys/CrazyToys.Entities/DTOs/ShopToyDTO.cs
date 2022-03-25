namespace CrazyToys.Entities.DTOs
{
    public class ShopToyDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

        public ShopToyDTO(string id, string name, int price, string image)
        {
            ID = id;
            Name = name;
            Price = price;
            Image = image;
        }
    }
}
