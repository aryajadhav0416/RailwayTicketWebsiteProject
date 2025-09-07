using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace RailwayTicketWebsite
{
    public partial class PreviousBook : System.Web.UI.Page
    {
        private string ConnString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    if (Session["referringPage"].Equals("dashboard"))
                    {
                        lblheading.Text = "Your Railway Tickets";
                        pnlBookedTicket.Visible = true;
                        ShowBookedTickets();
                    }
                    else if (Session["referringPage"].Equals("platformticket"))
                    {
                        lblheading.Text = "Your Platform Tickets";
                        pnlPlatformTicket.Visible = true;
                        ShowPlatformTickets();
                    }
                    else if (Session["referringPage"].Equals("railpayment"))
                    {
                        lblheading.Text = "Your Railway Tickets";
                        pnlBookedTicket.Visible = true;
                        ShowBookedTickets();
                    }
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void ShowPlatformTickets()
        {
            lblheading.Text = "Your Platform Tickets";
            string userName = Session["UserName"].ToString();

            string query = @"
                SELECT
                    'PlatformTicket' AS TicketType,
                    TicketID AS ID,
                    StationName AS SourceStation,
                    NumberOfTickets AS Adults,
                    TotalAmount AS TotalFare,
                    PurchaseDate AS BookingDate
                FROM
                    PlatformTickets
                WHERE
                    Username = @UserName
                ORDER BY BookingDate DESC";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind data to the Repeater control
                    rptPlatform.DataSource = dt;
                    rptPlatform.DataBind();
                }
            }
        }

        private void ShowBookedTickets()
        {
            lblheading.Text = "Your Railway Tickets";
            string userName = Session["UserName"].ToString();

            string query = @"
                SELECT 
                    BookingID,
                    SourceStation,
                    DestinationStation,
                    ViaRoute,
                    TicketType,
                    TrainType,
                    ClassType,
                    Adults,
                    TotalFare,
                    BookingDate
                FROM 
                    BookedTickets
                WHERE 
                    UserName = @UserName
                ORDER BY BookingDate DESC";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind data to the Repeater control
                    rptBookings.DataSource = dt;
                    rptBookings.DataBind();
                }
            }
        }
        protected void btnShowPlatformTickets_Click(object sender, EventArgs e)
        {
            pnlBookedTicket.Visible = false;
            pnlPlatformTicket.Visible = true;
            ShowPlatformTickets();
        }

        protected void btnShowBookedTickets_Click(object sender, EventArgs e)
        {
            pnlPlatformTicket.Visible = false;
            pnlBookedTicket.Visible = true;
            ShowBookedTickets(); // You might want to adjust this if there's different logic for booked tickets
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("dashboard.aspx"); // Redirect to the homepage or another appropriate page
        }
    }
}
