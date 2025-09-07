<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="route_list.aspx.cs" Inherits="RailwayTicketWebsite.User.route_list" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Interchanging Stations</title>
    <link href="styles.css" rel="stylesheet" />
    <style>
        /* Add your styles here */
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
            margin-bottom: 20px;
        }

        .form-control {
            width: 200px;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .btn {
            padding: 10px;
            border: none;
            border-radius: 5px;
            color: #ffffff;
            background-color: #007bff;
            cursor: pointer;
            text-align: center;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
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

        .btn-primary {
            display: inline-block;
            padding: 10px 15px;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
            color: #ffffff;
            background-color: #007bff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .btn-primary:hover {
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
        .top-right-button {
            text-align: center;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Interchanging Stations</h1>
            <div class="note">
                <p><u>Note</u> :<br />
                    <b>Branch: If a station lies on a branch (where the line is divided into sub-lines), then Branch has a value '1', otherwise '0'</b></p>
            </div>
            <div class="top-right-button">
                <asp:Button ID="btnAddNewRoute" runat="server" Text="Add New Route" CssClass="btn btn-primary" OnClick="btnAddNewRoute_Click" />
            </div>
            <!-- GridView for Interchanging Stations -->
            <asp:GridView ID="gvStationList" runat="server" AutoGenerateColumns="False" CssClass="table" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvStationList_PageIndexChanging" OnRowCommand="gvStationList_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Source Line ID">
                        <ItemTemplate>
                            <asp:Label ID="SourceLineID" runat="server" Text='<%# Eval("SourceLineName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Destination Line ID">
                        <ItemTemplate>
                            <asp:Label ID="DestinationLineID" runat="server" Text='<%# Eval("DestinationLineName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Interchanging Station">
                        <ItemTemplate>
                            <asp:Label ID="InterchangingStation" runat="server" Text='<%# Eval("FormattedRoute") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" Text="Edit Details" CommandArgument='<%# Container.DisplayIndex %>' CommandName="EditDetails" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Paging -->
            <asp:Label ID="lblPageInfo" runat="server" CssClass="form-control" />
            <div class="top-right-button">
                <asp:Button ID="btnBack" runat="server" Text="Back To Home" OnClick="btnBack_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </form>
</body>
</html>
