<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="traindetails.aspx.cs" Inherits="RailwayTicketWebsite.User.traindetails" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Train Details</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 800px;
            margin: 20px auto;
            background: #fff;
            padding: 20px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            position: relative;
        }

        h2 {
            margin-bottom: 20px;
            text-align: center;
        }

        .train-details {
            margin-bottom: 20px;
        }

        .train-details p {
            margin: 5px 0;
            font-size: 1em;
            color: #333;
        }

        .station-details {
            margin-top: 20px;
        }

        .station-item {
            padding: 10px;
            margin: 10px 0;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 5px;
            display: flex;
            justify-content: space-between;
            transition: background-color 0.2s;
        }

        .station-item:hover {
            background-color: #eaeaea;
        }

        .back-button {
            position: absolute;
            top: 20px;
            right: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="back-button" />
            <h2>Train Details</h2>
            <div class="train-details">
                <p><strong>Train Name:</strong> <asp:Label ID="lblTrainName" runat="server" /></p>
            </div>
            <div class="station-details">
                <asp:Repeater ID="rptStationDetails" runat="server">
                    <ItemTemplate>
                        <div class="station-item">
                            <p><strong>Station Name:</strong> <%# Eval("station_name") %></p>
                            <p><strong>Arrival Time:</strong> <%# Eval("arrival_time", "{0:hh\\:mm}") %></p>
                            <p><strong>Departure Time:</strong> <%# Eval("departure_time", "{0:hh\\:mm}") %></p>
                            <p><strong>Platform:</strong> <%# Eval("plat_no") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
