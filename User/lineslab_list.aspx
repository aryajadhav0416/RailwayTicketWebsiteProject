<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lineslab_list.aspx.cs" Inherits="RailwayTicketWebsite.User.lineslab_list" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Line Slab List</title>
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
            <h1>Line Slab List</h1>
            <div class="note">
                <p><u>Note</u> :<br />
                    <b>Manage the line slabs and their details here.</b></p>
            </div>
            <div class="top-right-button">
                <asp:Button ID="btnAddNewRoute" runat="server" Text="Add New Line" CssClass="btn btn-primary" OnClick="btnAddNewRoute_Click" />
            </div>
            <!-- GridView for Line Slab List -->
            <asp:GridView ID="gvLineSlabList" runat="server" AutoGenerateColumns="False" CssClass="table" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvLineSlabList_PageIndexChanging" OnRowCommand="gvLineSlabList_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Line ID">
                        <ItemTemplate>
                            <asp:Label ID="LineID" runat="server" Text='<%# Eval("line_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Line Name">
                        <ItemTemplate>
                            <asp:Label ID="LineName" runat="server" Text='<%# Eval("line_name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>' CommandName="EditDetails" CssClass="btn-primary" />
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
