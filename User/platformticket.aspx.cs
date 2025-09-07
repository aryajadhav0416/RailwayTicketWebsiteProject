using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web.Services;
using System.Web.UI;
using WebGrease.Activities;

namespace RailwayTicketWebsite.User
{
    public partial class platformticket : System.Web.UI.Page
    {
        private double farePerPerson = 10.00; // Example fare per person

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                if (!IsPostBack)
                {
                    UpdateTotalFare();
                }
            }
            else
            {
                Response.Redirect("userloginpg.aspx");
            }
        }

        [WebMethod]
        public static bool ValidateStations(string station)
        {
            bool isValidStation = false;

            string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT COUNT(1) FROM Station WHERE station_name = @StationName";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Validate station
                    cmd.Parameters.AddWithValue("@StationName", station);
                    isValidStation = (int)cmd.ExecuteScalar() > 0;
                }
            }

            return isValidStation;
        }

        [WebMethod]
        public static string[] GetStationSuggestions(string prefixText, int count)
        {
            return GetSuggestions(prefixText, count);
        }

        private static string[] GetSuggestions(string prefixText, int count)
        {
            List<string> suggestions = new List<string>();
            string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT DISTINCT TOP (@Count) station_name FROM Station WHERE station_name LIKE @PrefixText + '%'";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Count", count);
                    cmd.Parameters.AddWithValue("@PrefixText", prefixText);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestions.Add(reader["station_name"].ToString());
                        }
                    }
                }
            }

            return suggestions.ToArray();
        }

        protected void ddlPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotalFare();
        }

        private void UpdateTotalFare()
        {
            int persons = int.Parse(ddlPersons.SelectedValue);
            double totalFare = farePerPerson * persons;
            lblTotalFare.Text = $"Total Fare: {totalFare:C}";
        }

        protected void btnBookTicket_Click(object sender, EventArgs e)
        {
            // Retrieve user inputs
            string station = txtCity.Text;
            int persons = int.Parse(ddlPersons.SelectedValue);
            string paymentType = ddlPaymentType.SelectedValue;
            string username = Session["Username"].ToString();
            double totalFare = farePerPerson * persons;
            List<string> validCities = GetValidCities();

            // Validate if the city input is in the list of valid cities
            if (!validCities.Contains(station))
            {
                lblError.Text = "Invalid city. Please select a city from the list.";
                return; // Exit the method without proceeding further
            }
            Session["totalFare"] = totalFare;
            Session["payType"] = paymentType;
            Session["Count"] = persons;
            Session["platstn"] = station;
            Session["referringPage"]="platform";
            Response.Redirect("paygate.aspx");
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
