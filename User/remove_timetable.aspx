<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="remove_timetable.aspx.cs" Inherits="RailwayTicketWebsite.User.remove_timetable" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remove Train Timetable</title>
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

        h1, h2 {
            text-align: left;
            color: #ffffff;
            background-color: #ff6600;
            padding: 10px;
            border-radius: 5px;
        }

        .form-group {
            display: flex;
            flex-direction: row;
            align-items: center;
            margin-bottom: 15px;
        }

        label {
            font-weight: bold;
            margin-right: 10px;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .form-row {
            display: flex;
            justify-content: space-between;
            flex-wrap: wrap;
        }

        .form-column {
            width: 48%;
            display: flex;
            flex-direction: column;
            align-items: center;
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

        .table {
            width: fit-content;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .table, .table th, .table td {
            border: 1px solid #ddd;
        }

        .table th, .table td {
            padding: 8px;
            text-align: center;
        }

        .table th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Remove Train Timetable</h1>

            <!-- Section for Selecting Line and Train -->
            <h2>View Timetable</h2>
            <div class="form-row">
    <div class="form-column">
        <div class="form-group">
            <label for="trainName">Train Name:</label>
            <asp:TextBox ID="txtTrainName" runat="server" TextMode="SingleLine" CssClass="form-control" Enabled="False"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvTrainName" runat="server" ControlToValidate="txtTrainName" ErrorMessage="Train Name is required." ForeColor="Red" />
        </div>
        <div class="form-group">
            <label for="trainType">Train Type:</label>
            <asp:DropDownList ID="ddlTrainType" runat="server" CssClass="form-control" Enabled="false">
                <asp:ListItem>Slow</asp:ListItem>
                <asp:ListItem>Fast</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTrainType" runat="server" ControlToValidate="ddlTrainType" ErrorMessage="Train Type is required." ForeColor="Red" InitialValue="" />
        </div>
        <div class="form-group">
            <label for="trainAC">Train(AC/NONAC):</label>
            <asp:DropDownList ID="ddltrainAC" runat="server" CssClass="form-control" Enabled="false">
                <asp:ListItem>AC</asp:ListItem>
                <asp:ListItem>NONAC</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvtrainAC" runat="server" ControlToValidate="ddltrainAC" ErrorMessage="Train AC/NONAC is required." ForeColor="Red" InitialValue="" />
        </div>
    </div>
    <div class="form-column">
        <div class="form-group">
            <label for="srcStationId">Source Station:</label>
            <asp:DropDownList ID="ddlSourceStationId" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvSourceStation" runat="server" ControlToValidate="ddlSourceStationId" ErrorMessage="Source Station is required." ForeColor="Red" InitialValue="" />
        </div>
        <div class="form-group">
            <label for="destStationId">Destination Station:</label>
            <asp:DropDownList ID="ddlDestinationStationId" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvDestinationStation" runat="server" ControlToValidate="ddlDestinationStationId" ErrorMessage="Destination Station is required." ForeColor="Red" InitialValue="" />
        </div>
        <div class="form-group">
            <label for="originTime">Origin Time:</label>
            <asp:TextBox ID="txtOriginTime" runat="server" TextMode="Time" CssClass="form-control" Enabled="false"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvOriginTime" runat="server" ControlToValidate="txtOriginTime" ErrorMessage="Origin Time is required." ForeColor="Red" />
            <asp:RegularExpressionValidator ID="revOriginTime" runat="server" ControlToValidate="txtOriginTime" ErrorMessage="Invalid time format." ForeColor="Red" ValidationExpression="^\d{2}:\d{2}$" />
        </div>
    </div>
</div>
            <!-- Section for Viewing Timetable -->
            <asp:Panel ID="pnlTimetable" runat="server" Visible="false">
                <asp:GridView ID="gvTimetable" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                    <Columns>
                        <asp:TemplateField HeaderText="Route ID">
                            <ItemTemplate>
                                <asp:Label ID="RouteId" runat="server" Text='<%# Eval("route_id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Station Name">
                            <ItemTemplate>
                                <asp:Label ID="StationId" runat="server" Text='<%# Eval("station_name") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrival Time">
                            <ItemTemplate>
                                <asp:Label ID="ArrivalTime" runat="server" Text='<%# Eval("arrival_time", "{0:hh\\:mm}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Departure Time">
                            <ItemTemplate>
                                <asp:Label ID="DepartureTime" runat="server" Text='<%# Eval("departure_time", "{0:hh\\:mm}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Platform">
                            <ItemTemplate>
                                <asp:Label ID="Platform" runat="server" Text='<%# Eval("plat_no") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sequence">
                            <ItemTemplate>
                                <asp:Label ID="Sequence" runat="server" Text='<%# Eval("sequence") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Button ID="btnDeleteTimetable" runat="server" Text="Delete Timetable" CssClass="btn btn-danger" OnClick="btnDeleteTimetable_Click" />
            </asp:Panel>
        </div>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
