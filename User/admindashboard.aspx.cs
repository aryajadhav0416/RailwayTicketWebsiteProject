using System;
using System.Web.UI;

namespace RailwayTicketWebsite.Admin
{
    public partial class admindashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Logic to handle user logout
            // For example, clearing session data and redirecting to login page:
            Session.Clear();
            Session.Abandon();
            Response.Redirect("userloginpg.aspx");
        }

        protected void ManageTrainsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("train_list.aspx");
        }

        protected void ManageLinesButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("lineslab_list.aspx");
        }

        protected void ManageStationsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("station_list.aspx");
        }

        protected void ManageRoutesButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("route_list.aspx");
        }

        protected void ManageFaresButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("fareslab_list.aspx");
        }
    }
}
