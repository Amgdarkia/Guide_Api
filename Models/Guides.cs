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
        public bool IsDeleted { get; set; }
        public string[] Languages { get; set; }
        public override string ToString()
        {
            return $"{ID}, {FirstName}, {LastName}, {Bio}, {Country}, {HasCar}, {AverageRating}, {IsDeleted}, {String.Join(", ", Languages)}";
        }
    }
}
