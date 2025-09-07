using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class settingforgottenpassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial setup or reset on page load
                if (Session["Username"] == null)
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void TogglePasswordVisibility(object sender, EventArgs e)
        {
            // Toggle the password visibility
            if (pass.TextMode == TextBoxMode.Password)
            {
                pass.TextMode = TextBoxMode.SingleLine;
                btnTogglePassword.Text = "Hide";
            }
            else
            {
                pass.TextMode = TextBoxMode.Password;
                btnTogglePassword.Text = "Show";
            }

            // Maintain password value through postback
            pass.Attributes.Add("value", pass.Text);
        }

        protected void SubmitNewPassword_Click(object sender, EventArgs e)
        {
            string newPassword = pass.Text;
            string username = Session["Username"]?.ToString();

            if (!string.IsNullOrEmpty(username))
            {
                if (UpdatePassword(username, newPassword))
                {
                    // Password updated successfully
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('Password updated successfully.');", true);
                    Response.Redirect("userloginpg.aspx");
                }
                else
                {
                    // Error updating password
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('Error updating password.');", true);
                }
            }
            else
            {
                // Username is not available in the session
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('Username not found in session.');", true);
            }
        }

        private bool UpdatePassword(string username, string newPassword)
        {
            // Replace the connection string with your actual connection string
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET Pass = @Password WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Password", newPassword);
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, display error message, etc.)
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('An error occurred: {ex.Message}');", true);
                return false;
            }
        }
    }
}
