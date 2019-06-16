<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="SearchBets.aspx.cs" Inherits="SocialSports.SearchBets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


     <div class="container-fluid">
              <div class="row">
                   <div class="col-sm-2">
                         <div class="col-sm-9">

                       <div class="well well-sm"  style="color:black; background-color:white; border-bottom: 3px #e5e5e5 solid;border-right: 3px #e5e5e5 solid; border-left: 3px #e5e5e5 solid; ">
                   Bets

                           <asp:Label ID="lblStats" runat="server" ></asp:Label>


                               <asp:RadioButtonList ID="rdoTypeOfBet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoTypeOfBet_SelectedIndexChanged" RepeatDirection="Vertical" Font-Bold="false" Font-Size="Small"  >
                                <asp:ListItem>Team Bet</asp:ListItem>
                                <asp:ListItem>Player Bet</asp:ListItem>
                              </asp:RadioButtonList>

                           <asp:LinkButton ID="lnkClearSelections" runat="server" OnClick="lnkClearSelections_Click">Clear Selections</asp:LinkButton>
                         
                             <asp:Panel ID="pnlTeamBets" runat="server">

                               Select Teams:
                               <br />
                               <i style="font-size:x-small;">Hold Ctrl For Multi-Select</i>
                                  <asp:ListBox ID="lbTeams" runat="server" SelectionMode="Multiple" Rows="16" Font-Size="Small" CssClass="form-control">
                                  </asp:ListBox>
                               Select Stats:
                               <br />
                                <i style="font-size:x-small;">Hold Ctrl For Multi-Select</i>
                                  <asp:ListBox ID="lbTeamStatArguments" runat="server" SelectionMode="Multiple" Font-Size="Small" Rows="6" CssClass="form-control">
                                  </asp:ListBox>   

                                Select Logic:
                               <br />

                               <asp:DropDownList ID="ddlTeamLogic" runat="server" CssClass="form-control" Font-Size="Small">
                                      <asp:ListItem Value="Any Logic">Any Logic</asp:ListItem>
                                      <asp:ListItem Value="Will Have More Than">Will Have More Than</asp:ListItem>
                                      <asp:ListItem Value="Will Have Equal To">Will Have Equal To</asp:ListItem>
                                      <asp:ListItem Value="Will Have Less Than">Will Have Less Than</asp:ListItem>
                               </asp:DropDownList>   

                               Argument Value:
                               <br />
                                  <asp:TextBox ID="txtTeamArgumentValue" runat="server" CssClass="form-control" Font-Size="Small"></asp:TextBox>

                                  Wager                             
                               Between
                               <asp:TextBox ID="txtWagerStart" runat="server" CssClass="form-control"></asp:TextBox>
                               And
                               <asp:TextBox ID="txtWagerEnd" runat="server" CssClass="form-control"></asp:TextBox>

                                       Week #           
                                               
                                <asp:DropDownList ID="ddlTeamWeekNumber" runat="server" CssClass="form-control" Font-Size="Small">
                                      <asp:ListItem Value="Any Week">Any Week</asp:ListItem>
                                      <asp:ListItem Value="1">1</asp:ListItem>
                                      <asp:ListItem Value="2">2</asp:ListItem>
                                      <asp:ListItem Value="3">3</asp:ListItem>
                                      <asp:ListItem Value="4">4</asp:ListItem>
                                      <asp:ListItem Value="5">5</asp:ListItem>
                                      <asp:ListItem Value="6">6</asp:ListItem>
                                      <asp:ListItem Value="7">7</asp:ListItem>
                                      <asp:ListItem Value="8">8</asp:ListItem>
                                      <asp:ListItem Value="9">9</asp:ListItem>
                                      <asp:ListItem Value="10">10</asp:ListItem>
                                      <asp:ListItem Value="11">11</asp:ListItem>
                                      <asp:ListItem Value="12">12</asp:ListItem>
                                      <asp:ListItem Value="13">13</asp:ListItem>
                                      <asp:ListItem Value="14">14</asp:ListItem>
                                      <asp:ListItem Value="15">15</asp:ListItem>
                                      <asp:ListItem Value="16">16</asp:ListItem>
                                      <asp:ListItem Value="17">17</asp:ListItem>             
                               </asp:DropDownList>   

                               Only Open Bets:
                               <asp:CheckBox ID="chkIsOpened" runat="server" />
                               
                               <asp:Button ID="btnSearchTeamBets" runat="server" Text="Search Team Bets" OnClick="btnSearchTeamBets_Click"  Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%"/>




                           </asp:Panel>
                           <asp:Panel ID="pnlPlayerBets" runat="server">

                                 Select Player Team:
                               <asp:DropDownList ID="ddlPlayersTeam" runat="server" Font-Size="Small" CssClass="form-control" OnSelectedIndexChanged="ddlPlayersTeam_SelectedIndexChanged" AutoPostBack="true">
                                   <asp:ListItem>Select Team</asp:ListItem>
                               </asp:DropDownList>

                                 
                               Select Players:
                                       <br />
                                <i style="font-size:x-small;">Hold Ctrl For Multi-Select</i>
                                 
                               <asp:ListBox ID="lbPlayers" runat="server" SelectionMode="Multiple" Rows="16" Font-Size="Small" CssClass="form-control" OnSelectedIndexChanged="lbPlayers_SelectedIndexChanged" AutoPostBack="true">
                                  </asp:ListBox>

                               Select Logical Argument:
                                       <br />

                                     <asp:DropDownList ID="ddlPlayerLogic" runat="server" CssClass="form-control" Font-Size="Small">
                                      <asp:ListItem>Any Logic</asp:ListItem>
                                      <asp:ListItem>Will Have More Than</asp:ListItem>
                                      <asp:ListItem Value="Will Have Equal To">Will Have Equal To</asp:ListItem>
                                      <asp:ListItem>Will Have Less Than</asp:ListItem>
                               </asp:DropDownList>      

                                  Argument Value:
                                  <asp:TextBox ID="txtPlayerArgumentValue" runat="server" CssClass="form-control" Font-Size="Small"></asp:TextBox>




                               Select Stat Argument:
                                       <br />
                               <i style="font-size:x-small;">Hold Ctrl For Multi-Select</i>

                                <asp:ListBox ID="lbPlayerStatArgument" runat="server" SelectionMode="Multiple" Font-Size="Small" Rows="6" CssClass="form-control">
                                </asp:ListBox>   

                                
                               <br />
                            Wager  Between
                               <asp:TextBox ID="txtPlayerWageStart" runat="server" CssClass="form-control"></asp:TextBox>
                               And
                               <asp:TextBox ID="txtPlayerWageEnd" runat="server" CssClass="form-control"></asp:TextBox>
   

                            
                               <br />

                               Week #           
                                               
                                <asp:DropDownList ID="ddlPlayerWeekNumber" runat="server" CssClass="form-control" Font-Size="Small">
                                      <asp:ListItem Value="Any Week">Any Week</asp:ListItem>
                                      <asp:ListItem Value="1">1</asp:ListItem>
                                      <asp:ListItem Value="2">2</asp:ListItem>
                                      <asp:ListItem Value="3">3</asp:ListItem>
                                      <asp:ListItem Value="4">4</asp:ListItem>
                                      <asp:ListItem Value="5">5</asp:ListItem>
                                      <asp:ListItem Value="6">6</asp:ListItem>
                                      <asp:ListItem Value="7">7</asp:ListItem>
                                      <asp:ListItem Value="8">8</asp:ListItem>
                                      <asp:ListItem Value="9">9</asp:ListItem>
                                      <asp:ListItem Value="10">10</asp:ListItem>
                                      <asp:ListItem Value="11">11</asp:ListItem>
                                      <asp:ListItem Value="12">12</asp:ListItem>
                                      <asp:ListItem Value="13">13</asp:ListItem>
                                      <asp:ListItem Value="14">14</asp:ListItem>
                                      <asp:ListItem Value="15">15</asp:ListItem>
                                      <asp:ListItem Value="16">16</asp:ListItem>
                                      <asp:ListItem Value="17">17</asp:ListItem>             
                               </asp:DropDownList>  

                                 Only Open Bets:
                               <asp:CheckBox ID="chkPlayerOnlyOpened" runat="server" />

                             <asp:Button ID="btnSearchPlayerBets" runat="server" Text="Search Player Bets" OnClick="btnSearchPlayerBets_Click" Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%"/>


                           </asp:Panel>

                           </div>
                               <div class="col-sm-3">

                                   </div>
                         </div>
                        </div>
                  <div class="col-sm-1">


                        </div>
                   <div class="col-sm-6">
                       <div class="well well-lg" style="background-color:white; color:#1C5E55; border: 3px #1C5E55 solid; ">

                               <asp:Panel ID="pnlBetAccepted" runat="server" BorderColor="DarkRed">

                                   <div class="alert alert-success" style="text-align:center;">
                                          <strong>
                                              Bet Accepted!
                                          </strong> 
                                    </div>
                                </asp:Panel>     
                                                                    
                                 <asp:Panel ID="pnlMustBeLoggedId" runat="server" BorderColor="DarkRed">

                                                   <div class="alert alert-danger" style="text-align:center;">
                                                          <strong>
                                                              You Must Be Logged In. Click <a href="Login.aspx">Here</a> To Login.
                                                          </strong> 
                                                 </div>
                                  </asp:Panel>

                        
                            
                           
                             <asp:Panel ID="pnlTeamBetsGv" runat="server"> 
                                 

                                <asp:Label ID="lblTeamResultsCount" runat="server" Font-Size="Large" ></asp:Label>

                                 <div class="col-sm-3">

                                  <asp:DropDownList ID="ddlTeamBetPageSize" runat="server"  CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamBetPageSize_SelectedIndexChanged" >                                    
                                      <asp:ListItem>10</asp:ListItem>
                                      <asp:ListItem>25</asp:ListItem>
                                      <asp:ListItem>50</asp:ListItem>
                                      <asp:ListItem>100</asp:ListItem>
                                 </asp:DropDownList>
                                </div>
                                                              
                                  <div class="col-sm-3">

                           <asp:DropDownList ID="ddlTeamSortByTop" runat="server"  CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamSortByTop_SelectedIndexChanged" >
                                   <asp:ListItem>Sort By</asp:ListItem>             
                                   <asp:ListItem>Wager Amount</asp:ListItem>
                                   <asp:ListItem>Team</asp:ListItem>
                                   <asp:ListItem>Logic Argument</asp:ListItem>
                                   <asp:ListItem>Statistical Argument</asp:ListItem>
                                   <asp:ListItem>Argument Value</asp:ListItem>
                           </asp:DropDownList>

                                                           <br />

                                </div>
                                 <div class="col-sm-3">

                           <asp:DropDownList ID="ddlTeamAscOrDescTop" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamAscOrDescTop_SelectedIndexChanged" >
                               <asp:ListItem Value="DESC">Highest To Lowest</asp:ListItem>
                               <asp:ListItem Value="ASC">Lowest To Highest</asp:ListItem>
                           </asp:DropDownList>

                                                           <br />
                                </div>
                              <div class="col-sm-3">
                              
                                        </div>


                       <asp:GridView ID="gvTeamBets" runat="server" Width="100%"  AutoGenerateColumns="false" AllowSorting="true" CellPadding="4" ForeColor="#333333" GridLines="None"  OnRowDataBound="gvTeamBets_RowDataBound" OnRowCommand="gvTeamBets_RowCommand">
                           <Columns>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                             <asp:LinkButton ID="lbViewBet" runat="server" PostBackUrl='<%# "ViewBet.aspx?BetId=" + Eval("pkBetId") %>'>View Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                                   <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbAcceptBet" runat="server" CommandName="AcceptBet" CommandArgument='<%# Eval("pkBetId") %>'>Accept Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>                
                                  <asp:TemplateField>
                                      <ItemTemplate>
                                       <asp:Label ID="lblMoneyWager" runat="server" Text='<%# Eval("decMoneyWager", "{0:c}") %>' ForeColor="Green" Font-Bold="true" ></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                                   
                               <asp:TemplateField>
                                   <ItemTemplate>
                                              <asp:Image ID="imgTeamImage" runat="server" Width="40" Height="40" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                  <asp:TemplateField>
                                   <ItemTemplate>
                                        <asp:Label runat="server" ID="UserCreatorId" Text='<%# Eval("fkCreatorUserId") %>' Visible="false"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="strTeam"  HeaderText="Team"/>
                               <asp:BoundField DataField="strLogicArgument"  HeaderText="Logic"/>
                               <asp:BoundField DataField="decArgumentValue"  HeaderText="Value"/>
                               <asp:BoundField DataField="fkStatType"  HeaderText="Stat"/>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       During Week
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="intWeekNumber" HeaderText="Week" />

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

                               
                               <asp:Repeater ID="rptBets" runat="server">
                   <ItemTemplate>
                       <ul class="pagination">
                         <li>
                        <asp:LinkButton ID="lnkRepeater" runat="server" 
                            Text='<%#Eval("Text") %>' 
                            CommandArgument='<%# Eval("Value") %>'
                            Enabled='<%# Eval("Enabled") %>' 
                            OnClick="lnkRepeater_Click">
                        </asp:LinkButton>
                         </li>
                  </ul>
                    </ItemTemplate>
                                </asp:Repeater>


                               </asp:Panel>

                               <asp:Panel ID="pnlPlayerBetsGv" runat="server">


                                     <asp:Label ID="lblPlayerBetResultCount" runat="server" Font-Size="Large" ></asp:Label>

                                      <div class="col-sm-3">

                           <asp:DropDownList ID="ddlTopSortByPlayer" runat="server"  CssClass="form-control" AutoPostBack="true"  OnSelectedIndexChanged="ddlTopSortByPlayer_SelectedIndexChanged" >
                                   <asp:ListItem>Sort By</asp:ListItem>             
                                   <asp:ListItem>Wager Amount</asp:ListItem>
                                   <asp:ListItem>Player Name</asp:ListItem>
                                   <asp:ListItem>Logic Argument</asp:ListItem>
                                   <asp:ListItem>Statistical Argument</asp:ListItem>
                                   <asp:ListItem>Argument Value</asp:ListItem>
                           </asp:DropDownList>

                                                           <br />

                                </div>
                                <div class="col-sm-3">

                           <asp:DropDownList ID="ddlTopAscOrDescPlayer" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTopAscOrDescPlayer_SelectedIndexChanged" >
                               <asp:ListItem Value="DESC">Highest To Lowest</asp:ListItem>
                               <asp:ListItem Value="ASC">Lowest To Highest</asp:ListItem>
                           </asp:DropDownList>

                                                           <br />
                                </div>
                               <div class="col-sm-3">

                                     <asp:DropDownList ID="ddlPlayerPageSize" runat="server"  CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeamBetPageSize_SelectedIndexChanged" >                                    
                                      <asp:ListItem>10</asp:ListItem>
                                      <asp:ListItem>25</asp:ListItem>
                                      <asp:ListItem>50</asp:ListItem>
                                      <asp:ListItem>100</asp:ListItem>
                                 </asp:DropDownList>

                                            <br />
                                </div>

                                <div class="col-sm-6">
                                </div>

                       <asp:GridView ID="gvPlayerBets" runat="server" Width="100%"  AutoGenerateColumns="false" AllowSorting="true" CellPadding="4" ForeColor="#333333" GridLines="None"  OnRowDataBound="gvPlayerBets_RowDataBound" OnRowCommand="gvPlayerBets_RowCommand">
                           <Columns>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbViewBet" runat="server" PostBackUrl='<%# "ViewBet.aspx?Type=PlayerBet&BetId=" + Eval("pkBetId") %>'>View Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbAcceptBet" runat="server" CommandName="AcceptBet" CommandArgument='<%# Eval("pkBetId") %>'>Accept Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>                                                        
                                <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Label ID="lblMoneyWager" runat="server" Text='<%# Eval("decMoneyWager", "{0:c}") %>' ForeColor="Green" Font-Bold="true" ></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                                            <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Image ID="imgPlayerImage" runat="server" Width="40" Height="40" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="strPlayerName"  HeaderText="Team"/>
                               <asp:BoundField DataField="strLogicArgument"  HeaderText="Logic"/>
                               <asp:BoundField DataField="decArgumentValue"  HeaderText="Value"/>
                               <asp:BoundField DataField="fkStatType"  HeaderText="Stat"/>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                        <asp:Label runat="server" ID="UserCreatorId" Text='<%# Eval("fkCreatorUserId") %>' Visible="false"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       During Week
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="intWeekNumber" HeaderText="Week" />

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



                                   
                               <asp:Repeater ID="rptPlayerBets" runat="server">
                   <ItemTemplate>
                       <ul class="pagination">
                         <li>
                        <asp:LinkButton ID="lnkPlayerBetsRepeater" runat="server" 
                            Text='<%#Eval("Text") %>' 
                            CommandArgument='<%# Eval("Value") %>'
                            Enabled='<%# Eval("Enabled") %>' 
                            OnClick="lnkPlayerBetsRepeater_Click">
                        </asp:LinkButton>
                         </li>
                  </ul>
                    </ItemTemplate>
                                </asp:Repeater>



                                   </asp:Panel>





                           </div>
                   </div>
                  <div class="col-sm-1">

                        </div>
                   <div class="col-sm-2">
                          <div class="well well-sm" style="color:#1C5E55; border:#1C5E55; background-color:white; ">
                  <div style="text-align:center;color:black;">
                                             <h4 style="font-weight:bold; text-align:center; font-family:'Times New Roman';" >Suggested Bets</h4>  
                                
                      
                         <hr />

                       <asp:ListView ID="lvRecommendedBets" runat="server">
                           <ItemTemplate>
                               

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

                              For <b style="color:green;"><%# Eval("decMoneyWager", "{0:c}") %></b>

                               <br />

                               <a href="ViewBet.aspx?BetId=<%# Eval("pkBetId") %>">View Bet</a>

                          
                               <hr />

                           </ItemTemplate>
                       </asp:ListView>
                             </div>


                              </div>


                        </div>                 
             </div>
     </div>

</asp:Content>
