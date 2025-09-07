<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fareslab_list.aspx.cs" Inherits="RailwayTicketWebsite.User.fareslab_list" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fare Slab List</title>
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
            <h1>Fare Slab List</h1>
            <div class="note">
                <p><u>Note</u> :<br />
                    <b>Manage the fare slabs for various distances and classes here.</b></p>
            </div>
            <div class="top-right-button">
                <asp:Button ID="btnAddNewSlab" runat="server" Text="Add New FareSlab" CssClass="btn btn-primary" OnClick="btnAddNewRoute_Click" />
            </div>
            <!-- GridView for Fare Slab List -->
            <asp:GridView ID="gvFareSlabList" runat="server" AutoGenerateColumns="False" CssClass="table" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvFareSlabList_PageIndexChanging" OnRowCommand="gvFareSlabList_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Fare Slab ID">
                        <ItemTemplate>
                            <asp:Label ID="FareSlabID" runat="server" Text='<%# Eval("FareSlabID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Min Distance">
                        <ItemTemplate>
                            <asp:Label ID="MinDistance" runat="server" Text='<%# Eval("MinDistance") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Max Distance">
                        <ItemTemplate>
                            <asp:Label ID="MaxDistance" runat="server" Text='<%# Eval("MaxDistance") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Second Class Fare">
                        <ItemTemplate>
                            <asp:Label ID="SecondClassFare" runat="server" Text='<%# Eval("SecondClassFare") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="First Class Fare">
                        <ItemTemplate>
                            <asp:Label ID="FirstClassFare" runat="server" Text='<%# Eval("FirstClassFare") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AC Fare">
                        <ItemTemplate>
                            <asp:Label ID="ACFare" runat="server" Text='<%# Eval("ACFare") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditDetails" CssClass="btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteDetails" CssClass="btn-primary" />
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
