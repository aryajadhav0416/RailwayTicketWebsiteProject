<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="multilinepayment.aspx.cs" Inherits="RailwayTicketWebsite.User.multilinepayment" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Payment Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 400px;
            margin: 20px auto;
            background: #fff;
            padding: 20px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
        }

        .section-heading {
            font-size: 1.5em;
            margin-bottom: 20px;
            color: #ff6600;
            text-align: center;
            font-weight: bold;
        }

        .dropdown-container {
            margin-bottom: 15px;
        }

        .dropdown-container label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .dropdown-container select {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            background: #f9f9f9;
        }

        .summary-container {
            margin-top: 20px;
            background: #fff8e1;
            padding: 20px;
            border-radius: 8px;
            border: 1px solid #ff6600;
        }

        .summary-container h3 {
            margin-top: 0;
            color: #ff6600;
            font-size: 1.2em;
            font-weight: bold;
        }

        .summary-item {
            margin-bottom: 10px;
        }

        .summary-item label {
            font-weight: bold;
        }

        .btn {
            display: block;
            width: 100%;
            padding: 12px;
            background-color: #ff6600;
            color: white;
            text-align: center;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 1em;
            margin-top: 20px;
            text-transform: uppercase;
            font-weight: bold;
        }

        .btn:hover {
            background-color: #e65c00;
        }

        #map {
            height: 400px;
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="section-heading">Normal Booking</div>

            <input type="hidden" id="sourceStation" value="<%= Session["SourceStation"]?.ToString() %>" />
            <input type="hidden" id="destinationStation" value="<%= Session["DestinationStation"]?.ToString() %>" />

            <div class="dropdown-container">
                <label for="ddlAdult">Adult</label>
                <asp:DropDownList ID="ddlAdult" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                    <asp:ListItem Text="ONE (1)" Value="1"></asp:ListItem>
                    <asp:ListItem Text="TWO (2)" Value="2"></asp:ListItem>
                    <asp:ListItem Text="THREE (3)" Value="3"></asp:ListItem>
                    <asp:ListItem Text="FOUR (4)" Value="4"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="dropdown-container">
                <label for="ddlTicketType">Ticket Type</label>
                <asp:DropDownList ID="ddlTicketType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                    <asp:ListItem Text="JOURNEY (J)" Value="J"></asp:ListItem>
                    <asp:ListItem Text="RETURN (R)" Value="R"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="dropdown-container">
                <label for="ddlTrainType">Train Type</label>
                <asp:DropDownList ID="ddlTrainType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                    <asp:ListItem Text="ORDINARY (O)" Value="O"></asp:ListItem>
                    <asp:ListItem Text="AIR-CONDITIONED (AC)" Value="AC"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="dropdown-container">
                <label for="ddlClass">Class</label>
                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                    <asp:ListItem Text="SECOND (II)" Value="II"></asp:ListItem>
                    <asp:ListItem Text="FIRST (I)" Value="I"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="dropdown-container">
                <label for="ddlPaymentType">Payment Type</label>
                <asp:DropDownList ID="ddlPaymentType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                    <asp:ListItem Text="CARD" Value="CARD"></asp:ListItem>
                    <asp:ListItem Value="UPI">UPI</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="summary-container">
                <h3>Ticket Summary</h3>
                <div class="summary-item">Source Station: <asp:Label ID="lblSourceStation" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Destination Station: <asp:Label ID="lblDestinationStation" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Via: <asp:Label ID="lblViaRoute" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Adult: <asp:Label ID="lblSummaryAdult" runat="server" Text="0"></asp:Label></div>
                <div class="summary-item">Ticket Type: <asp:Label ID="lblSummaryTicketType" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Train Type: <asp:Label ID="lblSummaryTrainType" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Class Type: <asp:Label ID="lblSummaryClassType" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Payment Type: <asp:Label ID="lblSummaryPaymentType" runat="server" Text="None"></asp:Label></div>
                <div class="summary-item">Total Fare: <asp:Label ID="lblTotalFare" runat="server" Text="₹0"></asp:Label></div>
            </div>

            <asp:Button ID="btnBookTicket" runat="server" Text="BOOK TICKET" CssClass="btn" OnClick="BookTicket_Click" />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
        </div>
    </form>
</body>
</html>
