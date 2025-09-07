<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="settingforgottenpassword.aspx.cs" Inherits="RailwayTicketWebsite.User.settingforgottenpassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Set New Password</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        .container {
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            text-align: center;
            width: 600px; /* Adjusted width to accommodate wider text field */
            position: relative; /* To make absolute positioning within container easier */
        }
        h2 {
            margin-bottom: 20px;
        }
        hr {
            margin: 20px 0;
            border: 0;
            border-top: 1px solid #ccc;
            width: calc(100% + 60px); /* Extend beyond the container width */
            position: relative;
            left: -30px; /* Move left to start at the container edge */
        }
        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
            justify-content: center;
        }
        .input {
            width: 75%; /* Adjusted width for text field */
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px; /* Rounded corners only on the left side */
            box-sizing: border-box;
            font-size: 16px;
            height: 45px;
        }
        .toggle-password {
            width: 25%; /* Adjusted width for the button */
            height: 45px;
            background-color: #3498db;
            border: 1px solid #ccc;
            border-left: none;
            color: white;
            cursor: pointer;
            font-size: 14px;
            border-radius: 4px; /* Rounded corners only on the right side */
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 0;
            margin: 10px;
        }
        .btn {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 12px;
            text-align: center;
            cursor: pointer;
            width: 100%;
            border-radius: 4px;
            font-size: 18px;
            margin-top: 20px;
        }
        .btn:hover {
            background-color: #2980b9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Set New Password</h2>
            <hr> <!-- Horizontal line below the heading -->
            <div class="form-group">
                <asp:TextBox ID="pass" runat="server" placeholder="Enter New Password" CssClass="input" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="pass" ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="pass"
                    ValidationExpression="^(?=.*[0-9].*[0-9])(?=.*[\W])(?=.*[^\s]).{8,11}$"
                    ErrorMessage="Password must be 8-11 characters, include at least 2 digits, 1 special character, and no spaces." ForeColor="Red" Display="Dynamic" />
                <asp:Button ID="btnTogglePassword" runat="server" Text="Show" CssClass="toggle-password" OnClick="TogglePasswordVisibility" CausesValidation="false" Font-Size="Medium" />
            </div>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn" OnClick="SubmitNewPassword_Click" />
        </div>
    </form>
</body>
</html>
