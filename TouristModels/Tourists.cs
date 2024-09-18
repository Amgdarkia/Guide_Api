namespace FirstApiTry.TouristModels
{
    public class Tourist
    {
        public int TouristId { get; set; }  // Maps to 'tourist_id' in the database
        public string FirstName { get; set; }  // Maps to 'first_name'
        public string LastName { get; set; }  // Maps to 'last_name'
        public string Email { get; set; }  // Maps to 'email'
        public string PhoneNumber { get; set; }  // Maps to 'phone_number'
        public string Country { get; set; }  // Maps to 'country'
        public DateTime DateOfBirth { get; set; }  // Maps to 'date_of_birth'
        public string Password { get; set; }  // Maps to 'password'
    }
}
