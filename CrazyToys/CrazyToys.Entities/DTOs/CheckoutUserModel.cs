namespace CrazyToys.Entities.DTOs
{
    public class CheckoutUserModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public CheckoutUserModel()
        {
        }

        public CheckoutUserModel(string firstname, string lastname, string country, string address, string city, string postcode, string phone, string email)
        {
            Firstname = firstname;
            Lastname = lastname;
            Country = country;
            Address = address;
            City = city;
            Postcode = postcode;
            Phone = phone;
            Email = email;
        }
    }
}
