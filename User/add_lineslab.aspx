<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add_lineslab.aspx.cs" Inherits="RailwayTicketWebsite.User.add_lineslab" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Line Slab</title>
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
            width: 300px;
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

        .text-danger {
            color: #dc3545;
            font-size: 12px;
            margin-top: 5px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Add Line Slab</h1>
            <div class="form-group">
                <label for="txtLineName">Line Name:</label>
                <asp:TextBox ID="txtLineName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvLineName" runat="server" 
                    ControlToValidate="txtLineName" 
                    ErrorMessage="Line Name is required." 
                    CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="btnCancel_Click" />
            </div>
        </div>
    </form>
</body>
</html>
