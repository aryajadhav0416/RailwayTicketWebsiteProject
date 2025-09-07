<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userloginpg.aspx.cs" Inherits="RailwayTicketWebsite.User.userloginpg" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Railway Login</title>
    <link rel="stylesheet" type="text/css" href="css/styles.css">
    <!-- Link to your CSS file -->
    <style>
        /* General Styles */
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }

        /* Container Styles */
        .container {
            height: 530px;
            max-width: 1000px;
            margin: 40px auto;
            display: flex;
            background-color: #f2f2f2;
            border-radius: 40px;
            box-shadow: 0px 0px 10px 0px rgba(0,100,100,0.6);
        }

        .login-container {
            flex: 1;
            padding: 30px;
            border-top-right-radius: 40px;
            border-bottom-right-radius: 40px;
        }

        .img-container {
            flex: 1;
            background-color: white;
            border-top-left-radius: 40px;
            border-bottom-left-radius: 40px;
            display: flex;
            justify-content: center; /* Aligns horizontally */
            align-items: center; /* Aligns vertically */
        }

        #logoApp {
            width: 360px;
            height: 100px;
            background-color: yellow;
            object-fit: cover;
            border-radius: 20%;
            margin: 20px;
        }

        h2 {
            text-align: center;
            margin-bottom: 50px;
        }

        /* Input Styles */
        .input-container {
            width: 100%;
            margin-bottom: 30px;
        }

            .input-container input {
                width: 100%;
                padding: 15px;
                border: 1px solid #ccc;
                border-radius: 5px;
                box-sizing: border-box;
            }

        #btnlogin {
            width: 100%;
            padding: 15px;
            background-color: #007bff;
            border: none;
            color: #fff;
            border-radius: 5px;
            cursor: pointer;
            font-size: 20px;
        }

        .forgot-password,
        .signup-link {
            text-align: center;
            margin-top: 20px;
        }

            .forgot-password a,
            .signup-link a {
                color: #007bff;
                text-decoration: none;
            }

                .forgot-password a:hover,
                .signup-link a:hover {
                    text-decoration: underline;
                }

        .signup-link {
            margin-top: 10px;
        }
    </style>
    <!-- Add this script inside the <head> tag -->
    

</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="img-container">
                <asp:Image ID="logoApp" CssClass=".img" AlternateText="RailEase Image" runat="server" ImageUrl="~/Images/logo2.JPG" />
            </div>
            <div class="login-container">
                <h2>Login</h2>
                <div class="input-container">
                    <asp:TextBox ID="uname" runat="server" CssClass="form-control" placeholder="Username or Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUname" runat="server" ControlToValidate="uname" ErrorMessage="Username or Email is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="input-container">
                    <asp:TextBox ID="pass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="pass" ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="revPass" runat="server" ControlToValidate="pass" ErrorMessage="Password must be at least 6 characters long." ForeColor="Red" Display="Dynamic" ValidationExpression=".{4,}"></asp:RegularExpressionValidator>
                </div>
                <asp:Button ID="btnlogin" runat="server" CssClass="btn btn-primary" Text="Login" OnClick="GenerateOTP_Click" UseSubmitBehavior="False" />
                <asp:Label ID="debugLabel" runat="server" Text="" ForeColor="Red" />
                <div class="forgot-password">
                    <a id="forgotdetails" href="forgotpassword.aspx">Forgot Details?</a>
                </div>
                <div class="signup-link">
                    <a id="Register" href="registerpg.aspx">Register</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
