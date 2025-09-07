<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paygate.aspx.cs" Inherits="RailwayTicketWebsite.paygate" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Form</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        .container {
            max-width: 400px;
            margin: auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
        }
        .form-group input[type="text"] {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
        }
        .form-group input[disabled] {
            background-color: #f5f5f5;
        }
        .form-group button {
            padding: 10px 15px;
            background-color: #4CAF50;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            margin-right: 10px;
        }
        .form-group button.cancel {
            background-color: #f44336;
        }
        .error {
            color: red;
        }
    </style>
</head>
<body>
    <form id="paymentForm" runat="server">
        <div class="container">
            <div class="form-group">
                <label for="txtUPI"><asp:Label ID="lblID" runat="server" Text=""></asp:Label></label>
                <asp:TextBox ID="txtUPI" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUPI" runat="server" ControlToValidate="txtUPI" 
                    ErrorMessage="UPI ID is required." CssClass="error" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revUPI" runat="server" ControlToValidate="txtUPI" 
                    ErrorMessage="UPI ID must be numeric only." CssClass="error" 
                    ValidationExpression="^\d+$" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtName">Name:</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                    ErrorMessage="Name is required." CssClass="error" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" 
                    ErrorMessage="Name must contain only letters." CssClass="error" 
                    ValidationExpression="^[a-zA-Z\s]+$" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtAmount">Payment Amount:</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit Payment" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel Payment" CausesValidation="false" CssClass="cancel" OnClick="btnCancel_Click" />
            </div>
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
