<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="RailwayTicketWebsite.User.dashboard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Dashboard</title>
    <style>
        /* Styles for dashboard */
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }
        .container {
            display: flex;
            height: 100vh;
        }
        .sidebar {
            width: 200px;
            background-color: #2c3e50;
            color: white;
            padding: 20px;
            box-shadow: 2px 0 5px rgba(0,0,0,0.1);
        }
        .sidebar h2 {
            margin-top: 0;
        }
        .sidebar ul {
            list-style-type: none;
            padding: 0;
        }
        .sidebar ul li {
            margin: 15px 0;
            position: relative;
        }
        .sidebar ul li a {
            color: white;
            text-decoration: none;
        }
        .sidebar ul li:after {
            content: '';
            position: absolute;
            bottom: -10px;
            left: 0;
            width: 100%;
            border-bottom: 1px solid rgba(255, 255, 255, 0.2);
        }
        .sidebar .user-info {
            text-align: center;
            margin-bottom: 20px;
        }
        .sidebar .user-avatar {
            margin-bottom: 10px;
        }
        .sidebar .user-avatar .img {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            display: block;
            margin: 0 auto 10px;
        }
        .sidebar .user-info h3 {
            text-align: center;
            margin: 0;
        }
        .main-content {
            flex-grow: 1;
            padding: 20px;
        }
        .header {
            background-color: #2980b9;
            color: white;
            padding: 10px;
            text-align: center;
        }
        .search-container {
            display: flex;
            flex-wrap: nowrap;
            margin-top: 10px;
        }
        .search-container input[type="text"] {
            border: 1px solid #ddd;
            padding: 10px;
            border-radius: 5px 0 0 5px;
            outline: none;
            width: 100%;
            box-sizing: border-box;
        }
        .search-container input[type="submit"] {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 10px 20px;
            cursor: pointer;
            border-radius: 5px;
            width: auto;
            flex-shrink: 0;
        }
        .search-container input[type="submit"]:hover {
            background-color: #2980b9;
        }
        .btn-container {
            display: flex;
            flex-wrap: wrap;
            margin-top: 20px;
            justify-content: center;
        }
        .btn-container .styled-button {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 15px 25px;
            margin: 10px;
            cursor: pointer;
            border-radius: 5px;
            font-size: 16px;
            width: calc(50% - 20px);
        }
        .btn-container .styled-button:hover {
            background-color: #2980b9;
        }
        /* Hide submenu initially */
        .submenu {
            display: none;
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
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            $('.sidebar ul li').click(function () {
                $(this).children('.submenu').slideToggle();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container">
            <div class="sidebar">
                <h2>User Menu</h2>
                <div class="user-info">
                    <div class="user-name">
                        <asp:Label ID="uname" runat="server" Text="Label"></asp:Label>
                    </div>
                    <asp:Button ID="LoginButton" CssClass="styled-button" runat="server" Text="Login" OnClick="LoginButton_Click" Visible="false" />
                    <asp:Button ID="LogoutButton" CssClass="styled-button" runat="server" Text="Logout" OnClick="LogoutButton_Click" Style="display: none;" />
                </div>
                <ul>
                    <li>
                        <a href="#">Profile</a>
                        <ul class="submenu">
                            <li><a href="profile1.aspx">Edit Profile</a></li>
                            <li><a href="forgotpassword.aspx">Change Password</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <div class="main-content">
                <div class="header">
                    <h1>RailEase</h1>
                </div>
                <div class="search-container">
                    <asp:TextBox ID="srcTextBox" runat="server" CssClass="search-input" Placeholder="Source" onkeyup="validateStations()" />
                    <ajaxToolkit:AutoCompleteExtender ID="srcAutoCompleteExtender" runat="server"
                        TargetControlID="srcTextBox"
                        ServiceMethod="GetSourceSuggestions"
                        MinimumPrefixLength="1"
                        CompletionInterval="1000"
                        EnableCaching="true"
                        CompletionSetCount="10"
                        CompletionListCssClass="autocomplete-suggestions" />

                    <asp:TextBox ID="destTextBox" runat="server" CssClass="search-input no-border-radius" Placeholder="Destination" onkeyup="validateStations()" />
                    <ajaxToolkit:AutoCompleteExtender ID="destAutoCompleteExtender" runat="server"
                        TargetControlID="destTextBox"
                        ServiceMethod="GetDestinationSuggestions"
                        MinimumPrefixLength="1"
                        CompletionInterval="1000"
                        EnableCaching="true"
                        CompletionSetCount="10"
                        CompletionListCssClass="autocomplete-suggestions" />
                </div>

                <div class="btn-container">
                    <asp:Button ID="searchButton" CssClass="styled-button" runat="server" Text="Search Trains" OnClick="SearchButton_Click" Style="display: none;" />
                    <asp:Button ID="bookButton" CssClass="styled-button" runat="server" Text="Book Ticket" OnClick="BookButton_Click" Style="display: none;" />
                </div>

                <div class="btn-container">
                    <asp:Button ID="PlatformTicket" CssClass="styled-button" runat="server" Text="Platform Ticket" OnClick="PlatformTicket_Click" />
                    <asp:Button ID="PreviousBook" CssClass="styled-button" runat="server" Text="Previous Bookings" OnClick="PreviousBook_Click" />
                </div>
            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            $('.submenu').hide();
            $('.sidebar ul li').click(function () {
                var submenu = $(this).children('.submenu');
                if (!submenu.is(':visible')) {
                    $('.submenu').not(submenu).slideUp();
                    submenu.slideDown();
                }
            });
            $('.submenu').click(function (event) {
                event.stopPropagation();
            });
        });

        function validateStations() {
            var source = $('#<%= srcTextBox.ClientID %>').val();
            var destination = $('#<%= destTextBox.ClientID %>').val();

            if (source && destination) {
                $.ajax({
                    type: "POST",
                    url: "dashboard.aspx/ValidateStations",
                    data: JSON.stringify({ source: source, destination: destination }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d) {
                            $('#<%= searchButton.ClientID %>').show();
                            $('#<%= bookButton.ClientID %>').show();
                        } else {
                            $('#<%= searchButton.ClientID %>').hide();
                            $('#<%= bookButton.ClientID %>').hide();
                        }
                    }
                });
            } else {
                $('#<%= searchButton.ClientID %>').hide();
                $('#<%= bookButton.ClientID %>').hide();
            }
        }
    </script>
</body>
</html>