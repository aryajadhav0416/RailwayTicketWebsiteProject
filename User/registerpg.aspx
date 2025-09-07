<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registerpg.aspx.cs" Inherits="RailwayTicketWebsite.User.registerpg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registration Page</title>
    <style>
    body {
        font-family: Arial, sans-serif;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        height: 100vh;
        margin: 0;
        background-color: #f2f2f2;
    }

    .logo {
        margin-bottom: 20px;
            height: 78px;
            width: 294px;
        }

    .container {
        background-color: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        width: 300px;
        text-align: center;
    }

    .container h2 {
        margin-bottom: 10px;
    }

    .container hr {
        margin: 20px 0;
        border: 0;
        border-top: 1px solid #ccc;
        width: calc(100% + 40px);
        position: relative;
        left: -20px;
    }

    .container input[type="text"], .container input[type="password"], .container input[type="tel"], .container input[type="date"] {
        width: 93%;
        padding: 10px;
        margin: 10px 0;
        border: 1px solid #ccc;
        border-radius: 4px;
    }

    #btnregister {
        width: 100%;
        padding: 10px;
        background-color: #4CAF50;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    #btnregister:hover {
        background-color: #45a049;
    }

    .login-link {
        margin-top: 20px;
    }

    .login-link a {
        color: #4CAF50;
        text-decoration: none;
    }

    .login-link a:hover {
        text-decoration: underline;
    }

    /* OTP Modal Styles */
    .modal {
        display: block;
        position: fixed;
        z-index: 1;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        overflow: auto;
        background-color: rgb(0,0,0);
        background-color: rgba(0,0,0,0.4);
    }

    .modal-content {
        background-color: #fefefe;
        margin: 15% auto;
        padding: 20px;
        border: 1px solid #888;
        width: 80%;
        max-width: 400px;
        border-radius: 5px;
        position: relative;
    }

    .modal-content h2 {
        text-align: center;
    }

    .close {
        color: #aaa;
        float: right;
        font-size: 28px;
        font-weight: bold;
    }

    .close:hover,
    .close:focus {
        color: black;
        text-decoration: none;
        cursor: pointer;
    }

    .otp-container {
        display: flex;
        justify-content: space-between;
    }

    .otp-input {
        width: 20%;
        padding: 10px;
        text-align: center;
        margin: 10px 0;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    .btn-submit {
        display: block;
        width: 100%;
        padding: 10px;
        background: #007bff;
        border: none;
        color: #fff;
        border-radius: 5px;
        cursor: pointer;
        margin-top: 20px;
        text-align: center;
    }

    #timer {
        text-align: center;
        margin-top: 20px;
        font-size: 18px;
    }

    /* Autocomplete Styles */
    .autocomplete-suggestions {
        border: 1px solid #ddd;
        background: #fff;
        max-height: 150px;
        overflow-y: auto;
        box-shadow: 0px 5px 8px rgba(0, 0, 0, 0.1);
        position: absolute;
        z-index: 1000;
    }

    .autocomplete-suggestion-item {
        padding: 8px;
        cursor: pointer;
    }

    .autocomplete-suggestion-item:hover, .autocomplete-suggestion-item-highlighted {
        background-color: #f0f0f0;
    }

    .autocomplete-suggestions::-webkit-scrollbar {
        width: 10px;
    }

    .autocomplete-suggestions::-webkit-scrollbar-thumb {
        background-color: #ddd;
        border-radius: 10px;
    }

    .autocomplete-suggestions::-webkit-scrollbar-track {
        background-color: #f9f9f9;
    }

    .autocomplete-suggestion-item {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <img src="../Images/logo2.JPG" alt="Logo" class="logo">
        <div class="container">
            <h2>
                <asp:Label ID="heading" runat="server" Text="Register" Font-Italic="True" Font-Names="Algerian"></asp:Label>
            </h2>
            <hr>
            <asp:TextBox ID="fullname" runat="server" placeholder="Full Name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="fullname" ErrorMessage="Full Name is required." ForeColor="Red" Display="Dynamic"/>
            <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="fullname" 
                ValidationExpression="^[a-zA-Z\s]*$" ErrorMessage="Full Name cannot contain numbers or special characters." ForeColor="Red" Display="Dynamic"/>
            <asp:TextBox ID="username" runat="server" placeholder="Username"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="username" ErrorMessage="Username is required." ForeColor="Red" Display="Dynamic" />
            <asp:CustomValidator ID="cvUsername" runat="server" ControlToValidate="username" OnServerValidate="ValidateUsername"
                ErrorMessage="Username already exists. Please choose a different username." ForeColor="Red" Display="Dynamic" />
            <asp:TextBox ID="pass" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="pass" ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic"/>
            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="pass" 
                ValidationExpression="^(?=.*[0-9].*[0-9])(?=.*[\W])(?=.*[^\s]).{8,11}$" 
                ErrorMessage="Password must be 8-11 characters, include at least 2 digits, 1 special character, and no spaces." ForeColor="Red" Display="Dynamic"/>

            <asp:TextBox ID="dob" placeholder="Date of Birth" runat="server" TextMode="Date"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDob" runat="server" ControlToValidate="dob" ErrorMessage="Date of Birth is required." ForeColor="Red" Display="Dynamic"/>
            
            <asp:TextBox ID="city" runat="server" placeholder="City"></asp:TextBox>
            <ajaxToolkit:AutoCompleteExtender ID="cityAutoCompleteExtender" runat="server"
                TargetControlID="city"
                ServiceMethod="GetCitySuggestions"
                MinimumPrefixLength="1"
                CompletionInterval="1000"
                EnableCaching="true"
                CompletionSetCount="10"
                CompletionListCssClass="autocomplete-suggestions"
                CompletionListItemCssClass="autocomplete-suggestion-item"
                CompletionListHighlightedItemCssClass="autocomplete-suggestion-item-highlighted"
                />
            
            
            <asp:TextBox ID="mobile" placeholder="Mobile Number" runat="server" TextMode="Phone"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="mobile" ErrorMessage="Mobile number is required." ForeColor="Red" Display="Dynamic"/>
            <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="mobile" 
                ValidationExpression="^\d{10}$" ErrorMessage="Mobile number must be 10 digits." ForeColor="Red" Display="Dynamic"/>
            
            <asp:Button ID="btnregister" runat="server" Text="Register" OnClick="GenerateOTP_Click" />
            <asp:Label ID="debugLabel" runat="server" Text="" ForeColor="Red" />
            <div class="login-link">
                <a href="userloginpg.aspx">Already have an account?</a>
            </div>
        </div>
    </form>
</body>
</html>
