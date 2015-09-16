namespace DealsWhat.Application.WebApi.Models
{
    public class UpdateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public AddressViewModel DeliveryAddress { get; set; }
    }
}