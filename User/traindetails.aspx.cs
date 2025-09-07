using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace RailwayTicketWebsite.User
{
    public partial class traindetails : System.Web.UI.Page
    {
        private string ConnString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if train details are stored in session
                if (Session["SelectedTrainId"] != null && Session["SelectedTrainName"] != null)
                {
                    int trainId = Convert.ToInt32(Session["SelectedTrainId"]);
                    string trainName = Session["SelectedTrainName"].ToString();

                    // Load train details
                    LoadTrainDetails(trainId, trainName);

                    // Load station details
                    LoadStationDetails(trainId);
                }
                else
                {
                    if (Session["ReferringPage"] != null)
                    {
                        // Redirect if session variables are not set
                        Response.Redirect(Session["ReferringPage"].ToString());
                    }
                    else
                    {
                        Response.Redirect("userloginpg.aspx");
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect back to the previous page
            string referringPage = Session["ReferringPage"] as string;

            if (!string.IsNullOrEmpty(referringPage))
            {
                Response.Redirect(referringPage);
            }
        }

        private void LoadTrainDetails(int trainId, string trainName)
        {
            lblTrainName.Text = trainName;
        }

        private void LoadStationDetails(int trainId)
        {
            DataTable stationDetailsTable = new DataTable();

            string query = @"
                SELECT
                    S.station_name,
                    RS.arrival_time,
                    RS.departure_time,
RS.plat_no
                FROM
                    Train T
                JOIN
                    Route R ON T.train_id = R.train_id
                JOIN
                    RouteStop RS ON R.route_id = RS.route_id
                JOIN
                    Station S ON RS.station_id = S.station_id
                WHERE
                    T.train_id = @TrainId
                ORDER BY
                    RS.sequence;";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TrainId", trainId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(stationDetailsTable);
                }
            }

            rptStationDetails.DataSource = stationDetailsTable;
            rptStationDetails.DataBind();
        }
    }
}
