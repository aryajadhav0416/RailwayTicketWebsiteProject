using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class edit_lineslab : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    // Retrieve the LineID from the session
                    if (Session["SelectedLineId"] != null)
                    {
                        int lineId = (int)Session["SelectedLineId"];
                        LoadLineDetails(lineId);
                    }
                    else
                    {
                        Response.Redirect("lineslab_list.aspx");
                    }
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void LoadLineDetails(int lineId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Line WHERE line_id = @LineID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LineID", lineId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtLineId.Text = reader["line_id"].ToString();
                        txtLineName.Text = reader["line_name"].ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["SelectedLineId"] != null)
                {
                    int lineId = (int)Session["SelectedLineId"];
                    UpdateLine(lineId);
                }
            }
        }

        private void UpdateLine(int lineId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Line
                                 SET line_name = @LineName
                                 WHERE line_id = @LineID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LineName", txtLineName.Text);
                    cmd.Parameters.AddWithValue("@LineID", lineId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            Session.Remove("SelectedLineId"); // Clear the session state after saving
            Response.Redirect("lineslab_list.aspx"); // Redirect after saving
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove("SelectedLineId"); // Clear the session state on cancel
            Response.Redirect("lineslab_list.aspx"); // Redirect to list without saving
        }
    }
}
