using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite
{
    public partial class searchresult : System.Web.UI.Page
    {
        private string ConnString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure that session variables are not null
                if (Session["SourceStation"] != null && Session["DestinationStation"] != null)
                {
                    LoadTrainData("","",DateTime.Now.TimeOfDay); // Load all trains by default
                }
                else
                {
                    // Handle the case where session variables are not set, e.g., show an error message
                    // or redirect the user to a previous page
                    Response.Redirect("dashboard.aspx");
                }
            }
        }

        // Event handlers for different buttons
        protected void btnFast_Click(object sender, EventArgs e)
        {
            LoadTrainData("Fast", "",DateTime.Now.TimeOfDay);
        }

        protected void btnSlow_Click(object sender, EventArgs e)
        {
            LoadTrainData("Slow", "", DateTime.Now.TimeOfDay);
        }

        protected void btnAC_Click(object sender, EventArgs e)
        {
            LoadTrainData("", "AC", DateTime.Now.TimeOfDay);
        }

        protected void btnNextTrains_Click(object sender, EventArgs e)
        {
            LoadNextTrains(DateTime.Now.TimeOfDay);
        }

        // Load train data with optional filtering by train type
        private void LoadTrainData(string trainType, string acnon,TimeSpan currentTime)
        {
            // Retrieve source and destination station from session
            string sourceStation = Session["SourceStation"]?.ToString();
            string destinationStation = Session["DestinationStation"]?.ToString();

            string query = @"
                SELECT DISTINCT
                    t.train_id,
                    t.train_name,
                    t.train_type,
                    s_boarding.station_name AS source_station,
                    s_thane.station_name AS destination_station,
                    rs1.departure_time AS departure_time,
                    rs1.plat_no AS platform
                FROM
                    Train t
                JOIN
                    Route r1 ON t.train_id = r1.train_id
                JOIN
                    RouteStop rs1 ON r1.route_id = rs1.route_id
                JOIN
                    Station s_boarding ON rs1.station_id = s_boarding.station_id
                JOIN
                    Route r2 ON t.train_id = r2.train_id
                JOIN
                    RouteStop rs2 ON r2.route_id = rs2.route_id
                JOIN
                    Station s_thane ON rs2.station_id = s_thane.station_id
                WHERE
                    s_boarding.station_name = @SourceStation
                    AND s_thane.station_name = @DestinationStation
                    AND rs1.sequence < rs2.sequence
                    AND (@TrainType = '' OR t.train_type = @TrainType)
                    AND (@AC = '' OR t.ac_nonac = @AC)
                    AND rs1.departure_time > @CurrentTime
                ORDER BY
                    rs1.departure_time;";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("@SourceStation", sourceStation);
                    cmd.Parameters.AddWithValue("@DestinationStation", destinationStation);
                    cmd.Parameters.AddWithValue("@TrainType", trainType);
                    cmd.Parameters.AddWithValue("@AC", acnon);
                    cmd.Parameters.AddWithValue("@CurrentTime", currentTime);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind data to a repeater or other data control
                    rptTrainResults.DataSource = dt;
                    rptTrainResults.DataBind();
                }
            }
        }

        // Load next trains departing after the current time
        private void LoadNextTrains(TimeSpan currentTime)
        {
            // Retrieve source and destination station from session
            string sourceStation = Session["SourceStation"]?.ToString();
            string destinationStation = Session["DestinationStation"]?.ToString();

            string query = @"
                SELECT DISTINCT
                    t.train_id,
                    t.train_name,
                    t.train_type,
                    s_boarding.station_name AS source_station,
                    s_thane.station_name AS destination_station,
                    rs1.departure_time AS departure_time
                FROM
                    Train t
                JOIN
                    Route r1 ON t.train_id = r1.train_id
                JOIN
                    RouteStop rs1 ON r1.route_id = rs1.route_id
                JOIN
                    Station s_boarding ON rs1.station_id = s_boarding.station_id
                JOIN
                    Route r2 ON t.train_id = r2.train_id
                JOIN
                    RouteStop rs2 ON r2.route_id = rs2.route_id
                JOIN
                    Station s_thane ON rs2.station_id = s_thane.station_id
                WHERE
                    s_boarding.station_name = @SourceStation
                    AND s_thane.station_name = @DestinationStation
                    AND rs1.sequence < rs2.sequence
                    AND rs1.departure_time > @CurrentTime
                ORDER BY
                    rs1.departure_time;";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters for the SQL query
                    cmd.Parameters.AddWithValue("@SourceStation", sourceStation);
                    cmd.Parameters.AddWithValue("@DestinationStation", destinationStation);
                    cmd.Parameters.AddWithValue("@CurrentTime", currentTime);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind data to a repeater or other data control
                    rptTrainResults.DataSource = dt;
                    rptTrainResults.DataBind();
                }
            }
        }

        protected void rptTrainResults_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                int trainId = int.Parse(args[0]);
                string trainName = args[1];

                // Store trainId and trainName in session variables
                Session["SelectedTrainId"] = trainId;
                Session["SelectedTrainName"] = trainName;
                Session["ReferringPage"] = "searchresult.aspx";

                // Redirect to another page to display station names and arrival times
                Response.Redirect("traindetails.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("dashboard.aspx");
        }
    }
}
