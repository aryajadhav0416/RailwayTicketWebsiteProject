using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Helpers;
using System.Web.UI;

namespace RailwayTicketWebsite.User
{
    public partial class userloginpg : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear the username and OTP fields on initial load
                uname.Text = string.Empty;
                pass.Text = string.Empty;
            }
        }

        protected void GenerateOTP_Click(object sender, EventArgs e)
        {
            string username = uname.Text;
            string password = pass.Text;

            if (ValidateUser(username, password))
            {
                string role = GetUserRole(username); // Get user role from database
                Session["Username"] = uname.Text;
                if (role.Equals("User"))
                {
                    Response.Redirect("dashboard.aspx");
                }
                else if (role.Equals("Admin"))
                {
                    Response.Redirect("admindashboard.aspx");
                }
            }
            else
            {
                debugLabel.Text = "Invalid username or password";
            }
        }

        private bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Pass FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string storedHashedPassword = reader["Pass"].ToString();
                    if (storedHashedPassword.Equals(password))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        private string GetUserRole(string username)
        {
            string role = ""; // Default role
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    role = reader["Role"].ToString();
                }
            }

            return role;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}
