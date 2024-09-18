using System.Data.SqlClient;
using FirstApiTry.Models;

namespace FirstApiTry.TouristModels
{
    public class TouristDBServices
    {
        static string consStr = "workstation id=GuideTour.mssql.somee.com;packet size=4096;user id=Amjad_Arkia_SQLLogin_1;pwd=pfkcgates6;data source=GuideTour.mssql.somee.com;persist security info=False;initial catalog=GuideTour;TrustServerCertificate=True";

        public static List<Tourist> GetAllTourists()
        {
            List<Tourist> tourists = new List<Tourist>();

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tourists", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Tourist tourist = new Tourist()
                    {
                        TouristId = (int)rdr["tourist_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Email = rdr["email"].ToString(),
                        PhoneNumber = rdr["phone_number"].ToString(),
                        Country = rdr["country"].ToString(),
                        DateOfBirth = (DateTime)rdr["date_of_birth"],
                        Password = rdr["password"].ToString()  // Fetch and store the password
                    };
                    tourists.Add(tourist);
                }
            }
            return tourists;
        }

        public static Tourist GetTouristById(int id)
        {
            Tourist tourist = null;

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tourists WHERE tourist_id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    tourist = new Tourist()
                    {
                        TouristId = (int)rdr["tourist_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Email = rdr["email"].ToString(),
                        PhoneNumber = rdr["phone_number"].ToString(),
                        Country = rdr["country"].ToString(),
                        DateOfBirth = (DateTime)rdr["date_of_birth"],
                        Password = rdr["password"].ToString()  // Fetch and store the password
                    };
                }
            }
            return tourist;
        }

        public static Tourist LoginTourist(string email, string password)
        {
            Tourist tourist = null;

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tourists WHERE email = @Email AND password = @Password", con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    tourist = new Tourist()
                    {
                        TouristId = (int)rdr["tourist_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Email = rdr["email"].ToString(),
                        PhoneNumber = rdr["phone_number"].ToString(),
                        Country = rdr["country"].ToString(),
                        DateOfBirth = (DateTime)rdr["date_of_birth"],
                        Password = rdr["password"].ToString()
                    };
                }
            }
            return tourist;
        }
        public static void DeleteTourist(int id)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Tourists WHERE tourist_id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateTourist(int id, Tourist tourist)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Tourists SET first_name = @FirstName, last_name = @LastName, email = @Email, " +
                    "phone_number = @PhoneNumber, country = @Country, date_of_birth = @DateOfBirth, password = @Password " +
                    "WHERE tourist_id = @Id", con);

                cmd.Parameters.AddWithValue("@FirstName", tourist.FirstName);
                cmd.Parameters.AddWithValue("@LastName", tourist.LastName);
                cmd.Parameters.AddWithValue("@Email", tourist.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", tourist.PhoneNumber);
                cmd.Parameters.AddWithValue("@Country", tourist.Country);
                cmd.Parameters.AddWithValue("@DateOfBirth", tourist.DateOfBirth);
                cmd.Parameters.AddWithValue("@Password", tourist.Password);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void RegisterTourist(Tourist tourist)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Tourists (first_name, last_name, email, phone_number, country, date_of_birth, password) " +
                    "VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Country, @DateOfBirth, @Password)", con);

                cmd.Parameters.AddWithValue("@FirstName", tourist.FirstName);
                cmd.Parameters.AddWithValue("@LastName", tourist.LastName);
                cmd.Parameters.AddWithValue("@Email", tourist.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", tourist.PhoneNumber);
                cmd.Parameters.AddWithValue("@Country", tourist.Country);
                cmd.Parameters.AddWithValue("@DateOfBirth", tourist.DateOfBirth);
                cmd.Parameters.AddWithValue("@Password", tourist.Password);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void AddReview(int touristId, int guideId, decimal rating, string comment)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Reviews (guide_id, tourist_id, rating, comment, review_date) " +
                    "VALUES (@GuideId, @TouristId, @Rating, @Comment, @ReviewDate)", con);

                cmd.Parameters.AddWithValue("@GuideId", guideId);
                cmd.Parameters.AddWithValue("@TouristId", touristId);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.Parameters.AddWithValue("@Comment", comment);
                cmd.Parameters.AddWithValue("@ReviewDate", DateTime.Now);  // Automatically set the review date to now

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static Review GetReview(int touristId, int guideId)
        {
            Review review = null;

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Reviews WHERE tourist_id = @TouristId AND guide_id = @GuideId", con);

                cmd.Parameters.AddWithValue("@TouristId", touristId);
                cmd.Parameters.AddWithValue("@GuideId", guideId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    review = new Review()
                    {
                        ReviewId = (int)rdr["review_id"],
                        GuideId = (int)rdr["guide_id"],
                        TouristId = (int)rdr["tourist_id"],
                        Rating = (decimal)rdr["rating"],
                        Comment = rdr["comment"].ToString(),
                        ReviewDate = (DateTime)rdr["review_date"]
                    };
                }
            }

            return review;
        }
        public static List<Booking> GetAllBookings()
        {
            List<Booking> bookings = new List<Booking>();

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bookings", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Booking booking = new Booking()
                    {
                        BookingId = (int)rdr["booking_id"],
                        TouristId = (int)rdr["tourist_id"],
                        GuideId = (int)rdr["guide_id"],
                        RouteId = (int)rdr["route_id"],
                        TourDate = (DateTime)rdr["tour_date"],
                        BookingStatus = rdr["booking_status"].ToString(),
                        SpecialRequests = rdr["special_requests"] != DBNull.Value ? rdr["special_requests"].ToString() : null
                    };
                    bookings.Add(booking);
                }
            }

            return bookings;
        }
    
        public static Booking GetBooking(int bookingId)
        {
            Booking booking = null;

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bookings WHERE booking_id = @BookingId", con);
                cmd.Parameters.AddWithValue("@BookingId", bookingId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    booking = new Booking()
                    {
                        BookingId = (int)rdr["booking_id"],
                        TouristId = (int)rdr["tourist_id"],
                        GuideId = (int)rdr["guide_id"],
                        RouteId = (int)rdr["route_id"],
                        TourDate = (DateTime)rdr["tour_date"],
                        BookingStatus = rdr["booking_status"].ToString(),
                        SpecialRequests = rdr["special_requests"] != DBNull.Value ? rdr["special_requests"].ToString() : null
                    };
                }
            }

            return booking;
        }

        public static void AddBooking(Booking booking)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Bookings (tourist_id, guide_id, route_id, tour_date, booking_status, special_requests) " +
                    "VALUES (@TouristId, @GuideId, @RouteId, @TourDate, @BookingStatus, @SpecialRequests)", con);

                cmd.Parameters.AddWithValue("@TouristId", booking.TouristId);
                cmd.Parameters.AddWithValue("@GuideId", booking.GuideId);
                cmd.Parameters.AddWithValue("@RouteId", booking.RouteId);
                cmd.Parameters.AddWithValue("@TourDate", booking.TourDate);
                cmd.Parameters.AddWithValue("@BookingStatus", booking.BookingStatus);
                cmd.Parameters.AddWithValue("@SpecialRequests", string.IsNullOrEmpty(booking.SpecialRequests) ? DBNull.Value : (object)booking.SpecialRequests);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void DeleteBooking(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Bookings WHERE booking_id = @BookingId", con);
                cmd.Parameters.AddWithValue("@BookingId", bookingId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



    }
}
