using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class add_fareslab : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // No special handling needed on page load for this simple add page.
            if (Session["Username"] == null)
            {
                Response.Redirect("userloginpg.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string query = "INSERT INTO FareSlab (MinDistance, MaxDistance, SecondClassFare, FirstClassFare, ACFare) " +
                               "VALUES (@MinDistance, @MaxDistance, @SecondClassFare, @FirstClassFare, @ACFare)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MinDistance", float.Parse(txtMinDistance.Text));
                    cmd.Parameters.AddWithValue("@MaxDistance", float.Parse(txtMaxDistance.Text));
                    cmd.Parameters.AddWithValue("@SecondClassFare", float.Parse(txtSecondClassFare.Text));
                    cmd.Parameters.AddWithValue("@FirstClassFare", float.Parse(txtFirstClassFare.Text));
                    cmd.Parameters.AddWithValue("@ACFare", float.Parse(txtACFare.Text));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("fareslab_list.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("fareslab_list.aspx");
        }
    }
}
