using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;

namespace RailwayTicketWebsite.User
{
    public partial class edit_routestop : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        public int lastRouteId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    pnlRouteStops.Visible = false;
                    pnlEditableTable.Visible = false;
                    LoadLineIds();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void btnGenerateTable_Click(object sender, EventArgs e)
        {
            int numberOfRows = int.Parse(txtNumberOfRows.Text);
            if (numberOfRows > 1)
            {
                GenerateEditableTable(numberOfRows);
            }
            else
            {
                lblError.Text = "Enter integer number or no. more than 1";
            }
        }

        private void GenerateEditableTable(int numberOfRows)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RouteID");
            dt.Columns.Add("StationID");
            dt.Columns.Add("ArrivalTime");
            dt.Columns.Add("DepartureTime");
            dt.Columns.Add("Sequence");
            dt.Columns.Add("PlatformNo");
            for (int i = 0; i < numberOfRows; i++)
            {
                dt.Rows.Add();
            }

            gvRouteStops.DataSource = dt;
            gvRouteStops.DataBind();
            pnlEditableTable.Visible = true;
        }

        protected void gvRouteStops_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlStationId = (DropDownList)e.Row.FindControl("ddlStationId");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT station_id, station_name FROM Station WHERE line_id = @LineID", con))
                    {
                        cmd.Parameters.AddWithValue("@LineID",ddlLineId.SelectedValue);
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlStationId.DataSource = reader;
                        ddlStationId.DataTextField = "station_name";
                        ddlStationId.DataValueField = "station_id";
                        ddlStationId.DataBind();
                        reader.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 route_id FROM Route ORDER BY route_id DESC", con))
                    {
                        lastRouteId = (int)cmd.ExecuteScalar();
                        con.Close();
                    }
                    // Set the HiddenField value to the lastRouteId

                    Label RouteId = (Label)e.Row.FindControl("RouteId");
                    RouteId.Text= lastRouteId.ToString();
                }
            }
        }

        protected void btnSaveRouteStops_Click(object sender, EventArgs e)
        {
            bool allEntriesValid = true;
            foreach (GridViewRow row in gvRouteStops.Rows)
            {
                Label RouteId = (Label)row.FindControl("RouteId");
                DropDownList ddlStationId = (DropDownList)row.FindControl("ddlStationId");
                TextBox txtArrivalTime = (TextBox)row.FindControl("txtArrivalTime");
                TextBox txtDepartureTime = (TextBox)row.FindControl("txtDepartureTime");
                TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                TextBox txtplatform = (TextBox)row.FindControl("txtPlatformNo");
                
                int station = Convert.ToInt32(ddlStationId.SelectedValue);
                int platform = Convert.ToInt32(txtplatform.Text);
                DateTime arrival = Convert.ToDateTime(txtArrivalTime.Text);
                DateTime departure = Convert.ToDateTime(txtDepartureTime.Text);
                if (!IsPlatformAvailable(station, platform, arrival, departure))
                {
                    allEntriesValid = false;
                    lblError.Text = $"Platform {platform} at {station} is already occupied during {arrival.ToShortTimeString()} - {departure.ToShortTimeString()}. Please update this entry.";
                    break; // Exit the loop on the first invalid entry
                }
            }

            if (allEntriesValid)
            {
                // Save timetable logic for all valid rows
                SaveAllTimetables();
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Timetable saved successfully!";
                Response.Redirect("train_list.aspx");
            }
        }

        private void SaveAllTimetables()
        {
            foreach (GridViewRow row in gvRouteStops.Rows)
            {
                Label RouteId = (Label)row.FindControl("RouteId");
                DropDownList ddlStationId = (DropDownList)row.FindControl("ddlStationId");
                TextBox txtArrivalTime = (TextBox)row.FindControl("txtArrivalTime");
                TextBox txtDepartureTime = (TextBox)row.FindControl("txtDepartureTime");
                TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                TextBox txtplatform = (TextBox)row.FindControl("txtPlatformNo");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO RouteStop (route_id, station_id, arrival_time, departure_time, sequence, plat_no) VALUES (@RouteId, @StationId, @ArrivalTime, @DepartureTime, @Sequence, @plat)", con))
                    {
                        cmd.Parameters.AddWithValue("@RouteId", RouteId.Text);
                        cmd.Parameters.AddWithValue("@StationId", ddlStationId.SelectedValue);
                        cmd.Parameters.AddWithValue("@ArrivalTime", txtArrivalTime.Text);
                        cmd.Parameters.AddWithValue("@DepartureTime", txtDepartureTime.Text);
                        cmd.Parameters.AddWithValue("@Sequence", txtSequence.Text);
                        cmd.Parameters.AddWithValue("@plat", txtplatform.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private bool IsPlatformAvailable(int station, int platformNumber, DateTime arrivalTime, DateTime departureTime)
        {
            bool isAvailable = true;

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
        AND ((@ArrivalTime BETWEEN DATEADD(minute, -3, arrival_time) AND DATEADD(minute, 3, departure_time)) 
            OR (@DepartureTime BETWEEN DATEADD(minute, -3, arrival_time) AND DATEADD(minute, 3, departure_time)))";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Station", station);
                    cmd.Parameters.AddWithValue("@PlatformNumber", platformNumber);
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

        private void LoadLineIds()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT line_id, line_name FROM Line", con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlLineId.DataSource = reader;
                    ddlLineId.DataTextField = "line_name";
                    ddlLineId.DataValueField = "line_id";
                    ddlLineId.DataBind();
                    reader.Close();
                }
            }

            ddlLineId.Items.Insert(0, new ListItem("--Select Line--", "0"));
        }

        protected void ddlLineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            int lineId = int.Parse(ddlLineId.SelectedValue);
            LoadStations(lineId);
        }

        private void LoadStations(int lineId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT station_id, station_name FROM Station WHERE line_id = @LineId", con))
                {
                    cmd.Parameters.AddWithValue("@LineId", lineId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlSrcStationId.DataSource = reader;
                    ddlSrcStationId.DataTextField = "station_name";
                    ddlSrcStationId.DataValueField = "station_id";
                    ddlSrcStationId.DataBind();
                    reader.Close();

                    // Reset and reload the DataReader for the destination station dropdown
                    reader = cmd.ExecuteReader();
                    ddlDestStationId.DataSource = reader;
                    ddlDestStationId.DataTextField = "station_name";
                    ddlDestStationId.DataValueField = "station_id";
                    ddlDestStationId.DataBind();
                    reader.Close();
                }
            }

            ddlSrcStationId.Items.Insert(0, new ListItem("--Select Source Station--", "0"));
            ddlDestStationId.Items.Insert(0, new ListItem("--Select Destination Station--", "0"));
        }

        protected void btnSaveTrain_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTrainName.Text) && ddlSrcStationId.SelectedValue != "0" && ddlLineId.SelectedValue != "0" &&
                    ddlDestStationId.SelectedValue != "0" && ddlTrainType.SelectedValue != "0" && ddlAcNonAc.SelectedValue != "0")
            {
                string trainName = txtTrainName.Text;
                int lineId = int.Parse(ddlLineId.SelectedValue);
                int srcStationId = int.Parse(ddlSrcStationId.SelectedValue);
                int destStationId = int.Parse(ddlDestStationId.SelectedValue);
                string trainType = ddlTrainType.SelectedValue;
                string acNonAc = ddlAcNonAc.SelectedValue;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            int trainId;

                            // Insert into Train table and retrieve the generated train_id
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Train (train_name, line_id, src_station_id, dest_station_id, train_type, ac_nonac) OUTPUT INSERTED.train_id VALUES (@TrainName, @LineId, @SrcStationId, @DestStationId, @TrainType, @AcNonAc)", con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@TrainName", trainName);
                                cmd.Parameters.AddWithValue("@LineId", lineId);
                                cmd.Parameters.AddWithValue("@SrcStationId", srcStationId);
                                cmd.Parameters.AddWithValue("@DestStationId", destStationId);
                                cmd.Parameters.AddWithValue("@TrainType", trainType);
                                cmd.Parameters.AddWithValue("@AcNonAc", acNonAc);

                                trainId = (int)cmd.ExecuteScalar();
                            }

                            // Insert into Route table with the generated train_id
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Route (route_name, train_id) VALUES (@RouteName, @TrainId)", con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@RouteName", trainName); // Assuming route_name is same as train_name
                                cmd.Parameters.AddWithValue("@TrainId", trainId);
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            // Show the Route Stops panel and hide the Save Train button
                            pnlRouteStops.Visible = true;
                            btnSaveTrain.Visible = false;
                            btnCancel.Visible = true; // Show the Cancel button
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            // Optionally log the error or show a message to the user
                            throw new Exception("An error occurred while saving the train details and route.", ex);
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "An error occurred while saving the train details and route.";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Retrieve the most recent train_id
                        int trainId;
                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 train_id FROM Train ORDER BY train_id DESC", con, transaction))
                        {
                            trainId = (int)cmd.ExecuteScalar();
                        }

                        // Delete from Route table
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Route WHERE train_id = @TrainId", con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TrainId", trainId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from Train table
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Train WHERE train_id = @TrainId", con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TrainId", trainId);
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Hide the Route Stops panel and Cancel button
                        pnlRouteStops.Visible = false;
                        btnSaveTrain.Visible = true;
                        btnCancel.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        // Optionally log the error or show a message to the user
                        throw new Exception("An error occurred while canceling the train details.", ex);
                    }
                }
            }
        }
    }
}
