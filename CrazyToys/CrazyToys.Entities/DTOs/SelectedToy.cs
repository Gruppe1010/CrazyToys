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
