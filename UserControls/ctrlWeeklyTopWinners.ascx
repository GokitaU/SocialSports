<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlWeeklyTopWinners.ascx.cs" Inherits="SocialSports.UserControls.ctrlWeeklyTopWinners" %>


<asp:ListView ID="lvWeeklyWinners" runat="server">
    <ItemTemplate>

        <%# Eval("fkUserIdWinner") %>

    </ItemTemplate>
</asp:ListView>
