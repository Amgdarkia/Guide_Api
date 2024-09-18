namespace FirstApiTry.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string Description { get; set; }
        public decimal Duration { get; set; }
        public string DifficultyLevel { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string RouteType { get; set; } 
       
    }
}
