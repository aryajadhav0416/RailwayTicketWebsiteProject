<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviousBook.aspx.cs" Inherits="RailwayTicketWebsite.PreviousBook" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Your Platform Tickets</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
    <style>
        .container {
            width: 80%;
            margin: 0 auto;
            padding: 20px;
        }
        h2 {
            text-align: center;
        }
        .button-group {
            text-align: center;
            margin-bottom: 20px;
        }
        .filter-button, .back-button {
            padding: 10px 20px;
            margin: 5px;
            border: none;
            background-color: #007bff;
            color: white;
            cursor: pointer;
        }
        .filter-button:hover, .back-button:hover {
            background-color: #0056b3;
        }
        .train-item {
            padding: 15px;
            margin: 10px 0;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 5px;
            display: flex;
            flex-direction: column;
            transition: background-color 0.2s;
        }

            .train-item:hover {
                background-color: #eaeaea;
            }

            .train-item h3 {
                margin: 0 0 10px 0;
                font-size: 1.2em;
                color: #333;
            }

            .train-item p {
                margin: 5px 0;
                font-size: 0.9em;
                color: #666;
            }
        .search-results {
            max-height: 600px;
            overflow-y: auto;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 10px;
        }
        .booking-item {
            border: 1px solid #ddd;
            padding: 10px;
            margin-bottom: 10px;
        }
        .booking-item h3 {
            margin: 0;
            font-size: 1.5em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>
                <asp:Label ID="lblheading" runat="server" Text=""></asp:Label></h2>
            <div class="button-group">
                <asp:Button ID="btnShowPlatformTickets" runat="server" Text="Show Platform Tickets" OnClick="btnShowPlatformTickets_Click" CssClass="filter-button" />
                <asp:Button ID="btnShowBookedTickets" runat="server" Text="Show Booked Tickets" OnClick="btnShowBookedTickets_Click" CssClass="filter-button" />
            </div>
            <asp:Panel ID="pnlBookedTicket" runat="server" Visible="false">
                <!-- PreviousBook.aspx -->
                <div class="search-results" id="search-results">
                    <asp:Repeater ID="rptBookings" runat="server">
                        <ItemTemplate>
                            <div class="train-item">
                                <p><strong>Booking ID:</strong> <%# Eval("BookingID") %></p>
                                <p><strong>Source Station:</strong> <%# Eval("SourceStation") %></p>
                                <p><strong>Destination Station:</strong> <%# Eval("DestinationStation") %></p>
                                <p><strong>Via Route:</strong> <%# Eval("ViaRoute") %></p>
                                <p><strong>Ticket Type:</strong> <%# Eval("TicketType") %></p>
                                <p><strong>Train Type:</strong> <%# Eval("TrainType") %></p>
                                <p><strong>Class Type:</strong> <%# Eval("ClassType") %></p>
                                <p><strong>Adults:</strong> <%# Eval("Adults") %></p>
                                <p><strong>Total Fare:</strong> <%# Eval("TotalFare") %></p>
                                <p><strong>Booking Date:</strong> <%# Eval("BookingDate") %></p>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlPlatformTicket" runat="server" Visible="false">
                <div class="search-results" id="search-results">
                    <asp:Repeater ID="rptPlatform" runat="server">
                        <ItemTemplate>
                            <div class="train-item">
                                <p><strong>Ticket Type:</strong> <%# Eval("TicketType") %></p>
                                <p><strong>Ticket ID:</strong> <%# Eval("ID") %></p>
                                <p><strong>Source Station:</strong> <%# Eval("SourceStation") %></p>
                                <p><strong>Adults:</strong> <%# Eval("Adults") %></p>
                                <p><strong>Total Fare:</strong> <%# Eval("TotalFare") %></p>
                                <p><strong>Booking Date:</strong> <%# Eval("BookingDate") %></p>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <div class="button-group">
                <asp:Button ID="btnBack" runat="server" Text="Back to Home" OnClick="btnBack_Click" CssClass="back-button" />
            </div>
        </div>
    </form>
</body>
</html>
