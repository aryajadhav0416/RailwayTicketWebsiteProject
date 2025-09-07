using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class route_list : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    BindGridView();
                }
                else
                {
                    Response.Redirect("userloginpg.aspx");
                }
            }
        }

        private void BindGridView()
        {
            string query = @"
                WITH RouteParts AS (
    SELECT
        l.line_name AS SourceLineName,
        m.line_name AS DestinationLineName,
        t.interstn_code,
        t.seq_in_path,
        ROW_NUMBER() OVER (PARTITION BY t.srclineid, t.destlineid ORDER BY t.seq_in_path) AS rn
    FROM transferline t
    JOIN Line l ON l.line_id = t.srclineid
    JOIN Line m ON m.line_id = t.destlineid
)
SELECT
    SourceLineName AS SourceLineName,
    DestinationLineName AS DestinationLineName,
    STRING_AGG(interstn_code, '->') WITHIN GROUP (ORDER BY rn) AS FormattedRoute
FROM RouteParts
GROUP BY SourceLineName, DestinationLineName
ORDER BY SourceLineName, DestinationLineName;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                gvStationList.DataSource = dataTable;
                gvStationList.DataBind();

                lblPageInfo.Text = $"Page {gvStationList.PageIndex + 1} of {gvStationList.PageCount}";
            }
        }

        protected void gvStationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStationList.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void gvStationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditDetails"))
            {
                // Retrieve the row index stored in the CommandArgument property.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Get the GridViewRow based on the row index.
                GridViewRow row = gvStationList.Rows[rowIndex];

                // Retrieve all the cell values in the selected row.
                string SourceLineID = ((Label)row.FindControl("SourceLineID")).Text;
                string DestinationLineID = ((Label)row.FindControl("DestinationLineID")).Text;
                string InterchangingStation = ((Label)row.FindControl("InterchangingStation")).Text;
                // Concatenate all the values into a single string.
                Session["SelectedSourceLineID"] = LineID(SourceLineID);
                Session["SelectedDestinationLineID"] = LineID(DestinationLineID);
                Session["SelectedInterchangingStation"] = InterchangingStation;
                // Redirect to the details page with the station ID.
                Response.Redirect($"edit_route.aspx?station_id={SourceLineID}");
            }
        }

        private int LineID(string line)
        {
            string query = @"
SELECT line_id
FROM Line WHERE
line_name=@Line";
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Initialize the SqlCommand object
                SqlCommand command = new SqlCommand(query, connection);

                // Add the parameter and set its value
                command.Parameters.AddWithValue("@Line", line);

                try
                {
                    // Open the connection
                    connection.Open();

                    // Execute the query and get the result
                    object result = command.ExecuteScalar();

                    // Check if a result is returned and convert it to int
                    if (result != null && int.TryParse(result.ToString(), out int parsedId))
                    {
                        id = parsedId;
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, rethrow it, etc.)
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return id;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("admindashboard.aspx");
        }
        protected void btnAddNewRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_route.aspx");
        }
    }
}
