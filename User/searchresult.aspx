<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="searchresult.aspx.cs" Inherits="RailwayTicketWebsite.searchresult" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Train Search Results</title>
    <style>
        /* Style for search results */
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

        .details-button:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Train Search Results</h2>
            <div class="button-group">
                <asp:Button ID="btnFast" runat="server" Text="Fast" OnClick="btnFast_Click" CssClass="filter-button" />
                <asp:Button ID="btnSlow" runat="server" Text="Slow" OnClick="btnSlow_Click" CssClass="filter-button" />
                <asp:Button ID="btnAC" runat="server" Text="AC" OnClick="btnAC_Click" CssClass="filter-button" />
                <asp:Button ID="btnNextTrains" runat="server" Text="Next Trains" OnClick="btnNextTrains_Click" CssClass="filter-button" />
            </div>
            <div class="search-results" id="search-results">
                <asp:Repeater ID="rptTrainResults" runat="server" OnItemCommand="rptTrainResults_ItemCommand">
                    <ItemTemplate>
                        <div class="train-item">
                            <h3><%# Eval("train_name") %></h3>
                            <p><strong>Type:</strong> <%# Eval("train_type") %></p>
                            <p><strong>Time:</strong> <%# Eval("departure_time", "{0:hh\\:mm}") %></p>
                            <p><strong>Source Station:</strong> <%# Eval("source_station") %></p>
                            <p><strong>Destination Station:</strong> <%# Eval("destination_station") %></p>
                            <p><strong>Platform No:</strong> <%# Eval("platform") %></p>
                            <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CommandName="ViewDetails" CommandArgument='<%# Eval("train_id") + ";" + Eval("train_name") %>' CssClass="details-button" />
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
