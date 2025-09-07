using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class add_lineslab : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the LineID from the session
                if (Session["Username"] == null)
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveLine();
        }

        private void SaveLine()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Line(line_name) VALUES(@LineName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LineName", txtLineName.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("lineslab_list.aspx"); // Redirect after saving
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("lineslab_list.aspx"); // Redirect to list without saving
        }
    }
}