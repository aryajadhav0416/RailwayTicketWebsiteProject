<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit_fareslab.aspx.cs" Inherits="RailwayTicketWebsite.User.edit_fareslab" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Fare Slab</title>
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
            color: #dc3545; /* Bootstrap's red color for danger messages */
            font-size: 12px;
            margin-top: 5px;
            display: block;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Edit Fare Slab</h1>
            <div class="note">
                <p><u>Note</u> :<br />
                    <b>Ensure all fields are correctly filled before saving changes.</b></p>
            </div>
            <div class="form-group">
                <label for="txtMinDistance">Minimum Distance:</label>
                <asp:TextBox ID="txtMinDistance" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvMinDistance" runat="server" 
                    ControlToValidate="txtMinDistance" 
                    ErrorMessage="Minimum Distance is required." 
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RangeValidator ID="rvMinDistance" runat="server" 
                    ControlToValidate="txtMinDistance" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="Minimum Distance must be a positive number." 
                    CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtMaxDistance">Maximum Distance:</label>
                <asp:TextBox ID="txtMaxDistance" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvMaxDistance" runat="server" 
                    ControlToValidate="txtMaxDistance" 
                    ErrorMessage="Maximum Distance is required." 
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RangeValidator ID="rvMaxDistance" runat="server" 
                    ControlToValidate="txtMaxDistance" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="Maximum Distance must be a positive number." 
                    CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtSecondClassFare">Second Class Fare:</label>
                <asp:TextBox ID="txtSecondClassFare" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvSecondClassFare" runat="server" 
                    ControlToValidate="txtSecondClassFare" 
                    ErrorMessage="Second Class Fare is required." 
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RangeValidator ID="rvSecondClassFare" runat="server" 
                    ControlToValidate="txtSecondClassFare" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="Second Class Fare must be a positive number." 
                    CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtFirstClassFare">First Class Fare:</label>
                <asp:TextBox ID="txtFirstClassFare" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvFirstClassFare" runat="server" 
                    ControlToValidate="txtFirstClassFare" 
                    ErrorMessage="First Class Fare is required." 
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RangeValidator ID="rvFirstClassFare" runat="server" 
                    ControlToValidate="txtFirstClassFare" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="First Class Fare must be a positive number." 
                    CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtACFare">AC Fare:</label>
                <asp:TextBox ID="txtACFare" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvACFare" runat="server" 
                    ControlToValidate="txtACFare" 
                    ErrorMessage="AC Fare is required." 
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RangeValidator ID="rvACFare" runat="server" 
                    ControlToValidate="txtACFare" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="AC Fare must be a positive number." 
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
