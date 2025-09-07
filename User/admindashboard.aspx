<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="admindashboard.aspx.cs" Inherits="RailwayTicketWebsite.Admin.admindashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Admin Dashboard</title>
    <style>
        /* Styles for admin dashboard */
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }
        .container {
            display: flex;
            height: 100vh;
        }
        .sidebar {
            width: 200px;
            background-color: #2c3e50;
            color: white;
            padding: 20px;
            box-shadow: 2px 0 5px rgba(0,0,0,0.1);
        }
        .sidebar h2 {
            margin-top: 0;
        }
        .sidebar ul {
            list-style-type: none;
            padding: 0;
        }
        .sidebar ul li {
            margin: 15px 0;
            position: relative;
        }
        .sidebar ul li a {
            color: white;
            text-decoration: none;
        }
        .sidebar ul li:after {
            content: '';
            position: absolute;
            bottom: -10px;
            left: 0;
            width: 100%;
            border-bottom: 1px solid rgba(255, 255, 255, 0.2);
        }
        .sidebar .user-info {
            text-align: center;
            margin-bottom: 20px;
        }
        .sidebar .user-avatar {
            margin-bottom: 10px;
        }
        .sidebar .user-avatar .img {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            display: block;
            margin: 0 auto 10px;
        }
        .sidebar .user-info h3 {
            text-align: center;
            margin: 0;
        }
        .main-content {
            flex-grow: 1;
            padding: 20px;
        }
        .header {
            background-color: #2980b9;
            color: white;
            padding: 10px;
            text-align: center;
        }
        .btn-container {
            display: flex;
            flex-wrap: wrap;
            margin-top: 20px;
            justify-content: center;
        }
        .btn-container .styled-button {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 15px 25px;
            margin: 10px;
            cursor: pointer;
            border-radius: 5px;
            font-size: 16px;
            width: calc(50% - 20px);
        }
        .btn-container .styled-button:hover {
            background-color: #2980b9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">
            <div class="main-content">
                <div class="header">
                    <h1>Admin Dashboard</h1>
                </div>
                <div class="btn-container">
                    <asp:Button ID="ManageTrainsButton" CssClass="styled-button" runat="server" Text="Manage Trains" OnClick="ManageTrainsButton_Click" />
                    <asp:Button ID="ManageLinesButton" CssClass="styled-button" runat="server" Text="Manage Lines" OnClick="ManageLinesButton_Click" />
                    <asp:Button ID="ManageStationsButton" CssClass="styled-button" runat="server" Text="Manage Stations" OnClick="ManageStationsButton_Click" />
                    <asp:Button ID="ManageRoutesButton" CssClass="styled-button" runat="server" Text="Manage Routes" OnClick="ManageRoutesButton_Click" />
                    <asp:Button ID="ManageFaresButton" CssClass="styled-button" runat="server" Text="Manage Fares" OnClick="ManageFaresButton_Click" />
                    <asp:Button ID="LogoutButton" CssClass="styled-button" runat="server" Text="Logout" OnClick="LogoutButton_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
