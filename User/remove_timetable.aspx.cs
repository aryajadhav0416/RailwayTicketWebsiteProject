using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class remove_timetable : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedTrainName"] != null && Session["SourceStationId"] != null
                    && Session["DestinationStationId"] != null && Session["lineId"] != null
                    && Session["SelectedTrainCondtioned"] != null && Session["SelectedTrainType"] != null
                    && Session["OriginTime"] != null && Session["SelectedTrainId"] != null && Session["Username"]!=null)
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
                    Response.Redirect("admindashboard.aspx");
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

        protected void btnSearchTrain_Click(object sender, EventArgs e)
        {
            LoadTimetable();
        }

        private void LoadTimetable()
        {
            int routeId = CalculateRouteId(Convert.ToInt32(Session["SourceStationId"].ToString()), Convert.ToInt32(Session["DestinationStationId"].ToString()), Session["OriginTime"].ToString());
            int trainId = CalculateTrainId(routeId);
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
            ViewState["RouteId"] = routeId; // Store the RouteId for deletion
            ViewState["TrainId"] = trainId;
        }

        private DataTable GetTimetable(int routeId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT 
                rs.route_id,
                s.station_name,
                rs.arrival_time,
                rs.departure_time,
                rs.plat_no,
                rs.sequence
            FROM 
                RouteStop rs
            JOIN 
                Station s ON rs.station_id = s.station_id
            WHERE 
                rs.route_id = @RouteId;", con))
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

        protected void btnDeleteTimetable_Click(object sender, EventArgs e)
        {
            if (ViewState["RouteId"] != null && ViewState["TrainId"] != null)
            {
                int routeId = (int)ViewState["RouteId"];
                int trainId = (int)ViewState["TrainId"];
                DeleteTimetableEntries(routeId, trainId);
                Response.Redirect(Session["ReferringPage"].ToString()); // Reload the timetable after deletion
            }
        }

        private void DeleteTimetableEntries(int routeId, int trainId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Delete from RouteStop
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM RouteStop WHERE route_id = @RouteId", con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RouteId", routeId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from Route
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Route WHERE route_id = @RouteId", con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RouteId", routeId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from Train
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Train WHERE train_id = @TrainId", con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TrainId", trainId);
                            cmd.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on error
                        transaction.Rollback();
                        // Handle or log the error
                        // e.g., lblError.Text = "Error deleting timetable entries: " + ex.Message;
                    }
                }
            }
        }


        private int CalculateRouteId(int sourceStationId, int destinationStationId, string originTime)
        {
            int routeId = -1;
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
            AND rs.sequence = 1;"; // Adjust the query according to your database specifics

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SourceStationId", sourceStationId);
                    cmd.Parameters.AddWithValue("@DestinationStationId", destinationStationId);
                    cmd.Parameters.AddWithValue("@OriginTime", originTime);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    routeId = result != DBNull.Value ? Convert.ToInt32(result) : -1;
                }
            }
            return routeId;
        }

        private int CalculateTrainId(int routeId)
        {
            int trainId = -1;
            string query = @"
                SELECT train_id 
                from Route
                WHERE
                route_id=@routeId";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@routeId", routeId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    trainId = result != DBNull.Value ? Convert.ToInt32(result) : -1;
                }
            }
            return trainId;
        }
    }
}
