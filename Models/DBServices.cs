using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FirstApiTry.Models
{
    public class DBServices
    {
        static string consStr = @"workstation id=GuideTour.mssql.somee.com;packet size=4096;user id=Amjad_Arkia_SQLLogin_1;pwd=pfkcgates6;data source=GuideTour.mssql.somee.com;persist security info=False;initial catalog=GuideTour;TrustServerCertificate=True";

        public static Guide Login(string email, string pass)
        {
            Guide guide = null;
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Guides WHERE email = @Email AND password = @Pass", con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Pass", pass);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    guide = new Guide()
                    {
                        ID = (int)rdr["guide_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Bio = rdr["bio"].ToString(),
                        Country = rdr["country"].ToString(),
                        HasCar = (bool)rdr["hasCar"],
                        AverageRating = rdr["average_rating"] != DBNull.Value ? (decimal)rdr["average_rating"] : (decimal?)null,
                        DateOfBirth = (DateTime)rdr["date_of_birth"],  
                        Languages = rdr["languages"].ToString(),         
                        Email = email,
                        Password = pass,
                        PhoneNumber = rdr["phone_number"].ToString()
                    };
                }
            }
            return guide;
        }

        public static List<Guide> GetGuides()
        {
            List<Guide> guides = new List<Guide>();
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Guides", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Guide guide = new Guide()
                    {
                        ID = (int)rdr["guide_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Bio = rdr["bio"].ToString(),
                        Country = rdr["country"].ToString(),
                        HasCar = (bool)rdr["hasCar"],
                        AverageRating = rdr["average_rating"] != DBNull.Value ? (decimal)rdr["average_rating"] : (decimal?)null,
                        Password = rdr["password"].ToString(),
                        DateOfBirth = (DateTime)rdr["date_of_birth"],  
                        Languages = rdr["languages"].ToString(),         
                        Email = rdr["email"].ToString(),
                        PhoneNumber = rdr["phone_number"].ToString()
                    };
                    guides.Add(guide);
                }
            }
            return guides;
        }

        public static Guide GetGuideById(int id)
        {
            Guide guide = null;
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Guides WHERE guide_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    guide = new Guide()
                    {
                        ID = (int)rdr["guide_id"],
                        FirstName = rdr["first_name"].ToString(),
                        LastName = rdr["last_name"].ToString(),
                        Bio = rdr["bio"].ToString(),
                        Country = rdr["country"].ToString(),
                        HasCar = (bool)rdr["hasCar"],
                        AverageRating = rdr["average_rating"] != DBNull.Value ? (decimal)rdr["average_rating"] : (decimal?)null,
                        Password = rdr["password"].ToString(),
                        DateOfBirth = (DateTime)rdr["date_of_birth"],  // Added DateOfBirth
                        Languages = rdr["languages"].ToString(),         // Updated Languages
                        Email = rdr["email"].ToString(),
                        PhoneNumber = rdr["phone_number"].ToString()
                    };
                }
            }
            return guide;
        }

        public static void RegisterGuide(Guide guide)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Guides (first_name, last_name, bio, country, hasCar, average_rating, password, phone_number, email, date_of_birth, languages) " +
                    "OUTPUT INSERTED.guide_id " +
                    "VALUES (@FirstName, @LastName, @Bio, @Country, @HasCar, @AverageRating, @Password, @PhoneNumber, @Email, @DateOfBirth, @Languages)", con);

                cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                cmd.Parameters.AddWithValue("@Bio", guide.Bio);
                cmd.Parameters.AddWithValue("@Country", guide.Country);
                cmd.Parameters.AddWithValue("@HasCar", guide.HasCar);
                cmd.Parameters.AddWithValue("@AverageRating", guide.AverageRating.HasValue ? (object)guide.AverageRating.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", guide.Password);
                cmd.Parameters.AddWithValue("@PhoneNumber", guide.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", guide.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", guide.DateOfBirth);  
                cmd.Parameters.AddWithValue("@Languages", guide.Languages);      

                con.Open();
                guide.ID = (int)cmd.ExecuteScalar();
            }
        }

        public static void UpdateGuide(int id, Guide guide)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Guides SET first_name = @FirstName, last_name = @LastName, bio = @Bio, country = @Country, " +
                    "hasCar = @HasCar, average_rating = @AverageRating, password = @Password, phone_number = @PhoneNumber, " +
                    "email = @Email, date_of_birth = @DateOfBirth, languages = @Languages WHERE guide_id = @ID", con);

                cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                cmd.Parameters.AddWithValue("@Bio", guide.Bio);
                cmd.Parameters.AddWithValue("@Country", guide.Country);
                cmd.Parameters.AddWithValue("@HasCar", guide.HasCar);
                cmd.Parameters.AddWithValue("@AverageRating", guide.AverageRating.HasValue ? (object)guide.AverageRating.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", guide.Password);
                cmd.Parameters.AddWithValue("@PhoneNumber", guide.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", guide.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", guide.DateOfBirth);  
                cmd.Parameters.AddWithValue("@Languages", guide.Languages);      
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static void DeleteGuide(int id)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Guides WHERE guide_id = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static List<Route> GetAllRoutes()
        {
            List<Route> routes = new List<Route>();

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Routes", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Route route = new Route()
                    {
                        RouteId = (int)rdr["route_id"],
                        Description = rdr["description"].ToString(),
                        Duration = (decimal)rdr["duration"],
                        DifficultyLevel = rdr["difficulty_level"].ToString(),
                        StartPoint = rdr["start_point"].ToString(),
                        EndPoint = rdr["end_point"].ToString(),
                        RouteType = (string)rdr["route_type"],
                    };
                    routes.Add(route);
                }
            }
            return routes;
        }
        public static Route GetRouteById(int routeId)
        {
            Route route = null;

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Routes WHERE route_id = @RouteId", con);
                cmd.Parameters.AddWithValue("@RouteId", routeId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    route = new Route()
                    {
                        RouteId = (int)rdr["route_id"],
                        Description = rdr["description"].ToString(),
                        Duration = (decimal)rdr["duration"],
                        DifficultyLevel = rdr["difficulty_level"].ToString(),
                        StartPoint = rdr["start_point"].ToString(),
                        EndPoint = rdr["end_point"].ToString(),
                        RouteType = (string)rdr["route_type"],
                    };
                }
            }
            return route;
        }


        public static List<Route> GetRoutesByGuideId(int guideId)
        {
            List<Route> routes = new List<Route>();

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Routes WHERE guide_id = @GuideId ", con);
                cmd.Parameters.AddWithValue("@GuideId", guideId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Route route = new Route()
                    {
                        RouteId = (int)rdr["route_id"],
                        Description = rdr["description"].ToString(),
                        Duration = (decimal)rdr["duration"],
                        DifficultyLevel = rdr["difficulty_level"].ToString(),
                        StartPoint = rdr["start_point"].ToString(),
                        EndPoint = rdr["end_point"].ToString(),
                        RouteType = (string)rdr["route_type"],
                       
                    };
                    routes.Add(route);
                }
            }
            return routes;
        }
        
        public static void AddRoute(int guideId, string description, decimal duration, string difficultyLevel, string startPoint, string endPoint, string routeType)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Routes (guide_id, description, duration, difficulty_level, start_point, end_point, route_type) " +
                    "VALUES (@GuideId, @Description, @Duration, @DifficultyLevel, @StartPoint, @EndPoint, @RouteType, 0)", con);

                cmd.Parameters.AddWithValue("@GuideId", guideId);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@DifficultyLevel", difficultyLevel);
                cmd.Parameters.AddWithValue("@StartPoint", startPoint);
                cmd.Parameters.AddWithValue("@EndPoint", endPoint);
                cmd.Parameters.AddWithValue("@RouteType", routeType); 

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static List<Review> GetReviewsByGuideId(int guideId)
        {
            List<Review> reviews = new List<Review>();

            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Reviews WHERE guide_id = @GuideId ", con);
                cmd.Parameters.AddWithValue("@GuideId", guideId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Review review = new Review()
                    {
                        ReviewId = (int)rdr["review_id"],
                        GuideId = (int)rdr["guide_id"],
                        TouristId = (int)rdr["tourist_id"],
                        Rating = (decimal)rdr["rating"],
                        Comment = rdr["comment"].ToString(),
                        ReviewDate = (DateTime)rdr["review_date"],
                        
                    };
                    reviews.Add(review);
                }
            }
            return reviews;
        }

        public static void DeleteRoute(int guideId, int routeId)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Routes WHERE guide_id = @GuideId AND route_id = @RouteId", con);

                cmd.Parameters.AddWithValue("@GuideId", guideId);
                cmd.Parameters.AddWithValue("@RouteId", routeId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateRoute(int guideId, int routeId, string description, decimal duration, string difficultyLevel, string startPoint, string endPoint, string routeType)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Routes SET description = @Description, duration = @Duration, difficulty_level = @DifficultyLevel, " +
                    "start_point = @StartPoint, end_point = @EndPoint, route_type = @RouteType " +
                    "WHERE guide_id = @GuideId AND route_id = @RouteId", con);

                cmd.Parameters.AddWithValue("@GuideId", guideId);
                cmd.Parameters.AddWithValue("@RouteId", routeId);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@DifficultyLevel", difficultyLevel);
                cmd.Parameters.AddWithValue("@StartPoint", startPoint);
                cmd.Parameters.AddWithValue("@EndPoint", endPoint);
                cmd.Parameters.AddWithValue("@RouteType", routeType);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



    }
}
