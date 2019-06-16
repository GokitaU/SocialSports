<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="BetBrilliantly.aspx.cs" Inherits="SocialSports.BetBrilliantly" %>

<%@ Register Src="~/UserControls/ctrlNflSchedule.ascx" TagPrefix="uc1" TagName="ctrlNflSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


       <div class="container-fluid">
              <div class="row">
                 
                                      <div class="col-sm-2">
                                       
                                          <div class="well well-sm" style="border-color:#1C5E55;  ">
                                             
                                             <h4 style="font-weight:bold; text-align:center; font-family:'Times New Roman';" >Most Popular Teams</h4>  

                                              <hr />

                                      <asp:ListView ID="ListView1" runat="server">
                                            <ItemTemplate>


                                                       <div style="text-align:center;">
                                  
                                                       <%# Eval("TeamName") %> 
                                                       <br />
                                                          Number of Bets  <%# Eval("TeamCount") %> 

                                                              <br />

                                                           <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("TeamImage") %>' Width="50" Height="50" />
                                                                                                                         <br />


                                          <asp:LinkButton ID="lbTeam" runat="server" PostBackUrl='<%# "SearchBets.aspx?Team=" + Eval("Team") %>'>View <%# Eval("TeamName") %>'s Bets</asp:LinkButton> 
                                                      

                             


                                                       <hr />
                                                           </div>

                                         </ItemTemplate>
                                 </asp:ListView>    
                                          
                                          
                                          
                                          </div>
                                          
                                          
                                          
                      <asp:ListView ID="lvMostPopularTeams" runat="server">
                           <ItemTemplate>


                               <div style="text-align:center;">
                                  
                               <%# Eval("strTeam") %> 
                               <br />


                               <br />
                                <%# Eval("strLogicArgument") %>
                               <br />

                                 <%# Eval("decArgumentValue") %>
                             

                               <%# Eval("fkStatType") %>

                                                               During Week #
                               <%# Eval("intWeekNumber") %>

                               <br />

                             

                               <b style="color:green;">$ <%# Eval("decMoneyWager", "{0:c}") %></b>



                               <hr />
                                   </div>
                           </ItemTemplate>
                       </asp:ListView>


                                          </div>

                     <div class="col-sm-1">
                       
                        </div>


                   <div class="col-sm-6">


                  
                        <div class="well well-sm" style="color:#1C5E55; border-bottom: 3px #e5e5e5 solid;border-right: 3px #e5e5e5 solid; border-left: 3px #e5e5e5 solid; ">
                             

                             <asp:Panel ID="pnlLoginIsSuccessful" runat="server" BorderColor="DarkGreen">
                                <div class="alert alert-success" style="text-align:center;">
                                         <strong>Log out was successful!</strong> 
                                </div>
                          </asp:Panel>



 <div id="CarouselFeatured" class="carousel slide" data-ride="carousel">

                    <ol class="carousel-indicators">
                      <li data-target="#CarouselFeatured" data-slide-to="0" class="active"></li>
                      <li data-target="#CarouselFeatured" data-slide-to="1"></li>
                      <li data-target="#CarouselFeatured" data-slide-to="2"></li>
                      <li data-target="#CarouselFeatured" data-slide-to="3"></li>
                    </ol>


<div class="carousel-inner" role="listbox" >
    <div class="item active" style="margin-left:140px;">

  
                      <h3 style="text-align:center;">Recently Created Bets</h3>
    
     <asp:GridView ID="gvRecentlyCreatedBets" runat="server" Width="80%"  AutoGenerateColumns="false" AllowSorting="false" CellPadding="4" ForeColor="#333333" GridLines="None"  OnRowDataBound="gvBets_RowDataBound" >
                         <Columns>
                                <asp:TemplateField>
                                       <ItemTemplate>
                                          <asp:LinkButton ID="lbViewBet" runat="server" >View Bet</asp:LinkButton>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                <asp:BoundField DataField="Name"  HeaderText="Team"/>
                                   <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Image ID="img" runat="server" Width="40" Height="40" />
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                   <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                                   <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                                   <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                                   <asp:BoundField DataField="Wager"  HeaderText="$" DataFormatString="{0:c}" ItemStyle-ForeColor="Green" ItemStyle-Font-Bold="true"/>

                                  <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Label ID="lblBetId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                 <asp:TemplateField>
                                       <ItemTemplate>
                                           During Week
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="WeekNumber"  HeaderText="Week"/>
                             </Columns>

                               <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>

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








    </div>
    <div class="item"  style="margin-left:140px;">

           <h3 style="text-align:center;">Most Viewed Bets</h3>
    
      <asp:GridView ID="gvMostViewedBets" runat="server" Width="80%"  AutoGenerateColumns="false" AllowSorting="false" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvBets_RowDataBound" >
                         <Columns>
                                <asp:TemplateField>
                                       <ItemTemplate>
                                          <asp:LinkButton ID="lbViewBet" runat="server" >View Bet</asp:LinkButton>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                <asp:BoundField DataField="Name"  HeaderText="Team"/>
                                   <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Image ID="img" runat="server" Width="40" Height="40" />
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                   <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                                   <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                                   <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                                 <asp:BoundField DataField="Wager"  HeaderText="$" DataFormatString="{0:c}" ItemStyle-ForeColor="Green" ItemStyle-Font-Bold="true"/>
                                  <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Label ID="lblBetId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                 <asp:TemplateField>
                                       <ItemTemplate>
                                           During Week
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="WeekNumber"  HeaderText="Week"/>
                             </Columns>

                               <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>

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

        </div>

     <div class="item"  style="margin-left:140px;">

           <h2>Highest Wagers</h2>
    
      <asp:GridView ID="gvHighestWagers" runat="server" Width="80%"  AutoGenerateColumns="false" AllowSorting="false" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvBets_RowDataBound" >
                         <Columns>
                                <asp:TemplateField>
                                       <ItemTemplate>
                                          <asp:LinkButton ID="lbViewBet" runat="server" >View Bet</asp:LinkButton>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                <asp:BoundField DataField="Name"  HeaderText="Team"/>
                                   <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Image ID="img" runat="server" Width="40" Height="40" />
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                   <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                                   <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                                   <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                                   <asp:BoundField DataField="Wager"  HeaderText="$" DataFormatString="{0:c}" ItemStyle-ForeColor="Green" ItemStyle-Font-Bold="true"/>

                                  <asp:TemplateField>
                                       <ItemTemplate>
                                           <asp:Label ID="lblBetId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                       </ItemTemplate>
                                 </asp:TemplateField>    
                                 <asp:TemplateField>
                                       <ItemTemplate>
                                           During Week
                                       </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="WeekNumber"  HeaderText="Week"/>
                             </Columns>

                               <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>

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

        </div>




                    <a class="left carousel-control" href="#CarouselFeatured" role="button" data-slide="prev">
                      <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                      <span class="sr-only">Previous</span>
                    </a>
                    <a class="right carousel-control" href="#CarouselFeatured" role="button" data-slide="next">
                      <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                      <span class="sr-only">Next</span>
                    </a>
                  </div>
     </div>














                            <uc1:ctrlNflSchedule runat="server" ID="ctrlNflSchedule" />






                     


                           

                            </div>

                    
                              


                                








                       </div>
                  <div class="col-sm-1">



                                          </div>
                  <div class="col-sm-2">
                           <div class="well well-sm" style="color:black;  border:#1C5E55; ">

                                             <h4 style="font-weight:bold; text-align:center; font-family:'Times New Roman';" >Most Popular Players</h4>  
                             
                                     <hr />
                  
                              <asp:ListView ID="lvPopularPlayers" runat="server">
                                            <ItemTemplate>


                                                       <div style="text-align:center;">
                                  
                                                       <%# Eval("PlayerName") %> 
                                                       <br />
                                                          Number of Bets  <%# Eval("PlayerCount") %> 

                                                              <br />

                                                           <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("PlayerImage") %>' Width="50" Height="50" />
                                                                                                                         <br />
                

                                          <asp:LinkButton ID="lbPlayer" runat="server" PostBackUrl='<%# "SearchBets.aspx?PlayerTeam=" + Eval("PlayerTeam") + "&Player=" + Eval("Player") %>'>View <%# Eval("PlayerName") %>'s Bets</asp:LinkButton> 
                                                      

                             


                                                       <hr />
                                                           </div>

                                         </ItemTemplate>
                                 </asp:ListView>    





                              </div>
                        </div>
                  </div>
                
</div>

</asp:Content>
