<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit_timetable.aspx.cs" Inherits="RailwayTicketWebsite.User.edit_timetable" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Train Timetable</title>
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
            <h1>Edit Timetable</h1>

            <!-- Section for Selecting Line and Train -->
            <div class="form-row">
                <div class="form-column">
                    <div class="form-group">
                        <label for="trainName">Train Name:</label>
                        <asp:TextBox ID="txtTrainName" runat="server" TextMode="SingleLine" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTrainName" runat="server" ControlToValidate="txtTrainName" ErrorMessage="Train Name is required." ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label for="trainType">Train Type:</label>
                        <asp:DropDownList ID="ddlTrainType" runat="server" CssClass="form-control">
                            <asp:ListItem>Slow</asp:ListItem>
                            <asp:ListItem>Fast</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTrainType" runat="server" ControlToValidate="ddlTrainType" ErrorMessage="Train Type is required." ForeColor="Red" InitialValue="" />
                    </div>
                    <div class="form-group">
                        <label for="trainAC">Train(AC/NONAC):</label>
                        <asp:DropDownList ID="ddltrainAC" runat="server" CssClass="form-control">
                            <asp:ListItem>AC</asp:ListItem>
                            <asp:ListItem>NONAC</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvtrainAC" runat="server" ControlToValidate="ddltrainAC" ErrorMessage="Train AC/NONAC is required." ForeColor="Red" InitialValue="" />
                    </div>
                </div>
                <div class="form-column">
                    <div class="form-group">
                        <label for="srcStationId">Source Station:</label>
                        <asp:DropDownList ID="ddlSourceStationId" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSourceStation" runat="server" ControlToValidate="ddlSourceStationId" ErrorMessage="Source Station is required." ForeColor="Red" InitialValue="" />
                    </div>
                    <div class="form-group">
                        <label for="destStationId">Destination Station:</label>
                        <asp:DropDownList ID="ddlDestinationStationId" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDestinationStation" runat="server" ControlToValidate="ddlDestinationStationId" ErrorMessage="Destination Station is required." ForeColor="Red" InitialValue="" />
                    </div>
                    <div class="form-group">
                        <label for="originTime">Origin Time:</label>
                        <asp:TextBox ID="txtOriginTime" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOriginTime" runat="server" ControlToValidate="txtOriginTime" ErrorMessage="Origin Time is required." ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="revOriginTime" runat="server" ControlToValidate="txtOriginTime" ErrorMessage="Invalid time format." ForeColor="Red" ValidationExpression="^\d{2}:\d{2}$" />
                    </div>
                </div>
            </div>
            <!-- Section for Editing Timetable -->
            <asp:Panel ID="pnlTimetable" runat="server" Visible="false">
                <asp:GridView ID="gvTimetable" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvTimetable_RowDataBound" CssClass="table table-striped">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-control" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Route ID">
                            <ItemTemplate>
                                <asp:Label ID="RouteId" runat="server" Text='<%# Eval("routestop_id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Station ID">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStationId" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvStationId" runat="server" ControlToValidate="ddlStationId" InitialValue="0" ErrorMessage="Station is required" ForeColor="Red" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrival Time">
                            <ItemTemplate>
                                <asp:TextBox ID="txtArrivalTime" runat="server" TextMode="Time" CssClass="form-control" Text='<%# Eval("arrival_time", "{0:hh\\:mm}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvArrivalTime" runat="server" ControlToValidate="txtArrivalTime" ErrorMessage="Arrival Time is required." ForeColor="Red" />
                                <asp:RegularExpressionValidator ID="revArrivalTime" runat="server" ControlToValidate="txtArrivalTime" ErrorMessage="Invalid time format." ValidationExpression="^\d{2}:\d{2}$" ForeColor="Red" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Departure Time">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDepartureTime" runat="server" TextMode="Time" CssClass="form-control" Text='<%# Eval("departure_time", "{0:hh\\:mm}") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDepartureTime" runat="server" ControlToValidate="txtDepartureTime" ErrorMessage="Departure Time is required." ForeColor="Red" />
                                <asp:RegularExpressionValidator ID="revDepartureTime" runat="server" ControlToValidate="txtDepartureTime" ErrorMessage="Invalid time format." ValidationExpression="^\d{2}:\d{2}$" ForeColor="Red" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sequence">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSequence" runat="server" CssClass="form-control" Text='<%# Eval("sequence") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSequence" runat="server" ControlToValidate="txtSequence" ErrorMessage="Sequence is required." ForeColor="Red" />
                                <asp:RangeValidator ID="rvSequence" runat="server" ControlToValidate="txtSequence" ErrorMessage="Sequence must be a positive integer." MinimumValue="1" MaximumValue="999" Type="Integer" ForeColor="Red" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Platform">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPlatform" runat="server" CssClass="form-control" Text='<%# Eval("plat_no") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPlatform" runat="server" ControlToValidate="txtPlatform" ErrorMessage="Platform is required." ForeColor="Red" />
                                <asp:RangeValidator ID="rvPlatform" runat="server" ControlToValidate="txtPlatform" ErrorMessage="Platform must be a positive integer." MinimumValue="1" MaximumValue="100" Type="Integer" ForeColor="Red" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnAddStop" runat="server" Text="Add Stop" CssClass="btn btn-primary" OnClick="btnAddStop_Click" CausesValidation="false"/>
                <asp:Button ID="btnRemoveStop" runat="server" Text="Remove Stop" CssClass="btn btn-danger" OnClick="btnRemoveStop_Click" CausesValidation="false" />
                <asp:Button ID="btnSaveTimetable" runat="server" Text="Save Timetable" CssClass="btn btn-primary" OnClick="btnSaveTimetable_Click" />
            </asp:Panel>
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
