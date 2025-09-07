using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class registerpg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear the fields on initial load
                fullname.Text = string.Empty;
                pass.Text = string.Empty;
                username.Text = string.Empty;
                dob.Text = string.Empty;
                mobile.Text = string.Empty;
                city.Text = string.Empty;
                debugLabel.Text = string.Empty;
            }
        }

        protected void GenerateOTP_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string username = fullname.Text;
                string password = pass.Text;
                string cityInput = city.Text;
                Session["password"] = password;
                // Fetch valid cities from the database
                List<string> validCities = GetValidCities();

                // Validate if the city input is in the list of valid cities
                if (!validCities.Contains(cityInput))
                {
                    debugLabel.Text = "Invalid city. Please select a city from the list.";
                    return; // Exit the method without proceeding further
                }
                RegisterUser();
                Session["Username"] = fullname.Text;
                Response.Redirect("userloginpg.aspx");
            }
        }

        protected void ValidateUsername(object source, ServerValidateEventArgs args)
        {
            args.IsValid = IsUsernameUnique(args.Value);
        }

        private bool IsUsernameUnique(string username)
        {
            string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    conn.Open();
                    return (int)cmd.ExecuteScalar() == 0;
                }
            }
        }

        private void RegisterUser()
        {
            string fullName = fullname.Text;
            string userName = username.Text;
            string pass = Session["password"].ToString();
            DateTime dateOfBirth = DateTime.Parse(dob.Text);
            string mobileNumber = mobile.Text;
            string cityInput = city.Text;
            string role = "User";

            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FullName, Username, pass, BirthDate, PhoneNumber, City, Role) VALUES (@FullName, @Username, @Password, @BirthDate, @Mobile, @City, @Role)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Username", userName);
                command.Parameters.AddWithValue("@Password", pass);
                command.Parameters.AddWithValue("@BirthDate", dateOfBirth);
                command.Parameters.AddWithValue("@Mobile", mobileNumber);
                command.Parameters.AddWithValue("@City", cityInput);
                command.Parameters.AddWithValue("@Role", role);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        [WebMethod]
        public static List<string> GetCitySuggestions(string prefixText, int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT station_name FROM Station WHERE station_name LIKE @PrefixText";

            List<string> suggestions = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PrefixText", prefixText + "%");
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sourceName = reader["station_name"].ToString();
                            string suggestion = $"{sourceName}";
                            suggestions.Add(suggestion);
                        }
                    }
                }
            }

            return suggestions;
        }

        private List<string> GetValidCities()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT DISTINCT station_name FROM Station";

            List<string> validCities = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            validCities.Add(reader["station_name"].ToString());
                        }
                    }
                }
            }

            return validCities;
        }
    }
}
