<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add_timetable.aspx.cs" Inherits="RailwayTicketWebsite.User.edit_routestop" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Route Stops and Add Train</title>
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

        h2 {
            text-align: left;
            color: #ffffff;
            background-color: #ff6600;
            padding: 10px;
            border-radius: 5px;
            width:fit-content;
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
            width: 30%;
        }

        .form-control {
            width: 70%;
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

            .note p {
                margin-top: -5px;
            }
        .table {
            width: 100%;
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
            <h1>Add Train</h1>

            <!-- Section for Adding Train -->
            <div class="form-row">
                <div class="form-column">
                    <div class="form-group">
                        <label for="trainName">Train Name:</label>
                        <asp:TextBox ID="txtTrainName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTrainName" runat="server" ControlToValidate="txtTrainName" ErrorMessage="Train Name is required" ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label for="lineId">Line ID:</label>
                        <asp:DropDownList ID="ddlLineId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLineId_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvLineId" runat="server" ControlToValidate="ddlLineId" InitialValue="0" ErrorMessage="Line ID is required" ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label for="srcStationId">Source Station:</label>
                        <asp:DropDownList ID="ddlSrcStationId" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSrcStationId" runat="server" ControlToValidate="ddlSrcStationId" InitialValue="0" ErrorMessage="Source Station is required" ForeColor="Red" />
                    </div>
                </div>
                <div class="form-column">
                    <div class="form-group">
                        <label for="destStationId">Destination Station:</label>
                        <asp:DropDownList ID="ddlDestStationId" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDestStationId" runat="server" ControlToValidate="ddlDestStationId" InitialValue="0" ErrorMessage="Destination Station is required" ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label for="trainType">Train Type:</label>
                        <asp:DropDownList ID="ddlTrainType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--Select Type of Train--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Slow" Value="Slow"></asp:ListItem>
                            <asp:ListItem Text="Fast" Value="Fast"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTrainType" runat="server" ControlToValidate="ddlTrainType" InitialValue="0" ErrorMessage="Train Type is required" ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label for="acNonAc">AC/Non-AC:</label>
                        <asp:DropDownList ID="ddlAcNonAc" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--Select Train (AC/NONAC)--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="AC" Value="AC"></asp:ListItem>
                            <asp:ListItem Text="Non-AC" Value="Non-AC"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvAcNonAc" runat="server" ControlToValidate="ddlAcNonAc" InitialValue="0" ErrorMessage="AC/NONAC is required" ForeColor="Red" />
                    </div>
                </div>
            </div>
            <asp:Button ID="btnSaveTrain" runat="server" Text="Save Train" CssClass="btn btn-primary" OnClick="btnSaveTrain_Click" />

            <!-- Section for Adding Route Stops -->
            <asp:Panel ID="pnlRouteStops" runat="server" Visible="false">
                <h2>Edit Route Stops</h2>
                <div class="form-group">
                    <label for="numberOfRows">Number of Rows:</label>
                    <asp:TextBox ID="txtNumberOfRows" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnGenerateTable" runat="server" Text="Generate Table" CssClass="btn btn-primary" OnClick="btnGenerateTable_Click" />
                
                <!-- Editable Table for Route Stops -->
                <asp:Panel ID="pnlEditableTable" runat="server" Visible="false">
                    <div class="note">
                        <p><u>Note</u> :<br />
                            For Source & Destination of Train, Arrival & Departure Times should be same.</p>
                    </div>
                    <asp:GridView ID="gvRouteStops" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvRouteStops_RowDataBound" CssClass="table table-striped">
                        <Columns>
                            <asp:TemplateField HeaderText="Route ID">
                                <ItemTemplate>
                                    <asp:Label ID="RouteId" runat="server" />
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
                                    <asp:TextBox ID="txtArrivalTime" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvArrivalTime" runat="server" ControlToValidate="txtArrivalTime" ErrorMessage="Arrival Time is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Departure Time">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDepartureTime" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDepartureTime" runat="server" ControlToValidate="txtDepartureTime" ErrorMessage="Departure Time is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sequence">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSequence" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSequence" runat="server" ControlToValidate="txtSequence" ErrorMessage="Sequence is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Platform No.">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPlatformNo" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revPlatformNo" runat="server" ControlToValidate="txtPlatformNo" ErrorMessage="Invalid platform number" ValidationExpression="^\d+$" ForeColor="Red" />
                                    <asp:RequiredFieldValidator ID="rfvPlatformNo" runat="server" ControlToValidate="txtPlatformNo" ErrorMessage="Platform No. is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnSaveRouteStops" runat="server" Text="Save Route Stops" CssClass="btn btn-primary" OnClick="btnSaveRouteStops_Click" />

                </asp:Panel>
            </asp:Panel>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" Visible="false" CausesValidation="false" />
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
