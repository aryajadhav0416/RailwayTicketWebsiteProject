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
    public partial class multilinepayment : System.Web.UI.Page
    {
        private List<string> stationCodes;
        double distance = 0;
        double fare = 0;
        string sourceStation = "";
        string destinationStation = "";
        string pathStr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["TransferStationCodes"] != null)
                {
                    stationCodes = (List<string>)Session["TransferStationCodes"];
                    pathStr = string.Join(" -> ", stationCodes);
                    lblViaRoute.Text = pathStr + "<br />";
                }
                else
                {
                    Response.Redirect("dashboard.aspx");
                }
                UpdateClassOptions();
                lblSourceStation.Text = Session["SourceStation"]?.ToString();
                lblDestinationStation.Text = Session["DestinationStation"]?.ToString();
                lblSummaryAdult.Text = ddlAdult.SelectedValue;
                lblSummaryTicketType.Text = ddlTicketType.SelectedValue;
                lblSummaryTrainType.Text = ddlTrainType.SelectedValue;
                lblSummaryClassType.Text = ddlClass.SelectedValue;
                lblSummaryPaymentType.Text = ddlPaymentType.SelectedValue;

                for (int i = 0; i < stationCodes.Count - 1; i++)
                {
                    string srcStation = stationCodes[i];
                    string destStation = stationCodes[i + 1];

                    var srcCoords = GetCoordinates(srcStation);
                    var destCoords = GetCoordinates(destStation);

                    distance += CalculateHaversineDistance(srcCoords.lat, srcCoords.lon, destCoords.lat, destCoords.lon);
                }
                Session["Distance"] = distance;
                sourceStation = Session["SourceStation"].ToString();
                destinationStation = Session["DestinationStation"].ToString();
                fare = CalculateFare(distance, lblSummaryTicketType.Text, lblSummaryTrainType.Text, lblSummaryClassType.Text, lblSummaryAdult.Text);
                lblTotalFare.Text = $"Fare between {sourceStation} and {destinationStation} is {fare:F2} Rs.";
            }
            else
            {
                stationCodes = (List<string>)Session["TransferStationCodes"];
                pathStr = string.Join(" -> ", stationCodes);
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateClassOptions();
            lblSummaryAdult.Text = ddlAdult.SelectedValue;
            lblSummaryTicketType.Text = ddlTicketType.SelectedValue;
            lblSummaryTrainType.Text = ddlTrainType.SelectedValue;
            lblSummaryClassType.Text = ddlClass.SelectedValue;
            lblSummaryPaymentType.Text = ddlPaymentType.SelectedValue;
            double distance = (double)Session["Distance"];
            fare = CalculateFare(distance, lblSummaryTicketType.Text, lblSummaryTrainType.Text, lblSummaryClassType.Text, lblSummaryAdult.Text);
            lblTotalFare.Text = $"Fare between {sourceStation} and {destinationStation} is {fare:F2} Rs.";
            // Additional logic to update total fare and wallet balance if needed
        }

        private void UpdateClassOptions()
        {
            ddlClass.Items.Clear();

            if (ddlTrainType.SelectedValue == "AC")
            {
                ddlClass.Items.Add(new ListItem("FIRST (I)", "I"));
            }
            else
            {
                ddlClass.Items.Add(new ListItem("SECOND (II)", "II"));
                ddlClass.Items.Add(new ListItem("FIRST (I)", "I"));
            }
        }

        private double CalculateFare(double distance, string ticketType, string trainType, string classType, string person)
        {
            double totalFare = 0;
            double secondclass = 0;
            double firstclass = 0;
            double ac = 0;
            double dist = distance;
            string ticket = ticketType;
            string train = trainType;
            string trainclass = classType;
            int per = Convert.ToInt32(person);
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SecondClassFare, FirstClassFare, ACFare FROM FareSlab WHERE @Distance BETWEEN MinDistance AND MaxDistance;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Distance", dist);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        secondclass = reader.GetDouble(0);
                        firstclass = reader.GetDouble(1);
                        ac = reader.GetDouble(2);
                    }
                }
            }

            if (train.Equals("O"))
            {
                if (trainclass.Equals("II"))
                {
                    totalFare += secondclass;
                }
                else if (trainclass.Equals("I"))
                {
                    totalFare += firstclass;
                }
            }
            else if (train.Equals("AC"))
            {
                totalFare += ac;
            }

            if (ticket.Equals("R"))
            {
                totalFare *= 2;
            }

            totalFare *= per;
            return totalFare;
        }

        protected void BookTicket_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["Username"]?.ToString();
                string sourceStation = Session["SourceStation"].ToString();
                string destinationStation = Session["DestinationStation"].ToString();
                double distance = (double)Session["Distance"];
                double fare = CalculateFare(distance, lblSummaryTicketType.Text, lblSummaryTrainType.Text, lblSummaryClassType.Text, lblSummaryAdult.Text);
                int adultCount = Convert.ToInt32(ddlAdult.SelectedValue);
                string ticketType = ddlTicketType.SelectedValue;
                string trainType = ddlTrainType.SelectedValue;
                string classType = ddlClass.SelectedValue;
                string paymentType = ddlPaymentType.SelectedValue;
                Session["totalFare"]=fare;
                Session["payType"] = paymentType;
                Session["Count"] = adultCount;
                Session["srcstn"] = sourceStation;
                Session["deststn"] = destinationStation;
                Session["dist"] = distance;
                Session["ticketType"] = ticketType;
                Session["trainType"] = trainType;
                Session["classType"] = classType;
                Session["ViaRoute"] = pathStr;
                Session["referringPage"] = "multitrain";
                // Redirect to confirmation page or display success message
                Response.Redirect("paygate.aspx");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                lblError.Text = "An error occurred while booking the ticket: " + ex.Message;
            }
        }

        private (double lat, double lon) GetCoordinates(string stationName)
        {
            double latitude = 0;
            double longitude = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Latitude, Longitude FROM Station WHERE Station_Code = @StationName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StationName", stationName);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        latitude = reader.GetDouble(0);
                        longitude = reader.GetDouble(1);
                    }
                }
            }

            return (latitude, longitude);
        }

        private double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in kilometers
            var lat1Rad = ToRadians(lat1);
            var lon1Rad = ToRadians(lon1);
            var lat2Rad = ToRadians(lat2);
            var lon2Rad = ToRadians(lon2);

            var dlat = lat2Rad - lat1Rad;
            var dlon = lon2Rad - lon1Rad;

            var a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in kilometers
        }

        private double ToRadians(double angleInDegrees)
        {
            return angleInDegrees * (Math.PI / 180);
        }
    }
}
