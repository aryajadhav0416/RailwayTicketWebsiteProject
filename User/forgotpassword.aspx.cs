using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class forgotpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void GenerateOTP_Click(object sender, EventArgs e)
        {
            string username = uname.Text;

            if (ValidateUser(username))
            {
                Session["Username"] = uname.Text;
                Response.Redirect("settingforgottenpassword.aspx");
            }
            else
            {
                debugLabel.Text = "Invalid username";
            }
        }


        private bool ValidateUser(string username)
        {
            // Replace the connection string with your actual connection string
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Role!='Admin'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                int userCount = (int)command.ExecuteScalar();

                return userCount > 0;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}
