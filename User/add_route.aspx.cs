using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class add_route : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadLineIds();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }
        private void LoadLineIds()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT line_id, line_name FROM Line", con))
                {
                    SqlDataReader reader1 = cmd.ExecuteReader();
                    ddlsrcLineId.DataSource = reader1;
                    ddlsrcLineId.DataTextField = "line_name";
                    ddlsrcLineId.DataValueField = "line_id";
                    ddlsrcLineId.DataBind();
                    reader1.Close();
                    SqlDataReader reader2 = cmd.ExecuteReader();
                    ddldestLineId.DataSource = reader2;
                    ddldestLineId.DataTextField = "line_name";
                    ddldestLineId.DataValueField = "line_id";
                    ddldestLineId.DataBind();
                    reader2.Close();
                }
            }

            ddlsrcLineId.Items.Insert(0, new ListItem("--Select Source Line--", "0"));
            ddldestLineId.Items.Insert(0, new ListItem("--Select Destination Line--", "0"));
        }

        protected void btnSaveRoute_Click(object sender, EventArgs e)
        {
            if (ddlsrcLineId.SelectedIndex != 0 && ddldestLineId.SelectedIndex != 0)
            {
                pnlRouteStops.Visible = true;
                btnSaveRoute.Visible = false;
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
        protected void btnGenerateTable_Click(object sender, EventArgs e)
        {
            int numberOfRows = int.Parse(txtNumberOfRows.Text);
            if (numberOfRows > 0)
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
            dt.Columns.Add("Sequence");
            dt.Columns.Add("SourceRoute");
            dt.Columns.Add("DestinationRoute");
            dt.Columns.Add("InterchangeStation");
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
                ddlSourceRouteID.SelectedIndex = 0;
                ddlDestinationRouteID.SelectedIndex = 0;
                
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

        protected void btnSaveRouteStops_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvRouteStops.Rows)
            {
                DropDownList ddlInterSrcStationId = (DropDownList)row.FindControl("ddlSourceRouteID");
                DropDownList ddlInterDestStationId = (DropDownList)row.FindControl("ddlDestinationRouteID");
                DropDownList ddlInterchangeStationId = (DropDownList)row.FindControl("ddlInterchangeStationID");
                TextBox txtSequence = (TextBox)row.FindControl("txtSequence");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO transferline (srclineid, destlineid, intersrclineid, interdestlineid, interstn_code, seq_in_path) " +
                        "VALUES (@srcline, @destline, @intersrc, @interdest, @interstn, @sequence)", con))
                    {
                        cmd.Parameters.AddWithValue("@srcline", Convert.ToInt32(ddlsrcLineId.SelectedValue));
                        cmd.Parameters.AddWithValue("@destline", Convert.ToInt32(ddldestLineId.SelectedValue));
                        cmd.Parameters.AddWithValue("@intersrc", Convert.ToInt32(ddlInterSrcStationId.SelectedValue));
                        cmd.Parameters.AddWithValue("@interdest", Convert.ToInt32(ddlInterDestStationId.SelectedValue));
                        cmd.Parameters.AddWithValue("@interstn", Convert.ToString(ddlInterchangeStationId.SelectedValue));
                        cmd.Parameters.AddWithValue("@sequence", txtSequence.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Redirect("route_list.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("route_list.aspx");
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
    }
}