using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;

namespace RailwayTicketWebsite
{
    public partial class paygate : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    // Set the amount (you can modify this as needed)
                    double amt = (double)Session["totalFare"];
                    txtAmount.Text = Convert.ToString(amt); // Example fixed amount
                    string payType = Session["payType"]?.ToString();
                    if (payType.Equals("UPI"))
                    {
                        lblID.Text = "Enter UPI ID:";
                    }
                    else if (payType.Equals("CARD"))
                    {
                        lblID.Text = "Enter Card No:";
                    }
                }
                else
                {
                    Response.Redirect("dashboard.aspx");
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get values from the text boxes
            string upiId = txtUPI.Text;
            string name = txtName.Text;
            string amount = txtAmount.Text;
            string username = Session["Username"].ToString();
            int count = (int)Session["Count"];
            string payType = Session["payType"]?.ToString();
            string refer = Session["referringPage"]?.ToString();
            double fare = (double)Session["totalFare"];
            if (refer.Equals("singletrain"))
            {
                try
                {
                    string sourceStation = Session["srcstn"].ToString();
                    string destinationStation = Session["deststn"].ToString();
                    double distance = (double)Session["dist"];
                    string ticketType = Session["ticketType"].ToString();
                    string trainType = Session["trainType"].ToString();
                    string classType = Session["classType"].ToString();
                    string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Insert ticket booking information
                        string ticketQuery = "INSERT INTO BookedTickets (Username, SourceStation, DestinationStation, BookingDate, TotalFare, Adults, TicketType, TrainType, ClassType, PaymentType) " +
                                       "VALUES (@Username, @SourceStation, @DestinationStation, @BookingDate, @TotalFare, @Adults, @TicketType, @TrainType, @ClassType, @PaymentType)";
                        SqlCommand ticketCommand = new SqlCommand(ticketQuery, connection);
                        ticketCommand.Parameters.AddWithValue("@Username", username);
                        ticketCommand.Parameters.AddWithValue("@SourceStation", sourceStation);
                        ticketCommand.Parameters.AddWithValue("@DestinationStation", destinationStation);
                        ticketCommand.Parameters.AddWithValue("@BookingDate", DateTime.Now); // assuming booking date is the current date
                        ticketCommand.Parameters.AddWithValue("@TotalFare", fare);
                        ticketCommand.Parameters.AddWithValue("@Adults", count);
                        ticketCommand.Parameters.AddWithValue("@TicketType", ticketType);
                        ticketCommand.Parameters.AddWithValue("@TrainType", trainType);
                        ticketCommand.Parameters.AddWithValue("@ClassType", classType);
                        ticketCommand.Parameters.AddWithValue("@PaymentType", payType);
                        ticketCommand.Parameters.AddWithValue("@PayID", upiId);
                        ticketCommand.Parameters.AddWithValue("@PaidBy", name);

                        connection.Open();
                        ticketCommand.ExecuteNonQuery();
                    }

                    // Redirect to confirmation page or display success message
                    Session["referringPage"] = "railpayment";
                    Response.Redirect("previousbook.aspx");
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    lblError.Text = "An error occurred while booking the ticket: " + ex.Message;
                }
            }
            else if (refer.Equals("multitrain"))
            {
                try
                {
                    string sourceStation = Session["srcstn"].ToString();
                    string destinationStation = Session["deststn"].ToString();
                    double distance = (double)Session["dist"];
                    string ticketType = Session["ticketType"].ToString();
                    string trainType = Session["trainType"].ToString();
                    string classType = Session["classType"].ToString();
                    string pathStr = Session["ViaRoute"].ToString();
                    string connectionString = ConfigurationManager.ConnectionStrings["RaileaseConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO BookedTickets (Username, SourceStation, DestinationStation, ViaRoute, BookingDate, TotalFare, Adults, TicketType, TrainType, ClassType, PaymentType) " +
                                       "VALUES (@Username, @SourceStation, @DestinationStation, @ViaRoute, @BookingDate, @TotalFare, @Adults, @TicketType, @TrainType, @ClassType, @PaymentType)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@SourceStation", sourceStation);
                        command.Parameters.AddWithValue("@DestinationStation", destinationStation);
                        command.Parameters.AddWithValue("@ViaRoute", pathStr);
                        command.Parameters.AddWithValue("@BookingDate", DateTime.Now); // assuming booking date is the current date
                        command.Parameters.AddWithValue("@TotalFare", fare);
                        command.Parameters.AddWithValue("@Adults", count);
                        command.Parameters.AddWithValue("@TicketType", ticketType);
                        command.Parameters.AddWithValue("@TrainType", trainType);
                        command.Parameters.AddWithValue("@ClassType", classType);
                        command.Parameters.AddWithValue("@PaymentType", payType);
                        command.Parameters.AddWithValue("@PayID", upiId);
                        command.Parameters.AddWithValue("@PaidBy", name);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    // Redirect to confirmation page or display success message
                    Session["referringPage"] = "railpayment";
                    Response.Redirect("previousbook.aspx");
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    lblError.Text = "An error occurred while booking the ticket: " + ex.Message;
                }
            }
            else if (refer.Equals("platform"))
            {
                string station = Session["platstn"]?.ToString();

                string insertQuery = "INSERT INTO PlatformTickets (StationName, NumberOfTickets, PaymentType, PurchaseDate, Username, TotalAmount) VALUES (@StationName, @NumberOfPersons, @PaymentType, @BookingDate, @Username, @TotalFare)";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@StationName", station);
                            cmd.Parameters.AddWithValue("@NumberOfPersons", count);
                            cmd.Parameters.AddWithValue("@PaymentType", payType);
                            cmd.Parameters.AddWithValue("@BookingDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@TotalFare", fare);
                            cmd.Parameters.AddWithValue("@PayID", upiId);
                            cmd.Parameters.AddWithValue("@PaidBy", name);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Provide feedback to the user
                                Session["referringPage"] = "platformticket";
                                Response.Redirect("previousbook.aspx");
                            }
                            else
                            {
                                lblError.Text = "Booking failed. Please try again.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and provide feedback to the user
                        // LogError(ex); // Implement a logging mechanism if needed
                        lblError.Text = "An error occurred while processing your request. Please try again later.->" + ex;
                    }
                }
            }
            // Process the payment here
            // For demonstration purposes, we'll just display a message
            // You should replace this with actual payment processing code    
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear all session variables except 'username'
            if (Session["Username"] != null)
            {
                string username = Session["Username"].ToString();
                Session.Clear(); // Clears all session variables
                Session["Username"] = username; // Re-set 'username'
                Response.Redirect("dashboard.aspx");
            }
        }
    }
}