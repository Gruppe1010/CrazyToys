namespace CrazyToys.Entities.DTOs
{
    public class CheckoutUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryName { get; set; }
        public string StreetAddress { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public CheckoutUserModel()
        {
        }

        public CheckoutUserModel(string firstName, string lastName, string countryName, string streetAddress, string cityName, string postalCode, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            CountryName = country;
            StreetAddress = streetAddress;
            CityName = cityName;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
