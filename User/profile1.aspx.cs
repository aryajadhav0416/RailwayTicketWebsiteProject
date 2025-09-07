using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace RailwayTicketWebsite.User
{
    public partial class profile1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    fullname.Enabled = false;
                    txtCity.Enabled = false;
                    txtdob.Enabled = false;
                    mobile.Enabled = false;
                    // Load profile data on initial load
                    LoadProfileData();
                    saveButton.Style.Add("display", "none");
                    cancelButton.Style.Add("display", "none");
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void editButton_Click(object sender, EventArgs e)
        {
            fullname.Enabled = true;
            txtCity.Enabled = true;
            txtdob.Enabled = true;
            mobile.Enabled = true;
            // Hide Save and Cancel buttons, show Edit button
            saveButton.Style.Add("display", "block");
            cancelButton.Style.Add("display", "block");
            editButton.Style.Add("display", "none");
        }
        protected void SavePersonalDetails_Click(object sender, EventArgs e)
        {
            // Save Personal Details
            if (SavePersonalDetails())
            {
                // Disable text fields after saving
                fullname.Enabled = false;
                txtCity.Enabled = false;
                txtdob.Enabled = false;
                mobile.Enabled = false;
                // Hide Save and Cancel buttons, show Edit button
                saveButton.Style.Add("display", "none");
                cancelButton.Style.Add("display", "none");
                editButton.Style.Add("display", "block");
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Profile Details Updated Successfully.";
            }
        }

        protected void CancelPersonalDetails_Click(object sender, EventArgs e)
        {
            // Reload profile data to reset changes
            LoadProfileData();

            // Disable text fields without saving
            fullname.Enabled = false;
            txtCity.Enabled = false;
            txtdob.Enabled = false;
            mobile.Enabled = false;
            // Hide Save and Cancel buttons, show Edit button
            saveButton.Style.Add("display", "none");
            cancelButton.Style.Add("display", "none");
            editButton.Style.Add("display", "block");
        }

        private void LoadProfileData()
        {
            string username = Session["Username"]?.ToString();

            if (!string.IsNullOrEmpty(username))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT FullName, PhoneNumber, City, BirthDate FROM Users WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        fullname.Text = reader["FullName"].ToString();
                        mobile.Text = reader["PhoneNumber"].ToString();
                        txtCity.Text = reader["City"].ToString();
                        txtdob.Text = Convert.ToDateTime(reader["BirthDate"]).ToString("yyyy-MM-dd");
                    }
                }
            }
        }
        private bool SavePersonalDetails()
        {
            string username = Session["Username"].ToString();
            string name = fullname.Text;
            string address = txtCity.Text;
            string phone = mobile.Text;
            DateTime dob;
            if (!DateTime.TryParse(txtdob.Text, out dob))
            {
                // Handle invalid date format here
                lblError.Text = "Invalid Date of Birth format. Please use yyyy-MM-dd format.";
                return false;
            }
            List<string> validCities = GetValidCities();

            // Validate if the city input is in the list of valid cities
            if (!validCities.Contains(address))
            {
                lblError.Text = "Invalid city. Please select a city from the list.";
                return false; // Exit the method without proceeding further
            }
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET FullName = @Name, City = @City, BirthDate = @DOB, PhoneNumber = @Phone WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@City", address);
                    command.Parameters.AddWithValue("@DOB", dob);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Phone", phone);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception (log it, display error message, etc.)
                lblError.Text = "An error occurred: " + ex.Message;
                return false;
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