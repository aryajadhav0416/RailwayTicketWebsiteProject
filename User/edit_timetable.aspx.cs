using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class edit_timetable : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    if (Session["SelectedTrainName"] != null && Session["SourceStationId"] != null
                        && Session["DestinationStationId"] != null && Session["lineId"] != null
                        && Session["SelectedTrainCondtioned"] != null && Session["SelectedTrainType"] != null
                        && Session["OriginTime"] != null && Session["SelectedTrainId"] != null)
                    {
                        LoadStations(Session["lineId"].ToString());
                        LoadData();
                        if (ddlSourceStationId.SelectedValue != "0" && ddlDestinationStationId.SelectedValue != "0" && !string.IsNullOrEmpty(txtOriginTime.Text))
                        {
                            LoadTimetable();
                        }
                        else
                        {
                            pnlTimetable.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("train_list.aspx");
                    }
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }
        private void LoadData()
        {
            txtTrainName.Text = Session["SelectedTrainName"].ToString();
            ddlSourceStationId.SelectedValue = Session["SourceStationId"].ToString();
            ddlDestinationStationId.SelectedValue = Session["DestinationStationId"].ToString();
            ddltrainAC.SelectedValue = Session["SelectedTrainCondtioned"].ToString();
            ddlTrainType.SelectedValue = Session["SelectedTrainType"].ToString();
            txtOriginTime.Text = Session["OriginTime"].ToString();
        }

        private void LoadStations(string lineId)
        {
            DataTable dt = GetStations(lineId);
            ddlSourceStationId.DataSource = dt;
            ddlSourceStationId.DataTextField = "station_name";
            ddlSourceStationId.DataValueField = "station_id";
            ddlSourceStationId.DataBind();
            ddlSourceStationId.Items.Insert(0, new ListItem("Select Source", "0"));

            ddlDestinationStationId.DataSource = dt;
            ddlDestinationStationId.DataTextField = "station_name";
            ddlDestinationStationId.DataValueField = "station_id";
            ddlDestinationStationId.DataBind();
            ddlDestinationStationId.Items.Insert(0, new ListItem("Select Destination", "0"));
        }

        private DataTable GetStations(string lineId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT station_id, station_name FROM Station WHERE line_id = @LineId", con))
                {
                    cmd.Parameters.AddWithValue("@LineId", lineId);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private void LoadTimetable()
        {
            int routeId = CalculateRouteId(Convert.ToInt32(Session["SourceStationId"].ToString()), Convert.ToInt32(Session["DestinationStationId"].ToString()), Session["OriginTime"].ToString());

            if (routeId != -1)
            {
                DataTable dt = GetTimetable(routeId);

                if (dt.Rows.Count > 0)
                {
                    gvTimetable.DataSource = dt;
                    gvTimetable.DataBind();
                    lblError.Visible = false; // Hide the error message
                    pnlTimetable.Visible = true; // Show the panel with the timetable and buttons
                }
                else
                {
                    gvTimetable.DataSource = null;
                    gvTimetable.DataBind();
                    lblError.Text = "No timetable found for the selected route.";
                    lblError.Visible = true;
                    pnlTimetable.Visible = false; // Hide the panel if no timetable is found
                }
            }
            else
            {
                gvTimetable.DataSource = null;
                gvTimetable.DataBind();
                lblError.Text = "Train wasn't found for the specific details.";
                lblError.Visible = true;
                pnlTimetable.Visible = false; // Hide the panel if no train is found
            }
        }

        private int CalculateRouteId(int sourceStationId, int destinationStationId, string originTime)
        {
            int routeId = -1; // Default value if not found
            string query = @"
        SELECT 
            r.route_id
        FROM 
            RouteStop rs
        JOIN 
            Route r ON rs.route_id = r.route_id
        JOIN 
            Train t ON r.train_id = t.train_id
        WHERE 
            t.src_station_id = @SourceStationId
            AND t.dest_station_id = @DestinationStationId
            AND rs.departure_time = @OriginTime
            AND rs.sequence = 1;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SourceStationId", sourceStationId);
                    cmd.Parameters.AddWithValue("@DestinationStationId", destinationStationId);
                    cmd.Parameters.AddWithValue("@OriginTime", originTime); // Ensure this matches the format in your DB

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        routeId = Convert.ToInt32(result);
                    }
                }
            }

            return routeId;
        }

        private DataTable GetTimetable(int routeId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Routestop WHERE route_id = @RouteId", con))
                {
                    cmd.Parameters.AddWithValue("@RouteId", routeId);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        protected void gvTimetable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlStationId = (DropDownList)e.Row.FindControl("ddlStationId");
                DataTable dt = GetStations((Session["lineId"].ToString()));
                ddlStationId.DataSource = dt;
                ddlStationId.DataTextField = "station_name";
                ddlStationId.DataValueField = "station_id";
                ddlStationId.DataBind();

                ddlStationId.SelectedValue = DataBinder.Eval(e.Row.DataItem, "station_id").ToString();
            }
        }

        protected void btnAddStop_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("routestop_id");
            dt.Columns.Add("station_id");
            dt.Columns.Add("arrival_time");
            dt.Columns.Add("departure_time");
            dt.Columns.Add("sequence");
            dt.Columns.Add("plat_no");

            foreach (GridViewRow row in gvTimetable.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["routestop_id"] = ((Label)row.FindControl("RouteId")).Text;
                dr["station_id"] = ((DropDownList)row.FindControl("ddlStationId")).SelectedValue;
                dr["arrival_time"] = ((TextBox)row.FindControl("txtArrivalTime")).Text;
                dr["departure_time"] = ((TextBox)row.FindControl("txtDepartureTime")).Text;
                dr["sequence"] = ((TextBox)row.FindControl("txtSequence")).Text;
                dr["plat_no"] = ((TextBox)row.FindControl("txtPlatform")).Text;
                dt.Rows.Add(dr);
            }

            // Add a new empty row for user to input new stop details
            DataRow newRow = dt.NewRow();
            //newRow["routestop_id"] = "NEW"; Special identifier for new rows
            dt.Rows.Add(newRow);

            gvTimetable.DataSource = dt;
            gvTimetable.DataBind();
        }

        protected void btnRemoveStop_Click(object sender, EventArgs e)
        {
            // Try to retrieve the DataTable from ViewState
            DataTable dt = ViewState["Timetable"] as DataTable;

            // Ensure the DataTable is not null
            if (dt == null)
            {
                // Initialize the DataTable if it's null
                dt = new DataTable();
                // Add the required columns based on your GridView structure
                dt.Columns.Add("routestop_id");
                dt.Columns.Add("station_id");
                dt.Columns.Add("arrival_time");
                dt.Columns.Add("departure_time");
                dt.Columns.Add("sequence");
                dt.Columns.Add("plat_no");

                // Populate DataTable with current GridView data
                foreach (GridViewRow row in gvTimetable.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["routestop_id"] = ((Label)row.FindControl("RouteId")).Text;
                    dr["station_id"] = ((DropDownList)row.FindControl("ddlStationId")).SelectedValue;
                    dr["arrival_time"] = ((TextBox)row.FindControl("txtArrivalTime")).Text;
                    dr["departure_time"] = ((TextBox)row.FindControl("txtDepartureTime")).Text;
                    dr["sequence"] = ((TextBox)row.FindControl("txtSequence")).Text;
                    dr["plat_no"] = ((TextBox)row.FindControl("txtPlatform")).Text;
                    dt.Rows.Add(dr);
                }
            }

            List<int> routeStopIdsToRemove = new List<int>();
            List<int> newRowsToRemove = new List<int>();

            // Loop through each row in the GridView
            foreach (GridViewRow row in gvTimetable.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    Label lblRouteId = (Label)row.FindControl("RouteId");
                    if (!string.IsNullOrEmpty(lblRouteId.Text) && lblRouteId.Text != "NEW")
                    {
                        int routestopId;
                        if (int.TryParse(lblRouteId.Text, out routestopId))
                        {
                            routeStopIdsToRemove.Add(routestopId);
                        }
                    }
                    else
                    {
                        // Track newly added rows that need to be removed
                        newRowsToRemove.Add(row.RowIndex);
                    }
                }
            }

            // Remove newly added rows from DataTable (not saved to database)
            newRowsToRemove.Reverse(); // Remove rows from the end to avoid index shifting
            foreach (int rowIndex in newRowsToRemove)
            {
                if (rowIndex < dt.Rows.Count)
                {
                    dt.Rows.RemoveAt(rowIndex);
                }
            }

            // Re-bind the updated DataTable to the GridView
            gvTimetable.DataSource = dt;
            gvTimetable.DataBind();

            // Update ViewState
            ViewState["Timetable"] = dt;

            // Remove the database entries for the selected rows that exist in the database
            if (routeStopIdsToRemove.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (int routestopId in routeStopIdsToRemove)
                            {
                                string query = "DELETE FROM RouteStop WHERE routestop_id = @RoutestopId";
                                using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@RoutestopId", routestopId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // Reload the timetable after the delete operation
                LoadTimetable();
            }
        }

        protected void btnSaveTimetable_Click(object sender, EventArgs e)
        {
            bool allEntriesValid = true;
            foreach (GridViewRow row in gvTimetable.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                // Only process the row if the checkbox is ticked
                if (chkSelect.Checked)
                {
                    Label RouteId = (Label)row.FindControl("RouteId");
                    DropDownList ddlStationId = (DropDownList)row.FindControl("ddlStationId");
                    TextBox txtArrivalTime = (TextBox)row.FindControl("txtArrivalTime");
                    TextBox txtDepartureTime = (TextBox)row.FindControl("txtDepartureTime");
                    TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                    TextBox txtPlatform = (TextBox)row.FindControl("txtPlatform");

                    int station = Convert.ToInt32(ddlStationId.SelectedValue);
                    int platform = Convert.ToInt32(txtPlatform.Text);
                    DateTime arrival = Convert.ToDateTime(txtArrivalTime.Text);
                    DateTime departure = Convert.ToDateTime(txtDepartureTime.Text);

                    // Platform availability check
                    if (!IsPlatformAvailable(station, platform, arrival, departure))
                    {
                        allEntriesValid = false;
                        lblError.Visible = true;
                        lblError.Text = $"Platform {platform} at {station} is already occupied during {arrival.ToShortTimeString()} - {departure.ToShortTimeString()}. Please update this entry.";
                        break; // Exit the loop on the first invalid entry
                    }
                }
            }

            if (allEntriesValid)
            {
                // Save timetable logic for all valid rows
                SaveSelectedTimetables();
                SaveTrainDetails();
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Timetable saved successfully!";
                Response.Redirect("train_list.aspx");
            }
        }

        private bool IsPlatformAvailable(int stationId, int platformNo, DateTime arrivalTime, DateTime departureTime)
        {
            bool isAvailable = true;
            int routeId = CalculateRouteId(Convert.ToInt32(ddlSourceStationId.SelectedValue), Convert.ToInt32(ddlDestinationStationId.SelectedValue), txtOriginTime.Text);
            // Define the time buffer (e.g., 3 minutes before and after the selected train's arrival and departure times)
            TimeSpan timeBuffer = TimeSpan.FromMinutes(3);
            TimeSpan arrivalTimeOnly = arrivalTime.TimeOfDay;
            TimeSpan departureTimeOnly = departureTime.TimeOfDay;
            // Query to check if any train is scheduled on the same platform within the defined time buffer
            string query = @"
        SELECT COUNT(*)
        FROM RouteStop
        WHERE station_id = @Station
        AND plat_no = @PlatformNumber
        AND route_id != @RouteId
        AND ((@ArrivalTime BETWEEN DATEADD(minute, -3, arrival_time) AND DATEADD(minute, 3, departure_time)) 
            OR (@DepartureTime BETWEEN DATEADD(minute, -3, arrival_time) AND DATEADD(minute, 3, departure_time)))";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Station", stationId);
                    cmd.Parameters.AddWithValue("@RouteId", routeId);
                    cmd.Parameters.AddWithValue("@PlatformNumber", platformNo);
                    cmd.Parameters.AddWithValue("@ArrivalTime", arrivalTimeOnly);
                    cmd.Parameters.AddWithValue("@DepartureTime", departureTimeOnly);

                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        isAvailable = false; // If any train is found in the time range, the platform is not available
                    }
                }
            }
            return isAvailable;
        }
        private void SaveTrainDetails()
        {
            string trainName = txtTrainName.Text;
            int sourceStationId = Convert.ToInt32(ddlSourceStationId.SelectedValue);
            int destinationStationId = Convert.ToInt32(ddlDestinationStationId.SelectedValue);
            string isAC = Convert.ToString(ddltrainAC.SelectedValue);
            string trainType = ddlTrainType.SelectedValue;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Train 
                         SET train_name = @TrainName, src_station_id = @SourceStationId, dest_station_id = @DestinationStationId, 
                             ac_nonac = @IsAC, train_type = @TrainType 
                         WHERE train_id = @TrainId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrainName", trainName);
                    cmd.Parameters.AddWithValue("@SourceStationId", sourceStationId);
                    cmd.Parameters.AddWithValue("@DestinationStationId", destinationStationId);
                    cmd.Parameters.AddWithValue("@IsAC", isAC);
                    cmd.Parameters.AddWithValue("@TrainType", trainType);
                    cmd.Parameters.AddWithValue("@TrainId", Session["SelectedTrainId"]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Route 
                         SET route_name = @TrainName 
                         WHERE train_id = @TrainId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TrainName", trainName);
                    cmd.Parameters.AddWithValue("@TrainId", Session["SelectedTrainId"]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void SaveSelectedTimetables()
        {
            int routeId = CalculateRouteId(Convert.ToInt32(ddlSourceStationId.SelectedValue), Convert.ToInt32(ddlDestinationStationId.SelectedValue), txtOriginTime.Text);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (GridViewRow row in gvTimetable.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    if (chkSelect.Checked)
                    {
                        string routestopId = ((Label)row.FindControl("RouteId")).Text;
                        DropDownList ddlStationId = (DropDownList)row.FindControl("ddlStationId");
                        TextBox txtArrivalTime = (TextBox)row.FindControl("txtArrivalTime");
                        TextBox txtDepartureTime = (TextBox)row.FindControl("txtDepartureTime");
                        TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                        TextBox txtPlatform = (TextBox)row.FindControl("txtPlatform");

                        string query= "IF EXISTS (SELECT 1 FROM RouteStop WHERE routestop_id = @RoutestopId) " +
                                       "UPDATE RouteStop SET station_id = @StationId, arrival_time = @ArrivalTime, departure_time = @DepartureTime, sequence = @Sequence, plat_no = @PlatformNo WHERE routestop_id = @RoutestopId " +
                                       "ELSE " +
                                       "INSERT INTO RouteStop (route_id, station_id, arrival_time, departure_time, sequence, plat_no) " +
                                       "VALUES (@RouteId, @StationId, @ArrivalTime, @DepartureTime, @Sequence, @PlatformNo)";
                        
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@RoutestopId", routestopId);
                            cmd.Parameters.AddWithValue("@RouteId", routeId);
                            cmd.Parameters.AddWithValue("@StationId", Convert.ToInt32(ddlStationId.SelectedValue));
                            cmd.Parameters.AddWithValue("@ArrivalTime", Convert.ToDateTime(txtArrivalTime.Text));
                            cmd.Parameters.AddWithValue("@DepartureTime", Convert.ToDateTime(txtDepartureTime.Text));
                            cmd.Parameters.AddWithValue("@Sequence", Convert.ToInt32(txtSequence.Text));
                            cmd.Parameters.AddWithValue("@PlatformNo", Convert.ToInt32(txtPlatform.Text));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}