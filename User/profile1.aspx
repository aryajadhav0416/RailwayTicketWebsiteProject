<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile1.aspx.cs" Inherits="RailwayTicketWebsite.User.profile1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profile</title>
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
            width: 500px;
            position: relative;
        }

        h2 {
            margin: 0 0 20px 0;
            text-align: center;
        }

        .form-group {
            margin-bottom: 15px;
        }

            .form-group label {
                display: block;
                margin-bottom: 5px;
            }

            .form-group input, .form-group select {
                width: calc(100% - 20px);
                padding: 10px;
                border: 1px solid #ccc;
                border-radius: 4px;
                box-sizing: border-box;
            }

        .edit-button {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 10px;
            border-radius: 5px;
            cursor: pointer;
            display: block;
            margin-top: 10px;
        }

        .edit-button-container {
            text-align: center;
        }

        .save-button, .cancel-button {
            width: 48%;
            padding: 10px;
            border: none;
            cursor: pointer;
            margin-top: 10px;
            display: none;
        }

        .save-button {
            background-color: #2ecc71;
            color: white;
            border-radius: 5px;
        }

        .cancel-button {
            background-color: #e74c3c;
            color: white;
            border-radius: 5px;
        }

        .container-links {
            display: flex;
            justify-content: center;
            margin-bottom: 20px;
        }

            .container-links span {
                margin: 0 10px;
                cursor: pointer;
            }

                .container-links span a {
                    color: #3498db;
                    font-weight: bold;
                    text-decoration: none;
                }

                    .container-links span a:visited {
                        color: #3498db;
                        text-decoration: none;
                    }

        .container-details {
            display: none;
        }

            .container-details.active {
                display: block;
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
        <div class="container">
            <h2>Profile</h2>
            <div id="personaldetails" class="container-details active">
                <h3>Personal Details</h3>
                <div class="form-group">
                    <label for="txtName">Name</label>
                    <asp:TextBox ID="fullname" runat="server" placeholder="Full Name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="fullname" ErrorMessage="Full Name is required." ForeColor="Red" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="fullname"
                        ValidationExpression="^[a-zA-Z\s]*$" ErrorMessage="Full Name cannot contain numbers or special characters." ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label for="txtCity">City</label>
                    <asp:TextBox ID="txtCity" runat="server" placeholder="City"></asp:TextBox>
                    <ajaxToolkit:AutoCompleteExtender ID="cityAutoCompleteExtender" runat="server"
                        TargetControlID="txtCity"
                        ServiceMethod="GetCitySuggestions"
                        MinimumPrefixLength="1"
                        CompletionInterval="1000"
                        EnableCaching="true"
                        CompletionSetCount="10"
                        CompletionListCssClass="autocomplete-suggestions"
                        CompletionListItemCssClass="autocomplete-suggestion-item"
                        CompletionListHighlightedItemCssClass="autocomplete-suggestion-item-highlighted" />
                </div>
                <div class="form-group">
                    <label for="txtDOB">Date of Birth</label>
                    <asp:TextBox ID="txtdob" placeholder="Date of Birth" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDob" runat="server" ControlToValidate="txtdob" ErrorMessage="Date of Birth is required." ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label for="txtPhone">Phone</label>
                    <asp:TextBox ID="mobile" placeholder="Mobile Number" runat="server" TextMode="Phone"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="mobile" ErrorMessage="Mobile number is required." ForeColor="Red" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="mobile"
                        ValidationExpression="^\d{10}$" ErrorMessage="Mobile number must be 10 digits." ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="edit-button-container">
                    <asp:Button ID="editButton" runat="server" Text="Edit" CssClass="edit-button" OnClick="editButton_Click" />
                    <asp:Button ID="saveButton" runat="server" Text="Save" CssClass="save-button" OnClick="SavePersonalDetails_Click" />
                    <asp:Button ID="cancelButton" runat="server" Text="Cancel" CssClass="cancel-button" OnClick="CancelPersonalDetails_Click" />
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
