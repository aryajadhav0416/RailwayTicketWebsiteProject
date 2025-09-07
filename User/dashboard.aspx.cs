using System;
using System.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace RailwayTicketWebsite.User
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    // User is logged in
                    LoginButton.Visible = false;
                    LogoutButton.Style["display"] = "block";
                    uname.Text = Session["Username"].ToString();
                }
                else
                {
                    // User is not logged in
                    LoginButton.Visible = true;
                    LogoutButton.Style["display"] = "none";
                    uname.Text = "Guest";
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("userloginpg.aspx");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("dashboard.aspx");
        }



        [WebMethod]
        public static string[] GetSourceSuggestions(string prefixText, int count)
        {
            return GetSuggestions(prefixText, count);
        }

        [WebMethod]
        public static string[] GetDestinationSuggestions(string prefixText, int count)
        {
            return GetSuggestions(prefixText, count);
        }

        private static string[] GetSuggestions(string prefixText, int count)
        {
            List<string> suggestions = new List<string>();
            string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT DISTINCT TOP (@Count) station_name FROM Station WHERE station_name LIKE @PrefixText + '%'";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Count", count);
                    cmd.Parameters.AddWithValue("@PrefixText", prefixText);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestions.Add(reader["station_name"].ToString());
                        }
                    }
                }
            }

            return suggestions.ToArray();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            // Store source and destination in session
            Session["SourceStation"] = srcTextBox.Text.Trim();
            Session["DestinationStation"] = destTextBox.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            int srclineId = GetLineId(srcTextBox.Text.Trim(), connectionString);
            int destlineId = GetLineId(destTextBox.Text.Trim(), connectionString);
            int srcbranchId = GetBranchId(srcTextBox.Text.Trim(), connectionString);
            int destbranchId = GetBranchId(destTextBox.Text.Trim(), connectionString);
            if ((srclineId == destlineId) && (srcbranchId ==destbranchId))
            {
                Response.Redirect("searchresult.aspx");
            }
            else
            {
                List<string> stationCodes = new List<string>();
                stationCodes.Add(GetCode(srcTextBox.Text.Trim(),connectionString));
                string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
                string query = @"SELECT interstn_code
                     FROM TransferLine
                     WHERE srclineid = @srcLineId
                     AND destlineid = @destLineId Order By seq_in_path;";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@srcLineId", srclineId);
                        cmd.Parameters.AddWithValue("@destLineId", destlineId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stationCodes.Add(reader["interstn_code"].ToString());
                            }
                        }
                    }
                }
                stationCodes.Add(GetCode(destTextBox.Text.Trim(),connectionString));
                // Store the station codes in a session variable
                Session["TransferStationCodes"] = stationCodes;
                /*string pathStr = string.Join(" -> ", stationCodes);
                uname.Text = "Shortest Path: " + pathStr + "<br />";
                */Response.Redirect("multilinesearchresult.aspx");
            }
        }

        private int GetLineId(string stationName, string connectionString)
        {
            int lineId = 0;
            string query = "SELECT l.line_id FROM Station s JOIN Line l ON s.line_id = l.line_id WHERE s.station_name = @StationName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StationName", stationName);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lineId = Convert.ToInt32(result);
                    }
                }
            }

            return lineId;
        }

        private int GetBranchId(string stationName, string connectionString)
        {
            int lineId = 0;
            string query = "SELECT is_branch FROM Station WHERE station_name = @StationName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StationName", stationName);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lineId = Convert.ToInt32(result);
                    }
                }
            }

            return lineId;
        }

        private string GetCode(string stationName, string connectionString)
        {
            string stncode= string.Empty;
            string query = "SELECT s.station_code FROM Station s WHERE s.station_name = @StationName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StationName", stationName);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        stncode = Convert.ToString(result);
                    }
                }
            }

            return stncode;
        }

        protected void BookButton_Click(object sender, EventArgs e)
        {
            // Store source and destination in session
            Session["SourceStation"] = srcTextBox.Text.Trim();
            Session["DestinationStation"] = destTextBox.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

            int srclineId = GetLineId(srcTextBox.Text.Trim(), connectionString);
            int destlineId = GetLineId(destTextBox.Text.Trim(), connectionString);
            int srcbranchId = GetBranchId(srcTextBox.Text.Trim(), connectionString);
            int destbranchId = GetBranchId(destTextBox.Text.Trim(), connectionString);
            if ((srclineId == destlineId) && (srcbranchId !=1 && destbranchId !=1))
            {
                Response.Redirect("payment.aspx");
            }
            else
            {
                List<string> stationCodes = new List<string>();
                stationCodes.Add(GetCode(srcTextBox.Text.Trim(), connectionString));
                string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
                string query = @"SELECT interstn_code
                     FROM TransferLine
                     WHERE srclineid = @srcLineId
                     AND destlineid = @destLineId Order By seq_in_path;";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@srcLineId", srclineId);
                        cmd.Parameters.AddWithValue("@destLineId", destlineId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stationCodes.Add(reader["interstn_code"].ToString());
                            }
                        }
                    }
                }
                stationCodes.Add(GetCode(destTextBox.Text.Trim(), connectionString));
                // Store the station codes in a session variable
                Session["TransferStationCodes"] = stationCodes;
                /*string pathStr = string.Join(" -> ", stationCodes);
                uname.Text = "Shortest Path: " + pathStr + "<br />";
                */
                Response.Redirect("multilinepayment.aspx");
            }
        }

        protected void PlatformTicket_Click(object sender, EventArgs e)
        {
            // Redirect to book ticket page
            Response.Redirect("platformticket.aspx");
        }

        protected void PreviousBook_Click(object sender, EventArgs e)
        {
            // Redirect to previous bookings page
            Session["referringPage"] = "dashboard";
            Response.Redirect("previousbook.aspx");
        }

        [WebMethod]
        public static bool ValidateStations(string source, string destination)
        {
            bool isValidSource = false;
            bool isValidDestination = false;

            string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
            string query = "SELECT COUNT(1) FROM Station WHERE station_name = @StationName";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Validate source station
                    cmd.Parameters.AddWithValue("@StationName", source);
                    isValidSource = (int)cmd.ExecuteScalar() > 0;

                    // Reset parameter for destination station
                    cmd.Parameters["@StationName"].Value = destination;
                    isValidDestination = (int)cmd.ExecuteScalar() > 0;
                }
            }

            return isValidSource && isValidDestination;
        }
    }
}
