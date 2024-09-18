namespace FirstApiTry.Models
{
    public class Guide
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Country { get; set; }
        public bool HasCar { get; set; }
        public decimal? AverageRating { get; set; }
        public string Languages { get; set; } // Changed from string[] to string to match SQL
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; } // Added DateOfBirth property

        public override string ToString()
        {
            return $"{ID}, {FirstName}, {LastName}, {Bio}, {Country}, {HasCar}, {AverageRating}, {Languages}, {Password}, {PhoneNumber}, {Email}, {DateOfBirth.ToShortDateString()}";
        }
    }
}
