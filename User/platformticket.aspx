<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="platformticket.aspx.cs" Inherits="RailwayTicketWebsite.User.platformticket" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Platform Booking</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 400px;
            margin: 50px auto;
            background: #fff;
            padding: 20px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
        }
        .header {
            background-color: #ff6f61;
            padding: 10px;
            border-radius: 8px 8px 0 0;
            text-align: center;
            color: #fff;
        }
        label {
            display: block;
            margin: 10px 0 5px;
            font-weight: bold;
        }
        input[type="text"], select {
            width: 93%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
        .btn {
            width: 100%;
            padding: 15px;
            background-color: #ff6f61;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 16px;
            cursor: pointer;
        }
        .btn:hover {
            background-color: #e65a50;
        }
        .radio-group {
            display: flex;
            justify-content: space-between;
            margin-bottom: 20px;
        }
        .radio-group label {
            margin-right: 10px;
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
            <div class="header">
                <h2>PLATFORM BOOKING</h2>
            </div>
            <label for="stationName">Station Name / Code</label>
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
            <label for="ddlPersons">Person(s)</label>
            <asp:DropDownList ID="ddlPersons" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPersons_SelectedIndexChanged">
                <asp:ListItem Value="1">ONE (1)</asp:ListItem>
                <asp:ListItem Value="2">TWO (2)</asp:ListItem>
                <asp:ListItem Value="3">THREE (3)</asp:ListItem>
                <asp:ListItem Value="4">FOUR (4)</asp:ListItem>
            </asp:DropDownList>
            <label for="ddlPaymentType">Payment Type</label>
            <asp:DropDownList ID="ddlPaymentType" runat="server">
                <asp:ListItem Value="CARD">CARD</asp:ListItem>
                <asp:ListItem Value="UPI">UPI</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblTotalFare" runat="server" CssClass="total-fare-label"></asp:Label>
            <asp:Button ID="btnBookTicket" runat="server" Text="BOOK TICKET" CssClass="btn" OnClick="btnBookTicket_Click" />
            
        <asp:Label ID="lblError" runat="server" CssClass="total-fare-label" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
