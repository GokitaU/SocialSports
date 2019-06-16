<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlNflSchedule.ascx.cs" Inherits="SocialSports.ctrlNflSchedule" %>





<asp:Calendar ID="Calendar1" SelectWeekText="Select Week" SelectionMode="DayWeek"   runat="server" BackColor="White" OnSelectionChanged="Calendar1_SelectionChanged" BorderColor="#1C5E55" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#1C5E55" Height="190px" NextPrevFormat="FullMonth" Width="350px">

    
     <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="Black"></DayHeaderStyle>

    <NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="Black"></NextPrevStyle>

    <OtherMonthDayStyle ForeColor="Black"></OtherMonthDayStyle>

    <SelectedDayStyle BackColor="#1C5E55" ForeColor="White"></SelectedDayStyle>

    <TitleStyle BackColor="White" BorderColor="#1C5E55" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="Black"></TitleStyle>

</asp:Calendar>


    <asp:GridView ID="gvNflSchedule" runat="server" AutoGenerateColumns="false" OnRowCommand="gvNflSchedule_RowCommand" OnRowDataBound="gvNflSchedule_RowDataBound" OnDataBound="gvNflSchedule_DataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
        <Columns>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Image ID="imgHome" runat="server" Width="40px" Height="40px" ImageUrl='<%# "~/" + Eval("VisitorImage") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    VS
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Image ID="imgVisitor" runat="server" Width="40px" Height="40px" ImageUrl='<%# "~/" + Eval("HomeImage") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    During Week <b><%# Eval("WeekNumber") %></b>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lbViewWeeksBets" runat="server" Font-Size="Small" PostBackUrl='<%# "~/SearchBets.aspx?WeekNumber=" + Eval("WeekNumber") + "&HomeTeam=" + Eval("HomeTeam") + "&VisitorTeam=" + Eval("VisitorTeam") %>'>View Bets</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lbViewPlayers" runat="server" Font-Size="Small" CommandArgument='<%# Eval("HomeTeam") +  "-" + Eval("VisitorTeam") %>'>View Players</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#7C6F57"></EditRowStyle>

        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></FooterStyle>

        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></HeaderStyle>

        <PagerStyle HorizontalAlign="Center" BackColor="#666666" ForeColor="White"></PagerStyle>

        <RowStyle BackColor="#E3EAEB"></RowStyle>

        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

        <SortedAscendingCellStyle BackColor="#F8FAFA"></SortedAscendingCellStyle>

        <SortedAscendingHeaderStyle BackColor="#246B61"></SortedAscendingHeaderStyle>

        <SortedDescendingCellStyle BackColor="#D4DFE1"></SortedDescendingCellStyle>

        <SortedDescendingHeaderStyle BackColor="#15524A"></SortedDescendingHeaderStyle>
    </asp:GridView>


   <asp:GridView ID="gvPlayers" runat="server" AutoGenerateColumns="false" >
        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Image ID="imgPlayer" runat="server" Width="40px" Height="40px" ImageUrl='<%# "~/" + Eval("PlayerImage") %>' />
                </ItemTemplate>
            </asp:TemplateField>
   <asp:BoundField DataField="PlayerName" />
   <asp:BoundField DataField="PlayerPosition" />
          <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lbViewPlayerBets" runat="server" Font-Size="Small" PostBackUrl='<%# "~/SearchBets.aspx?PlayerName=" + Eval("PlayerName") %>'>View Bets</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>        
        </Columns>

        <EditRowStyle BackColor="#7C6F57"></EditRowStyle>

        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></FooterStyle>

        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"></HeaderStyle>

        <PagerStyle HorizontalAlign="Center" BackColor="#666666" ForeColor="White"></PagerStyle>

        <RowStyle BackColor="#E3EAEB"></RowStyle>

        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

        <SortedAscendingCellStyle BackColor="#F8FAFA"></SortedAscendingCellStyle>

        <SortedAscendingHeaderStyle BackColor="#246B61"></SortedAscendingHeaderStyle>

        <SortedDescendingCellStyle BackColor="#D4DFE1"></SortedDescendingCellStyle>

        <SortedDescendingHeaderStyle BackColor="#15524A"></SortedDescendingHeaderStyle>
    </asp:GridView>

