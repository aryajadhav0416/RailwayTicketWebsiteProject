<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add_station.aspx.cs" Inherits="RailwayTicketWebsite.User.add_station" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Station</title>
    <link href="styles.css" rel="stylesheet" />
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .container {
            width: 80%;
            margin: 0 auto;
            padding: 20px;
        }

        h1 {
            text-align: center;
            color: #ffffff;
            background-color: #ff6600;
            padding: 10px;
            border-radius: 5px;
        }

        .form-group {
            margin-bottom: 15px;
            display: flex;
            align-items: center;
        }

        label {
            font-weight: bold;
            margin-right: 10px;
            width: 30%;
        }

        .form-control {
            width: 70%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .btn {
            display: block;
            width: 20%;
            padding: 10px;
            margin: 10px auto;
            border: none;
            border-radius: 5px;
            color: #ffffff;
            background-color: #007bff;
            cursor: pointer;
            text-align: center;
        }

        .btn-danger {
            background-color: #dc3545;
        }

        .btn-primary:hover,
        .btn-danger:hover {
            background-color: #0056b3;
        }

        .note {
            font-size: 14px;
            color: #555;
            height: 40px;
            text-align: left;
            margin-top: -10px;
            margin-bottom: 10px;
            padding: 10px;
            background-color: #91ed76;
            border-radius: 4px;
            border-left: 4px solid #3498db;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Add Station</h1>

            <div class="form-group">
                <label for="stationName">Station Name:</label>
                <asp:TextBox ID="txtStationName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvStationName" runat="server" ControlToValidate="txtStationName" ErrorMessage="Station Name is required" ForeColor="Red" />
            </div>

            <div class="form-group">
                <label for="stationCode">Station Code:</label>
                <asp:TextBox ID="txtStationCode" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvStationCode" runat="server" ControlToValidate="txtStationCode" ErrorMessage="Station Code is required" ForeColor="Red" />
                <asp:RegularExpressionValidator ID="revStationCode" runat="server" ControlToValidate="txtStationCode" ValidationExpression="^[A-Z]{5}$" ErrorMessage="Invalid Station Code. It should be 3 uppercase letters." ForeColor="Red" />
            </div>

            <div class="form-group">
                <label for="lineId">Line ID:</label>
                <asp:DropDownList ID="ddlLineId" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvLineId" runat="server" ControlToValidate="ddlLineId" InitialValue="0" ErrorMessage="Line ID is required" ForeColor="Red" />
            </div>

            <div class="form-group">
                <label for="latitude">Latitude:</label>
                <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLatitude" runat="server" ControlToValidate="txtLatitude" ErrorMessage="Latitude is required" ForeColor="Red" />
                <asp:RegularExpressionValidator ID="revLatitude" runat="server" ControlToValidate="txtLatitude" ValidationExpression="^-?\d{1,2}\.\d+$" ErrorMessage="Invalid Latitude format." ForeColor="Red" />
            </div>

            <div class="form-group">
                <label for="longitude">Longitude:</label>
                <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLongitude" runat="server" ControlToValidate="txtLongitude" ErrorMessage="Longitude is required" ForeColor="Red" />
                <asp:RegularExpressionValidator ID="revLongitude" runat="server" ControlToValidate="txtLongitude" ValidationExpression="^-?\d{1,3}\.\d+$" ErrorMessage="Invalid Longitude format." ForeColor="Red" />
            </div>

            <div class="form-group">
                <label for="isbranch">Branch:</label>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:Button ID="btnSaveStation" runat="server" Text="Save Station" CssClass="btn btn-primary" OnClick="btnSaveStation_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" Visible="false" CausesValidation="false" />
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
