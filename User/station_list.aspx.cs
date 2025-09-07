using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class station_list : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindStationList();
                    BindLineFilter();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }
        private void BindLineFilter()
        {
            DataTable lines = GetLines();
            ddlLineFilter.DataSource = lines;
            ddlLineFilter.DataTextField = "LineName";
            ddlLineFilter.DataValueField = "LineId";
            ddlLineFilter.DataBind();
            ddlLineFilter.Items.Insert(0, new ListItem("All", "0")); // Add "All" option
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
        private void BindStationList()
        {
            int selectedLineId = int.Parse(ddlLineFilter.SelectedValue);
            DataTable stations = GetStations(selectedLineId);
            gvStationList.DataSource = stations;
            gvStationList.DataBind();
        }

        protected void ddlLineFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStationList();
        }

        private DataTable GetStations(int lineId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                s.station_id, 
                s.station_name,
                s.station_code,
                l.line_name,
                s.longitude,
                s.latitude,
                s.is_branch
                FROM Station s
                INNER JOIN Line l ON s.line_id = l.line_id";
                if (lineId != 0)
                {
                    query += " AND s.line_id = @LineId";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (lineId != 0)
                    {
                        cmd.Parameters.AddWithValue("@LineId", lineId);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        protected void gvStationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStationList.PageIndex = e.NewPageIndex;
            BindStationList();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admindashboard.aspx");
        }
        protected void gvStationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditDetails"))
            {
                // Retrieve the row index stored in the CommandArgument property.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Get the GridViewRow based on the row index.
                GridViewRow row = gvStationList.Rows[rowIndex];

                // Retrieve all the cell values in the selected row.
                string stationId = ((Label)row.FindControl("StationID")).Text;
                string stationName = ((Label)row.FindControl("StationName")).Text;
                string stationCode = ((Label)row.FindControl("StationCode")).Text;
                // Concatenate all the values into a single string.
                int stationIdValue;
                if (int.TryParse(stationId, out stationIdValue))
                {
                    Session["SelectedStationId"] = stationIdValue;
                }
                Session["SelectedStationName"] = stationName;
                Session["SelectedStationCode"] = stationCode;
                // Redirect to the details page with the station ID.
                Response.Redirect($"edit_station.aspx?station_id={stationId}");
            }
        }
        protected void btnAddNewRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_station.aspx");
        }
    }
}
