using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class edit_station : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadStationDetails();
                    BindLineDropdown(); // Bind line dropdown when page loads
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void LoadStationDetails()
        {
            if (Session["SelectedStationId"] != null)
            {
                int stationId = (int)Session["SelectedStationId"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT station_name, station_code, longitude, latitude, line_id, is_branch FROM Station WHERE station_id = @StationId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StationId", stationId);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtStationId.Text = stationId.ToString();
                            txtStationName.Text = reader["station_name"].ToString();
                            txtStationCode.Text = reader["station_code"].ToString();
                            txtLongitude.Text = reader["longitude"].ToString();
                            txtLatitude.Text = reader["latitude"].ToString();
                            ddlLineId.SelectedValue = reader["line_id"].ToString();
                            ddlBranch.SelectedValue = reader["is_branch"].ToString();
                        }
                        reader.Close();
                    }
                }
            }
            else
            {
                Response.Redirect("station_list.aspx");
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["SelectedStationId"] != null)
            {
                int stationId = (int)Session["SelectedStationId"];
                string stationName = txtStationName.Text.Trim();
                string stationCode = txtStationCode.Text.Trim();
                string longitude = txtLongitude.Text.Trim();
                string latitude = txtLatitude.Text.Trim();
                int lineId = int.Parse(ddlLineId.SelectedValue);
                int branch = int.Parse(ddlBranch.SelectedValue);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Station 
                                     SET station_name = @StationName, 
                                         station_code = @StationCode, 
                                         longitude = @Longitude, 
                                         latitude = @Latitude,
                                         line_id = @LineId,
                                         is_branch = @Branch 
                                     WHERE station_id = @StationId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StationId", stationId);
                        cmd.Parameters.AddWithValue("@StationName", stationName);
                        cmd.Parameters.AddWithValue("@StationCode", stationCode);
                        cmd.Parameters.AddWithValue("@Longitude", longitude);
                        cmd.Parameters.AddWithValue("@Latitude", latitude);
                        cmd.Parameters.AddWithValue("@LineId", lineId);
                        cmd.Parameters.AddWithValue("@Branch", branch);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Redirect to station list after successful update
                Response.Redirect("station_list.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect to station list without saving
            Response.Redirect("station_list.aspx");
        }
    }
}
