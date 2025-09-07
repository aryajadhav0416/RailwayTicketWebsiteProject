using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class train_list : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindLineFilter();
                    BindTrainList();
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

        private void BindTrainList()
        {
            int selectedLineId = int.Parse(ddlLineFilter.SelectedValue);
            DataTable trains = GetTrains(selectedLineId);
            gvTrainList.DataSource = trains;
            gvTrainList.DataBind();
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

        private DataTable GetTrains(int lineId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        t.train_id, 
                        r.route_name, 
                        t.train_type,
                        t.ac_nonac,
                        l.line_name,
                        src.station_name AS src,
                        dest.station_name AS dest,
                        rs.departure_time
                    FROM Train t
                    INNER JOIN Line l ON t.line_id = l.line_id
                    INNER JOIN Station src ON t.src_station_id = src.station_id
                    INNER JOIN Station dest ON t.dest_station_id = dest.station_id
                    INNER JOIN Route r ON t.train_id = r.train_id
                    INNER JOIN RouteStop rs ON r.route_id = rs.route_id
                    WHERE rs.sequence = 1";

                if (lineId != 0)
                {
                    query += " AND t.line_id = @LineId";
                }
                query += " ORDER BY l.line_id,rs.departure_time";
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

        protected void ddlLineFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTrainList();
        }

        protected void gvTrainList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTrainList.PageIndex = e.NewPageIndex;
            BindTrainList();
        }

        private int CalculateLineId(string lineName)
        {
            int lineId = -1; // Default value if not found
            string query = @"
        SELECT 
            line_id
        FROM 
            Line
        WHERE 
            line_name = @LineName;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LineName", lineName); // Ensure this matches the format in your DB

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        lineId = Convert.ToInt32(result);
                    }
                }
            }

            return lineId;
        }

        private int CalculateStationId(string stationName)
        {
            int stationId = -1; // Default value if not found
            string query = @"
        SELECT 
            station_id
        FROM 
            Station
        WHERE 
            station_name = @StationName;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StationName", stationName); // Ensure this matches the format in your DB

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        stationId = Convert.ToInt32(result);
                    }
                }
            }

            return stationId;
        }

        protected void gvTrainList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            // Get the GridViewRow based on the row index.
            GridViewRow row = gvTrainList.Rows[rowIndex];

            // Retrieve all the cell values in the selected row.
            string trainId = ((Label)row.FindControl("TrainID")).Text;
            string trainName = ((Label)row.FindControl("TrainName")).Text;
            string trainType = ((Label)row.FindControl("TrainType")).Text;
            string acNonAc = ((Label)row.FindControl("trainCondtioned")).Text;
            string lineName = ((Label)row.FindControl("linename")).Text;
            string srcStation = ((Label)row.FindControl("srcstn")).Text;
            string destStation = ((Label)row.FindControl("deststn")).Text;
            string departureTime = ((Label)row.FindControl("ArrivalTime")).Text;

            // Concatenate all the values into a single string.
            string rowData = $"{trainId}, {trainName}, {trainType}, {acNonAc}, {lineName}, {srcStation}, {destStation}, {departureTime}";
            int trainIdValue;
            int lineId= CalculateLineId(lineName);
            int srcStationId = CalculateStationId(srcStation);
            int destStationId = CalculateStationId(destStation);
            if (int.TryParse(trainId, out trainIdValue))
            {
                Session["SelectedTrainId"] = trainIdValue;
            }
            Session["SelectedTrainCondtioned"] = acNonAc;
            Session["SelectedTrainType"] = trainType;
            Session["SelectedTrainName"] = trainName;
            Session["SourceStationId"] = srcStationId;
            Session["DestinationStationId"] = destStationId;
            Session["OriginTime"] = departureTime;
            Session["lineId"] = lineId;
            // Optionally, you can store this data in a session variable, redirect to another page, or use it as needed.
            Session["SelectedTrainDetails"] = rowData;
            Session["referringPage"] = "train_list.aspx";
            if (e.CommandName.Equals("ViewDetails"))
            {
                // Redirect to the details page with the train ID.
                Response.Redirect($"traindetails.aspx?train_id={trainId}");
            }
            if (e.CommandName.Equals("EditDetails"))
            {
                // Redirect to the details page with the train ID.
                Response.Redirect($"edit_timetable.aspx?train_id={trainId}");
            }
            if (e.CommandName.Equals("DeleteDetails"))
            {
                // Redirect to the details page with the train ID.
                Response.Redirect($"remove_timetable.aspx?train_id={trainId}");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admindashboard.aspx");
        }
        protected void btnAddNewRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_timetable.aspx");
        }
    }
}
