<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="multilinesearchresult.aspx.cs" Inherits="RailwayTicketWebsite.User.multilinesearchresult" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Multi-Line Train Search Results</title>
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
        }

        .details-button {
            align-self: flex-end;
            padding: 5px 10px;
            border: none;
            background-color: #28a745;
            color: white;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .stops-button {
            align-self: flex-end;
            padding: 5px 10px;
            border: none;
            background-color: #3f78b6;
            color: white;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .button-group {
            text-align: center;
            margin-bottom: 20px;
        }

        .filter-button {
            padding: 10px 20px;
            margin: 0 10px;
            border: none;
            background-color: #007BFF;
            color: white;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .filter-button:hover {
            background-color: #0056b3;
        }

        .search-results {
            max-height: 600px;
            overflow-y: auto;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 10px;
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

        .stations-list {
            display: none;
            margin-top: 10px;
            padding: 10px;
            background-color: #f4f4f4;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-family: 'Times New Roman', Times, serif; /* Use Times New Roman font */
        }

        .stations-list p {
            font-weight: bold; /* Make station names bold */
        }
    </style>
    <script type="text/javascript">
        function toggleStationsList(button) {
            var stationsList = button.parentElement.querySelector('.stations-list');
            if (stationsList.style.display === 'none') {
                stationsList.style.display = 'block';
                button.innerText = 'Hide Stops';
            } else {
                stationsList.style.display = 'none';
                button.innerText = 'View Stops';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Multi-Line Train Search Results</h2>
            <div class="button-group">
                <asp:Button ID="btnNextTrains" runat="server" Text="Next Trains" OnClick="btnNextTrains_Click" CssClass="filter-button" />
            </div>
            <div class="search-results" id="search-results">
                <asp:Repeater ID="rptMultiLineTrainResults" runat="server" OnItemDataBound="rptMultiLineTrainResults_ItemDataBound">
                    <ItemTemplate>
                        <div class="train-item">
                            <h3><%# Eval("train_name") %></h3>
                            <p><strong>Line:</strong> <%# Eval("line_name") %></p>
                            <p><strong>Type:</strong> <%# Eval("train_type") %></p>
                            <p><strong>Boarding Time:</strong> <%# Eval("departure_time", "{0:hh\\:mm}") %></p>
                            <p><strong>Source Station:</strong> <%# Eval("source_station") %></p>
                            <p><strong>Destination Station:</strong> <%# Eval("destination_station") %></p>
                            <p><strong>Destination Arrival Time:</strong> <%# Eval("arrival_time", "{0:hh\\:mm}") %></p>
                            <asp:Button ID="btnViewStops" runat="server" Text="View Stops" CssClass="stops-button" OnClientClick="toggleStationsList(this); return false;" />
                            <div class="stations-list">
                                <asp:Repeater ID="rptStations" runat="server">
                                    <ItemTemplate>
                                        <p><strong><%# Eval("arrival_time", "{0:hh\\:mm}") %></strong> - <%# Eval("station_name") %></p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="button-group">
                <asp:Button ID="btnBack" runat="server" Text="Back To Home" OnClick="btnBack_Click" CssClass="filter-button" />
            </div>
        </div>
    </form>
</body>
</html>
