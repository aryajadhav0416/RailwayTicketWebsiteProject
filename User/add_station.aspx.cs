using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class add_station : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindLineDropdown();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void BindLineDropdown()
        {
            DataTable lines = GetLines();
            ddlLineId.DataSource = lines;
            ddlLineId.DataTextField = "LineName";
            ddlLineId.DataValueField = "LineId";
            ddlLineId.DataBind();
            ddlLineId.Items.Insert(0, new ListItem("Select Line", "0"));
        }

        private DataTable GetLines()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT line_id AS LineId, line_name AS LineName FROM Line";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        protected void btnSaveStation_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string stationName = txtStationName.Text.Trim();
                    string stationCode = txtStationCode.Text.Trim();
                    int lineId = int.Parse(ddlLineId.SelectedValue);
                    float latitude = float.Parse(txtLatitude.Text.Trim());
                    float longitude = float.Parse(txtLongitude.Text.Trim());
                    int branch = int.Parse(ddlBranch.SelectedValue);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO 
                        Station (
                        station_name, station_code,
                        line_id, latitude,
                        longitude, is_branch) 
                        VALUES (
                        @StationName, @StationCode, 
                        @LineId, @Latitude, 
                        @Longitude, @Branch)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@StationName", stationName);
                            cmd.Parameters.AddWithValue("@StationCode", stationCode);
                            cmd.Parameters.AddWithValue("@LineId", lineId);
                            cmd.Parameters.AddWithValue("@Latitude", latitude);
                            cmd.Parameters.AddWithValue("@Longitude", longitude);
                            cmd.Parameters.AddWithValue("@Branch", branch);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    // Clear form and show success message
                    txtStationName.Text = "";
                    txtStationCode.Text = "";
                    ddlLineId.SelectedIndex = 0;
                    ddlBranch.SelectedIndex = 0;
                    txtLatitude.Text = "";
                    txtLongitude.Text = "";
                    lblError.Text = "Station added successfully.";
                    Response.Redirect("station_list.aspx");
                    lblError.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("station_list.aspx"); // Redirect to the station list page or any other relevant page
        }
    }
}
