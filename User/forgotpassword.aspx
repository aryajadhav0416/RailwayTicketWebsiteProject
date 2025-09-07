<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpassword.aspx.cs" Inherits="RailwayTicketWebsite.User.forgotpassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Forgot Password</title>
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
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            width: 400px;
            text-align: center;
        }

            .container h2 {
                margin-bottom: 10px;
                text-align: center;
            }

            .container hr {
                margin: 20px 0;
                border: 0;
                border-top: 1px solid #ccc;
                width: calc(100% + 40px);
                position: relative;
                left: -20px;
            }

        .form-group {
            margin-bottom: 15px;
        }

            .form-group input {
                width: 100%;
                padding: 10px;
                border: 1px solid #ccc;
                border-radius: 4px;
                box-sizing: border-box;
            }

        .btn {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 10px;
            text-align: center;
            cursor: pointer;
            width: 100%;
            border-radius: 4px;
            font-size: 16px;
        }

            .btn:hover {
                background-color: #2980b9;
            }

        .note {
            font-size: 14px;
            color: #555;
            height:40px;
            text-align: left;
            margin-top: -10px;
            margin-bottom: 10px;
            padding: 10px;
            background-color: #91ed76;
            border-radius: 4px;
            border-left: 4px solid #3498db;
        }
        .note p{
            margin-top: -5px;
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Forgot / Update Password</h2>
            <hr />
            <div class="note">
                <p><u>Note</u> :<br />Before reseting password first find account. After finding account reset password by setting new password.</p>
            </div>
            <div class="form-group">
                <asp:TextBox ID="uname" runat="server" CssClass="form-control" placeholder="Enter Username, Email or Phone"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUname" runat="server" ControlToValidate="uname" ErrorMessage="Username or Email is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <asp:Button ID="btnfindacc" CssClass="btn" runat="server" Text="Generate OTP" OnClick="GenerateOTP_Click" />
            <asp:Label ID="debugLabel" runat="server" Text="" ForeColor="Red" />
        </div>
    </form>
</body>
</html>

