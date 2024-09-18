namespace FirstApiTry.TouristModels
{
    public class Booking
    {
        public int BookingId { get; set; }  // Maps to 'booking_id' in the database
        public int TouristId { get; set; }  // Maps to 'tourist_id' in the database
        public int GuideId { get; set; }  // Maps to 'guide_id' in the database
        public int RouteId { get; set; }  // Maps to 'route_id' in the database
        public DateTime TourDate { get; set; }  // Maps to 'tour_date' in the database
        public string BookingStatus { get; set; }  // Maps to 'booking_status' in the database
        public string SpecialRequests { get; set; }  // Maps to 'special_requests' in the database

    }
}
