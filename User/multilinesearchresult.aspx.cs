using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RailwayTicketWebsite.User
{
    public partial class multilinesearchresult : System.Web.UI.Page
    {
        private string ConnString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
        private List<string> stationCodes;
        private TimeSpan? currentTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["TransferStationCodes"] != null)
                {
                    stationCodes = (List<string>)Session["TransferStationCodes"];
                    currentTime = DateTime.Now.TimeOfDay;
                    LoadMultiLineTrainData();
                }
                else
                {
                    Response.Redirect("dashboard.aspx");
                }
            }
        }

        protected void btnNextTrains_Click(object sender, EventArgs e)
        {
            LoadMultiLineTrainData(currentTime);
        }

        private void LoadMultiLineTrainData(TimeSpan? fromTime = null)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("line_name", typeof(string));
            resultTable.Columns.Add("train_id", typeof(int));
            resultTable.Columns.Add("train_name", typeof(string));
            resultTable.Columns.Add("train_type", typeof(string));
            resultTable.Columns.Add("source_station", typeof(string));
            resultTable.Columns.Add("destination_station", typeof(string));
            resultTable.Columns.Add("departure_time", typeof(TimeSpan));
            resultTable.Columns.Add("arrival_time", typeof(TimeSpan));

            for (int i = 0; i < stationCodes.Count - 1; i++)
            {
                string sourceStation = stationCodes[i];
                string destinationStation = stationCodes[i + 1];

                string query = @"
            SELECT DISTINCT
                l.line_name AS line_name,
                t.train_id,
                t.train_name,
                t.train_type,
                s_boarding.station_name AS source_station,
                s_thane.station_name AS destination_station,
                rs1.departure_time AS departure_time,
                rs2.arrival_time AS arrival_time
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
            JOIN
                Line l ON l.line_id = t.line_id
            WHERE
                s_boarding.station_code = @SourceStation
                AND s_thane.station_code = @DestinationStation
                AND rs1.sequence < rs2.sequence
                " + (fromTime.HasValue ? "AND rs1.departure_time > @FromTime" : "") + @"
            ORDER BY
                rs1.departure_time;";

                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SourceStation", sourceStation);
                        cmd.Parameters.AddWithValue("@DestinationStation", destinationStation);
                        if (fromTime.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@FromTime", fromTime.Value);
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            bool trainsAvailable = false;

                            foreach (DataRow row in dt.Rows)
                            {
                                TimeSpan departureTime;
                                TimeSpan arrivalTime;

                                if (!TimeSpan.TryParse(row["departure_time"]?.ToString(), out departureTime) ||
                                    !TimeSpan.TryParse(row["arrival_time"]?.ToString(), out arrivalTime))
                                {
                                    continue; // Skip this row if parsing fails
                                }

                                if (departureTime >= (currentTime.HasValue ? currentTime.Value : TimeSpan.Zero))
                                {
                                    DataRow newRow = resultTable.NewRow();
                                    newRow["line_name"] = row["line_name"];
                                    newRow["train_id"] = row["train_id"];
                                    newRow["train_name"] = row["train_name"];
                                    newRow["train_type"] = row["train_type"];
                                    newRow["source_station"] = row["source_station"];
                                    newRow["destination_station"] = row["destination_station"];
                                    newRow["departure_time"] = departureTime;
                                    newRow["arrival_time"] = arrivalTime;
                                    resultTable.Rows.Add(newRow);

                                    // Update current time for the next segment (arrival time + 5 minutes buffer)
                                    currentTime = arrivalTime.Add(TimeSpan.FromMinutes(5));
                                    trainsAvailable = true;
                                    break;
                                }
                            }

                            if (!trainsAvailable)
                            {
                                DataRow newRow = resultTable.NewRow();
                                newRow["line_name"] = DBNull.Value;
                                newRow["train_id"] = DBNull.Value;
                                newRow["train_name"] = "Train Not Available";
                                newRow["train_type"] = "";
                                newRow["source_station"] = sourceStation;
                                newRow["destination_station"] = destinationStation;
                                newRow["departure_time"] = DBNull.Value;
                                newRow["arrival_time"] = DBNull.Value;
                                resultTable.Rows.Add(newRow);
                            }
                        }
                        else
                        {
                            DataRow newRow = resultTable.NewRow();
                            newRow["line_name"] = DBNull.Value;
                            newRow["train_id"] = DBNull.Value;
                            newRow["train_name"] = "Train Not Available";
                            newRow["train_type"] = "";
                            newRow["source_station"] = sourceStation;
                            newRow["destination_station"] = destinationStation;
                            newRow["departure_time"] = DBNull.Value;
                            newRow["arrival_time"] = DBNull.Value;
                            resultTable.Rows.Add(newRow);
                        }
                    }
                }
            }

            rptMultiLineTrainResults.DataSource = resultTable;
            rptMultiLineTrainResults.DataBind();
        }


        protected void rptMultiLineTrainResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (DataBinder.Eval(e.Item.DataItem, "train_id") != DBNull.Value)
                {
                    int trainId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "train_id"));
                    string sourceStationCode = DataBinder.Eval(e.Item.DataItem, "source_station").ToString();
                    string destinationStationCode = DataBinder.Eval(e.Item.DataItem, "destination_station").ToString();
                    Repeater rptStations = (Repeater)e.Item.FindControl("rptStations");

                    if (rptStations != null && trainId != 0)
                    {
                        DataTable dtStations = GetTrainStations(trainId, sourceStationCode, destinationStationCode);
                        rptStations.DataSource = dtStations;
                        rptStations.DataBind();
                    }
                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("dashboard.aspx");
        }

        private DataTable GetTrainStations(int trainId, string sourceStationCode, string destinationStationCode)
        {
            DataTable dtStations = new DataTable();
            dtStations.Columns.Add("station_name", typeof(string));
            dtStations.Columns.Add("arrival_time", typeof(TimeSpan));

            string routeStationsQuery = @"
        SELECT
            s.station_name,
            rs.arrival_time
        FROM
            RouteStop rs
        JOIN
            Station s ON rs.station_id = s.station_id
        JOIN
            Route r ON r.route_id = rs.route_id
        JOIN
            Train t ON t.train_id = r.train_id
        WHERE
            t.train_id = @TrainId
            AND rs.sequence >= (
                SELECT TOP 1 rs1.sequence
                FROM RouteStop rs1
                JOIN Station s1 ON rs1.station_id = s1.station_id
                WHERE rs1.route_id = r.route_id AND s1.station_name = @SourceStationCode
            )
            AND rs.sequence <= (
                SELECT TOP 1 rs2.sequence
                FROM RouteStop rs2
                JOIN Station s2 ON rs2.station_id = s2.station_id
                WHERE rs2.route_id = r.route_id AND s2.station_name = @DestinationStationCode
            )
        ORDER BY rs.sequence;";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(routeStationsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TrainId", trainId);
                    cmd.Parameters.AddWithValue("@SourceStationCode", sourceStationCode);
                    cmd.Parameters.AddWithValue("@DestinationStationCode", destinationStationCode);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtStations);
                }
            }

            return dtStations;
        }


    }
}
