<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="train_list.aspx.cs" Inherits="RailwayTicketWebsite.User.train_list" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Train List</title>
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
        .top-right-button {
            text-align: center;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Train List</h1>

            <!-- Filtering Section -->
            <div class="form-group">
                <label for="lineFilter">Filter by Line:</label>
                <asp:DropDownList ID="ddlLineFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLineFilter_SelectedIndexChanged" CssClass="form-control">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="top-right-button">
                <asp:Button ID="btnAddNewRoute" runat="server" Text="Add New Train" CssClass="btn btn-primary" OnClick="btnAddNewRoute_Click" />
            </div>

            <!-- GridView for Train List -->
            <asp:GridView ID="gvTrainList" runat="server" AutoGenerateColumns="False" CssClass="table" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvTrainList_PageIndexChanging" OnRowCommand="gvTrainList_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Train ID">
                        <ItemTemplate>
                            <asp:Label ID="TrainID" runat="server" Text='<%# Eval("train_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Train Name">
                        <ItemTemplate>
                            <asp:Label ID="TrainName" runat="server" Text='<%# Eval("route_name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Train Type">
                        <ItemTemplate>
                            <asp:Label ID="TrainType" runat="server" Text='<%# Eval("train_type") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AC/Non-AC">
                        <ItemTemplate>
                            <asp:Label ID="trainCondtioned" runat="server" Text='<%# Eval("ac_nonac") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Line">
                        <ItemTemplate>
                            <asp:Label ID="linename" runat="server" Text='<%# Eval("line_name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Source">
                        <ItemTemplate>
                            <asp:Label ID="srcstn" runat="server" Text='<%# Eval("src") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Destination">
                        <ItemTemplate>
                            <asp:Label ID="deststn" runat="server" Text='<%# Eval("dest") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arrival Time">
                        <ItemTemplate>
                            <asp:Label ID="ArrivalTime" runat="server" Text='<%# Eval("departure_time", "{0:hh\\:mm}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Details">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" Text="View Details" CommandArgument='<%# Container.DisplayIndex %>' CommandName="ViewDetails" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit Details" CommandArgument='<%# Container.DisplayIndex %>' CommandName="EditDetails" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete Details" CommandArgument='<%# Container.DisplayIndex %>' CommandName="DeleteDetails" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Paging -->
            <asp:Label ID="lblPageInfo" runat="server" />
            <div class="top-right-button">
                <asp:Button ID="btnBack" runat="server" Text="Back To Home" OnClick="btnBack_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </form>
</body>
</html>
