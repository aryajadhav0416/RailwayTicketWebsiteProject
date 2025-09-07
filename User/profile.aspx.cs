using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace RailwayTicketWebsite.User
{
    public partial class Profile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    // Load profile data on initial load
                    LoadProfileData();
                    // Hide Save and Cancel buttons initially
                    saveButton.Style.Add("display", "none");
                    cancelButton.Style.Add("display", "none");
                    saveButton2.Style.Add("display", "none");
                    cancelButton2.Style.Add("display", "none");
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void SavePersonalDetails_Click(object sender, EventArgs e)
        {
            // Save Personal Details
            if (SavePersonalDetails())
            {
                // Disable text fields after saving
                txtName.Enabled = false;
                txtCity.Enabled = false;
                txtDOB.Enabled = false;

                // Hide Save and Cancel buttons, show Edit button
                saveButton.Style.Add("display", "none");
                cancelButton.Style.Add("display", "none");
                editButton.Style.Add("display", "block");
                Response.Redirect("dashboard.aspx");
            }
        }

        protected void CancelPersonalDetails_Click(object sender, EventArgs e)
        {
            // Reload profile data to reset changes
            LoadProfileData();

            // Disable text fields without saving
            txtName.Enabled = false;
            txtCity.Enabled = false;
            txtDOB.Enabled = false;

            // Hide Save and Cancel buttons, show Edit button
            saveButton.Style.Add("display", "none");
            cancelButton.Style.Add("display", "none");
            editButton.Style.Add("display", "block");
        }

        protected void SaveContactDetails_Click(object sender, EventArgs e)
        {
            // Save Contact Details
            if (SaveContactDetails())
            {
                // Disable text fields after saving
                txtPhone.Enabled = false;

                // Hide Save and Cancel buttons, show Edit button
                saveButton2.Style.Add("display", "none");
                cancelButton2.Style.Add("display", "none");
                editButton2.Style.Add("display", "block");
                Response.Redirect("dashboard.aspx");
            }
        }

        protected void CancelContactDetails_Click(object sender, EventArgs e)
        {
            // Reload profile data to reset changes
            LoadProfileData();

            // Disable text fields without saving
            txtPhone.Enabled = false;

            // Hide Save and Cancel buttons, show Edit button
            saveButton2.Style.Add("display", "none");
            cancelButton2.Style.Add("display", "none");
            editButton2.Style.Add("display", "block");
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
                        txtName.Text = reader["FullName"].ToString();
                        txtPhone.Text = reader["PhoneNumber"].ToString();
                        txtCity.Text = reader["City"].ToString();
                        txtDOB.Text = Convert.ToDateTime(reader["BirthDate"]).ToString("yyyy-MM-dd");
                    }
                }
            }
        }

        private bool SavePersonalDetails()
        {
            string username = Session["Username"].ToString();
            string name = txtName.Text;
            string address = txtCity.Text;
            DateTime dob;
            if (!DateTime.TryParse(txtDOB.Text, out dob))
            {
                // Handle invalid date format here
                lblError.Text = "Invalid Date of Birth format. Please use yyyy-MM-dd format.";
                return false;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET FullName = @Name, City = @City, BirthDate = @DOB WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@City", address);
                    command.Parameters.AddWithValue("@DOB", dob);
                    command.Parameters.AddWithValue("@Username", username);

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

        private bool SaveContactDetails()
        {
            string username = Session["Username"].ToString();
            string phone = txtPhone.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET PhoneNumber = @Phone WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Username", username);

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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}
