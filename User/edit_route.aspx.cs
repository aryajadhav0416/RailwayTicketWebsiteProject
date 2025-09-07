using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class edit_route : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    if (Session["SelectedSourceLineID"] != null && Session["SelectedDestinationLineID"] != null)
                    {
                        LoadRouteDetails(Convert.ToInt32(Session["SelectedSourceLineID"].ToString()), Convert.ToInt32(Session["SelectedDestinationLineID"].ToString()));
                        txtSourceLineID.Text = LineName(Convert.ToInt32(Session["SelectedSourceLineID"].ToString()));
                        txtDestinationLineID.Text = LineName(Convert.ToInt32(Session["SelectedDestinationLineID"].ToString()));
                    }
                    else
                    {
                        // Handle the case where the parameters are invalid
                        Response.Redirect("route_list.aspx");
                    }
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }
        private DataTable GetLines()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT line_id, line_name FROM Line", con))
                {
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        private string LineName(int lineid)
        {
            string linename="";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT line_name FROM Line WHERE line_id = @LineID", con))
                {
                    cmd.Parameters.AddWithValue("@LineID", lineid);; // Ensure this matches the format in your DB

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        linename = Convert.ToString(result);
                    }
                }
            }
            return linename;
        }
        protected void gvTimetable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlSourceRouteID = (DropDownList)e.Row.FindControl("ddlSourceRouteID");
                DropDownList ddlDestinationRouteID = (DropDownList)e.Row.FindControl("ddlDestinationRouteID");
                DataTable dt = GetLines();

                ddlSourceRouteID.DataSource = dt;
                ddlSourceRouteID.DataTextField = "line_name";
                ddlSourceRouteID.DataValueField = "line_id";
                ddlSourceRouteID.DataBind();

                ddlDestinationRouteID.DataSource = dt;
                ddlDestinationRouteID.DataTextField = "line_name";
                ddlDestinationRouteID.DataValueField = "line_id";
                ddlDestinationRouteID.DataBind();

                // Set the selected values for dropdowns
                string sourceLineId = DataBinder.Eval(e.Row.DataItem, "intersrclineid")?.ToString();
                string destinationLineId = DataBinder.Eval(e.Row.DataItem, "interdestlineid")?.ToString();

                if (!string.IsNullOrEmpty(sourceLineId))
                {
                    ddlSourceRouteID.SelectedValue = sourceLineId;
                }

                if (!string.IsNullOrEmpty(destinationLineId))
                {
                    ddlDestinationRouteID.SelectedValue = destinationLineId;
                }

                // Ensure that the selected values of dropdowns are used to update the label.
                UpdateInterchangeStationLabel(e.Row);
            }
        }

        protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((DropDownList)sender).NamingContainer;
            UpdateInterchangeStationLabel(row);
        }

        private void UpdateInterchangeStationLabel(GridViewRow row)
        {
            DropDownList ddlSourceRouteID = (DropDownList)row.FindControl("ddlSourceRouteID");
            DropDownList ddlDestinationRouteID = (DropDownList)row.FindControl("ddlDestinationRouteID");
            DropDownList ddlInterchangeStationID = (DropDownList)row.FindControl("ddlInterchangeStationID");

            if (ddlSourceRouteID != null && ddlDestinationRouteID != null && ddlInterchangeStationID != null)
            {
                int sourceLineId = int.Parse(ddlSourceRouteID.SelectedValue);
                int destinationLineId = int.Parse(ddlDestinationRouteID.SelectedValue);

                DataTable interchangeStations = GetInterchangeStations(sourceLineId, destinationLineId);
                ddlInterchangeStationID.DataSource = interchangeStations;
                ddlInterchangeStationID.DataTextField = "station_code";
                ddlInterchangeStationID.DataValueField = "station_code";
                ddlInterchangeStationID.DataBind();

                // Optionally, set a default selected value if necessary
                if (ddlInterchangeStationID.Items.Count > 0)
                {
                    ddlInterchangeStationID.SelectedIndex = 0; // Example: set to the first item
                }
            }
        }

        private DataTable GetInterchangeStations(int sourceLineId, int destinationLineId)
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT DISTINCT s1.station_code
        FROM dbo.Station s1
        INNER JOIN dbo.Station s2
        ON s1.station_code = s2.station_code
        WHERE s1.line_id = @SourceLineID
        AND s2.line_id = @DestinationLineID;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SourceLineID", sourceLineId);
                    cmd.Parameters.AddWithValue("@DestinationLineID", destinationLineId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        private string GetInterchangeStation(int sourceLineId, int destinationLineId)
        {
            string stationCode = string.Empty;
            string query = @"SELECT DISTINCT s1.station_code
            FROM dbo.Station s1
            INNER JOIN dbo.Station s2
            ON s1.station_code = s2.station_code
            WHERE s1.line_id = @SourceLineID
            AND s2.line_id = @DestinationLineID;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SourceLineID", sourceLineId);
                    cmd.Parameters.AddWithValue("@DestinationLineID", destinationLineId);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        stationCode = Convert.ToString(result);
                    }
                }
            }

            return stationCode;
        }

        protected void btnAddStop_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Sequence");
            dt.Columns.Add("intersrclineid");
            dt.Columns.Add("interdestlineid");
            dt.Columns.Add("interstn_code");

            foreach (GridViewRow row in gvRouteDetails.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = ((Label)row.FindControl("Id")).Text;
                dr["Sequence"] = ((TextBox)row.FindControl("txtSequence")).Text;
                dr["intersrclineid"] = ((DropDownList)row.FindControl("ddlSourceRouteID")).SelectedValue;
                dr["interdestlineid"] = ((DropDownList)row.FindControl("ddlDestinationRouteID")).SelectedValue;
                dr["interstn_code"] = ((DropDownList)row.FindControl("ddlInterchangeStationID")).SelectedValue;
                dt.Rows.Add(dr);
            }

            // Add a new empty row for user to input new stop details
            DataRow newRow = dt.NewRow();
            //newRow["routestop_id"] = "NEW"; Special identifier for new rows
            dt.Rows.Add(newRow);

            gvRouteDetails.DataSource = dt;
            gvRouteDetails.DataBind();
        }

        private void LoadRouteDetails(int sourceLineId, int destinationLineId)
        {
            string query = @"
                SELECT
                    id,
                    seq_in_path AS Sequence,
                    intersrclineid,
                    interdestlineid,
                    interstn_code AS StationCode
                FROM transferline
                WHERE srclineid = @SourceLineID AND destlineid = @DestinationLineID
                ORDER BY seq_in_path;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SourceLineID", sourceLineId);
                command.Parameters.AddWithValue("@DestinationLineID", destinationLineId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                gvRouteDetails.DataSource = dataTable;
                gvRouteDetails.DataBind();
            }
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
                dt.Columns.Add("id");
                dt.Columns.Add("Sequence");
                dt.Columns.Add("intersrclineid");
                dt.Columns.Add("interdestlineid");
                dt.Columns.Add("interstn_code");

                foreach (GridViewRow row in gvRouteDetails.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = ((Label)row.FindControl("Id")).Text;
                    dr["Sequence"] = ((TextBox)row.FindControl("txtSequence")).Text;
                    dr["intersrclineid"] = ((DropDownList)row.FindControl("ddlSourceRouteID")).SelectedValue;
                    dr["interdestlineid"] = ((DropDownList)row.FindControl("ddlDestinationRouteID")).SelectedValue;
                    dr["interstn_code"] = ((DropDownList)row.FindControl("ddlInterchangeStationID")).SelectedValue;
                    dt.Rows.Add(dr);
                }
            }

            List<int> routeStopIdsToRemove = new List<int>();
            List<int> newRowsToRemove = new List<int>();

            // Loop through each row in the GridView
            foreach (GridViewRow row in gvRouteDetails.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    Label lblRouteId = (Label)row.FindControl("Id");
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
            gvRouteDetails.DataSource = dt;
            gvRouteDetails.DataBind();

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
                                string query = "DELETE FROM transferline WHERE id = @RoutestopId";
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
                LoadRouteDetails(Convert.ToInt32(Session["SelectedSourceLineID"].ToString()), Convert.ToInt32(Session["SelectedDestinationLineID"].ToString()));
            }
        }

        protected void btnSaveTimetable_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                foreach (GridViewRow row in gvRouteDetails.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    // Only process the row if the checkbox is ticked
                    if (chkSelect.Checked)
                    {
                        string Id = ((Label)row.FindControl("Id")).Text;
                        DropDownList ddlInterSrcStationId = (DropDownList)row.FindControl("ddlSourceRouteID");
                        DropDownList ddlInterDestStationId = (DropDownList)row.FindControl("ddlDestinationRouteID");
                        DropDownList ddlInterchangeStationId = (DropDownList)row.FindControl("ddlInterchangeStationID");
                        TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                        int srcstation = Convert.ToInt32(ddlInterSrcStationId.SelectedValue);
                        int deststation = Convert.ToInt32(ddlInterDestStationId.SelectedValue);
                        string station = Convert.ToString(ddlInterchangeStationId.SelectedValue);
                        int sequence = Convert.ToInt32(txtSequence.Text);
                        string query = @"IF EXISTS (SELECT 1 FROM transferline WHERE id = @Id)
                                            UPDATE transferline SET intersrclineid = @intersrc, 
                                            interdestlineid = @interdest, interstn_code = @interchange, 
                                            seq_in_path = @sequence WHERE id = @Id
                                            ELSE
                                            INSERT INTO transferline 
                                            (srclineid, destlineid, intersrclineid, 
                                            interdestlineid, interstn_code,seq_in_path) 
                                            VALUES (@srcline, @destline, @intersrc, 
                                            @interdest, @interchange, @sequence)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", Id);
                            cmd.Parameters.AddWithValue("@srcline", Convert.ToInt32(Session["SelectedSourceLineID"].ToString()));
                            cmd.Parameters.AddWithValue("@destline", Convert.ToInt32(Session["SelectedDestinationLineID"].ToString()));
                            cmd.Parameters.AddWithValue("@intersrc", srcstation);
                            cmd.Parameters.AddWithValue("@interdest", deststation);
                            cmd.Parameters.AddWithValue("@interchange", station);
                            cmd.Parameters.AddWithValue("@sequence", sequence);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            Response.Redirect("route_list.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("route_list.aspx");
        }
    }
}
