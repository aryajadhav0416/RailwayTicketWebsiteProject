<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="RailwayTicketWebsite.User.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
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
    </style>
    <script>
        function showContainer(containerId) {
            document.querySelectorAll('.container-details').forEach(container => {
                container.classList.remove('active');
            });
            document.getElementById(containerId).classList.add('active');
            resetFormFields();
        }

        function resetFormFields() {
            document.querySelectorAll('.input').forEach(input => input.disabled = true);
            document.querySelectorAll('.save-button, .cancel-button').forEach(button => button.style.display = 'none');
            document.querySelectorAll('.edit-button').forEach(button => button.style.display = 'block');
        }

        function toggleEditPersonal() {
            document.querySelectorAll('#personaldetails .input').forEach(input => input.disabled = !input.disabled);
            document.getElementById('<%= saveButton.ClientID %>').style.display = 'block';
            document.getElementById('<%= cancelButton.ClientID %>').style.display = 'block';
            document.getElementById('<%= editButton.ClientID %>').style.display = 'none';
        }

        function toggleEditContact() {
            document.querySelectorAll('#contactdetails .input').forEach(input => input.disabled = !input.disabled);
            document.getElementById('<%= saveButton2.ClientID %>').style.display = 'block';
            document.getElementById('<%= cancelButton2.ClientID %>').style.display = 'block';
            document.getElementById('<%= editButton2.ClientID %>').style.display = 'none';
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Profile</h2>
            <div class="container-links">
                <span onclick="showContainer('personaldetails');"><a href="#">Personal Details</a></span>
                <span onclick="showContainer('contactdetails');"><a href="#">Contact Details</a></span>
            </div>

            <div id="personaldetails" class="container-details active">
                <h3>Personal Details</h3>
                <div class="form-group">
                    <label for="txtName">Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtCity">City</label>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtDOB">Date of Birth</label>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="input" Enabled="false" TextMode="Date"></asp:TextBox>
                </div>
                <div class="edit-button-container">
                    <asp:Button ID="editButton" runat="server" Text="Edit" CssClass="edit-button" OnClientClick="toggleEditPersonal(); return false;" />
                    <asp:Button ID="saveButton" runat="server" Text="Save" CssClass="save-button" OnClick="SavePersonalDetails_Click" />
                    <asp:Button ID="cancelButton" runat="server" Text="Cancel" CssClass="cancel-button" OnClick="CancelPersonalDetails_Click" />
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" />
                </div>
            </div>

            <div id="contactdetails" class="container-details">
                <h3>Contact Details</h3>
                <div class="form-group">
                    <label for="txtPhone">Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                </div>
                <div class="edit-button-container">
                    <asp:Button ID="editButton2" runat="server" Text="Edit" CssClass="edit-button" OnClientClick="toggleEditContact(); return false;" />
                    <asp:Button ID="saveButton2" runat="server" Text="Save" CssClass="save-button" OnClick="SaveContactDetails_Click" />
                    <asp:Button ID="cancelButton2" runat="server" Text="Cancel" CssClass="cancel-button" OnClick="CancelContactDetails_Click" />
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
