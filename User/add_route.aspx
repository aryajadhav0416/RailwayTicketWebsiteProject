<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add_route.aspx.cs" Inherits="RailwayTicketWebsite.User.add_route" %>

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

            <!-- Section for Adding Train -->
            <h2>Add Route</h2>
            <div class="form-row">
                <div class="form-group">
                    <label for="srclineId">Source Line ID:</label>
                    <asp:DropDownList ID="ddlsrcLineId" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvsrcLineId" runat="server" ControlToValidate="ddlsrcLineId" InitialValue="0" ErrorMessage="Source Line ID is required" ForeColor="Red" />
                </div>
                <div class="form-group">
                    <label for="destlineId">Destination Line ID:</label>
                    <asp:DropDownList ID="ddldestLineId" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvdestLineId" runat="server" ControlToValidate="ddldestLineId" InitialValue="0" ErrorMessage="Destination Line ID is required" ForeColor="Red" />
                </div>
            </div>
            <asp:Button ID="btnSaveRoute" runat="server" Text="Save Route" CssClass="btn btn-primary" OnClick="btnSaveRoute_Click"/>

            <!-- Section for Adding Route Stops -->
            <asp:Panel ID="pnlRouteStops" runat="server" Visible="false">
                <div class="form-group">
                    <label for="numberOfRows">Number of Rows:</label>
                    <asp:TextBox ID="txtNumberOfRows" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnGenerateTable" runat="server" Text="Generate Table" CssClass="btn btn-primary" OnClick="btnGenerateTable_Click" CausesValidation="false" />
                
                <!-- Editable Table for Route Stops -->
                <asp:Panel ID="pnlEditableTable" runat="server" Visible="false">
                    <asp:GridView ID="gvRouteStops" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvRouteStops_RowDataBound" CssClass="table table-striped">
                        <Columns>
                            <asp:TemplateField HeaderText="Sequence">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSequence" runat="server" CssClass="form-control" Text='<%# Eval("Sequence") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSequence" runat="server" ControlToValidate="txtSequence" ErrorMessage="Sequence is required." ForeColor="Red" />
                                    <asp:RangeValidator ID="rvSequence" runat="server" ControlToValidate="txtSequence" ErrorMessage="Sequence must be a positive integer." MinimumValue="1" MaximumValue="999" Type="Integer" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Source Route">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSourceRouteID" runat="server" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSourceRouteID" runat="server" ControlToValidate="ddlSourceRouteID"
                                        InitialValue="0" ErrorMessage="Station is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Destination Route">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlDestinationRouteID" runat="server" CssClass="form-control"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDestinationRouteID" runat="server" ControlToValidate="ddlDestinationRouteID"
                                        InitialValue="0" ErrorMessage="Station is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interchanging Station">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlInterchangeStationID" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvInterchangeStationID" runat="server" ControlToValidate="ddlInterchangeStationID"
                                        InitialValue="0" ErrorMessage="Station is required" ForeColor="Red" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnSaveRouteStops" runat="server" Text="Save Transfer Routes" CssClass="btn btn-primary" OnClick="btnSaveRouteStops_Click" />
                </asp:Panel>
            </asp:Panel>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" Visible="false" CausesValidation="false" />
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
