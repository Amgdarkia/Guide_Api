using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FirstApiTry.Models
{
    public class DBServices
    {
        static string consStr = @"Data Source=DESKTOP-31CEKIV\SQLEXPRESS01;Initial Catalog=TourGuideHubDB;Integrated Security=True;TrustServerCertificate=True";

        public static Guide Login(string Name, string password)
        {
            Guide Gui2ret = null;
            SqlConnection con = new SqlConnection(consStr);
            SqlCommand cmd = new SqlCommand(
                 "SELECT * FROM Guides WHERE first_name = @Name AND Password = @password", con);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@password", password);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Gui2ret = new Guide()
                {
                    ID = (int)rdr["guide_id"],
                    FirstName = rdr["first_name"].ToString(),
                    LastName = rdr["last_name"].ToString(),
                    Bio = rdr["bio"].ToString(),
                    Country = rdr["country"].ToString(),
                    HasCar = (bool)rdr["hasCar"],
                    AverageRating = rdr["average_rating"] != DBNull.Value ? (decimal)rdr["average_rating"] : (decimal?)null,
                    IsDeleted = (bool)rdr["is_deleted"],
                    Languages = GetLanguages((int)rdr["guide_id"])
                };
            }
            con.Close();
            return Gui2ret;
        }

        public static List<Guide> GetGuides()
        {
            List<Guide> guides = new List<Guide>();
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Guides WHERE is_deleted = 0", con);
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
                        IsDeleted = (bool)rdr["is_deleted"],
                        Languages = GetLanguages((int)rdr["guide_id"])
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Guides WHERE guide_id = @id AND is_deleted = 0", con);
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
                        IsDeleted = (bool)rdr["is_deleted"],
                        Languages = GetLanguages((int)rdr["guide_id"])
                    };
                }
            }
            return guide;
        }

        public static void AddGuide(Guide guide)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Guides (first_name, last_name, bio, country, hasCar, average_rating, is_deleted) " +
                    "OUTPUT INSERTED.guide_id " +
                    "VALUES (@FirstName, @LastName, @Bio, @Country, @HasCar, @AverageRating, @IsDeleted)", con);
                cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                cmd.Parameters.AddWithValue("@Bio", guide.Bio);
                cmd.Parameters.AddWithValue("@Country", guide.Country);
                cmd.Parameters.AddWithValue("@HasCar", guide.HasCar);
                cmd.Parameters.AddWithValue("@AverageRating", guide.AverageRating.HasValue ? (object)guide.AverageRating.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@IsDeleted", guide.IsDeleted);

                con.Open();
                guide.ID = (int)cmd.ExecuteScalar();

                foreach (var language in guide.Languages)
                {
                    AddLanguage(guide.ID, language);
                }
            }
        }

        public static void UpdateGuide(int id, Guide guide)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Guides SET first_name = @FirstName, last_name = @LastName, bio = @Bio, country = @Country, " +
                    "hasCar = @HasCar, average_rating = @AverageRating, is_deleted = @IsDeleted WHERE guide_id = @ID", con);
                cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                cmd.Parameters.AddWithValue("@Bio", guide.Bio);
                cmd.Parameters.AddWithValue("@Country", guide.Country);
                cmd.Parameters.AddWithValue("@HasCar", guide.HasCar);
                cmd.Parameters.AddWithValue("@AverageRating", guide.AverageRating.HasValue ? (object)guide.AverageRating.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@IsDeleted", guide.IsDeleted);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();

                UpdateLanguages(id, guide.Languages);
            }
        }

        public static void DeleteGuide(int id)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Guides SET is_deleted = 1 WHERE guide_id = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static string[] GetLanguages(int guideId)
        {
            List<string> languages = new List<string>();
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT language FROM LanguageProficiency WHERE guide_id = @guideId AND is_deleted = 0", con);
                cmd.Parameters.AddWithValue("@guideId", guideId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    languages.Add(rdr["language"].ToString());
                }
            }
            return languages.ToArray();
        }

        private static void AddLanguage(int guideId, string language)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO LanguageProficiency (guide_id, language, is_deleted) VALUES (@guideId, @language, 0)", con);
                cmd.Parameters.AddWithValue("@guideId", guideId);
                cmd.Parameters.AddWithValue("@language", language);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static void UpdateLanguages(int guideId, string[] languages)
        {
            using (SqlConnection con = new SqlConnection(consStr))
            {
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM LanguageProficiency WHERE guide_id = @guideId", con);
                deleteCmd.Parameters.AddWithValue("@guideId", guideId);
                con.Open();
                deleteCmd.ExecuteNonQuery();
                con.Close();

                foreach (var language in languages)
                {
                    AddLanguage(guideId, language);
                }
            }
        }
    }
}
