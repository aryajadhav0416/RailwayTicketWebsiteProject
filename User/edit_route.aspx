<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit_route.aspx.cs" Inherits="RailwayTicketWebsite.User.edit_route" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Route</title>
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
            text-align: left;
            color: #ffffff;
            background-color: #ff6600;
            padding: 10px;
            border-radius: 5px;
        }

        .form-group {
            margin-bottom: 20px;
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
            width: 100%;
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
            <h1>Edit Route</h1>
            <div class="form-row">
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblSourceLineID" runat="server" Text="Source Line ID:" CssClass="form-group" />
                        <asp:TextBox ID="txtSourceLineID" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblDestinationLineID" runat="server" Text="Destination Line ID:" CssClass="form-group" />
                        <asp:TextBox ID="txtDestinationLineID" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>
            </div>
            <!-- GridView to show multiple stations -->
            <asp:GridView ID="gvRouteDetails" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowDataBound="gvTimetable_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-control" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label ID="Id" runat="server" Text='<%# Eval("id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
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
            <asp:Button ID="btnAddStop" runat="server" Text="Add Stop" CssClass="btn btn-primary" OnClick="btnAddStop_Click" CausesValidation="false" />
            <asp:Button ID="btnRemoveStop" runat="server" Text="Remove Stop" CssClass="btn btn-danger" OnClick="btnRemoveStop_Click" CausesValidation="false" />
            <asp:Button ID="btnSaveTimetable" runat="server" Text="Save Timetable" CssClass="btn btn-primary" OnClick="btnSaveTimetable_Click" />
        </div>
    </form>
</body>
</html>
