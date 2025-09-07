using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class edit_fareslab : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    int fareSlabID;
                    if (int.TryParse(Session["FareSlabID"].ToString(), out fareSlabID))
                    {
                        LoadFareSlabDetails(fareSlabID);
                    }
                    else
                    {
                        // Handle invalid FareSlabID
                        Response.Redirect("fareslab_list.aspx");
                    }
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void LoadFareSlabDetails(int fareSlabID)
        {
            string query = "SELECT * FROM FareSlab WHERE FareSlabID = @FareSlabID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FareSlabID", fareSlabID);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtMinDistance.Text = reader["MinDistance"].ToString();
                        txtMaxDistance.Text = reader["MaxDistance"].ToString();
                        txtSecondClassFare.Text = reader["SecondClassFare"].ToString();
                        txtFirstClassFare.Text = reader["FirstClassFare"].ToString();
                        txtACFare.Text = reader["ACFare"].ToString();
                    }
                    else
                    {
                        // Handle case where FareSlabID is not found
                        Response.Redirect("fareslab_list.aspx");
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int fareSlabID;
            if (int.TryParse(Request.QueryString["FareSlabID"], out fareSlabID))
            {
                string query = "UPDATE FareSlab SET MinDistance = @MinDistance, MaxDistance = @MaxDistance, " +
                               "SecondClassFare = @SecondClassFare, FirstClassFare = @FirstClassFare, " +
                               "ACFare = @ACFare WHERE FareSlabID = @FareSlabID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MinDistance", float.Parse(txtMinDistance.Text));
                    cmd.Parameters.AddWithValue("@MaxDistance", float.Parse(txtMaxDistance.Text));
                    cmd.Parameters.AddWithValue("@SecondClassFare", float.Parse(txtSecondClassFare.Text));
                    cmd.Parameters.AddWithValue("@FirstClassFare", float.Parse(txtFirstClassFare.Text));
                    cmd.Parameters.AddWithValue("@ACFare", float.Parse(txtACFare.Text));
                    cmd.Parameters.AddWithValue("@FareSlabID", fareSlabID);

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
