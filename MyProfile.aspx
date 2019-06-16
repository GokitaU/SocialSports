<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="SocialSports.MyProfile" %>

<%@ Register Src="~/UserControls/ctrlNflSchedule.ascx" TagPrefix="uc1" TagName="ctrlNflSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


        <div class="container-fluid">
              <div class="row">
                   <div class="col-sm-2">
                         <div class="col-sm-9">
                       <div class="well well-sm" style="color:#1C5E55; border-bottom: 3px #e5e5e5 solid;border-right: 3px #e5e5e5 solid; border-left: 3px #e5e5e5 solid; ">
                          
                           <asp:Menu ID="Menu1" OnMenuItemClick="Menu1_MenuItemClick" StaticMenuItemStyle-Font-Size="Small" runat="server" BackColor="#E3EAEB" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#666666" StaticSubMenuIndent="10px">
                             <Items>
                                   <asp:MenuItem Text="My Bets"   ></asp:MenuItem>
                                   <asp:MenuItem Text="My Payment Information" ></asp:MenuItem>
                                   <asp:MenuItem Text="My Bet History \ History" ></asp:MenuItem>
                                   <asp:MenuItem Text="My Avatar" ></asp:MenuItem>

                             </Items>


                               <DynamicHoverStyle BackColor="#1C5E55" ForeColor="White"></DynamicHoverStyle>
                               <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px"></DynamicMenuItemStyle>
                               <DynamicMenuStyle BackColor="#E3EAEB"></DynamicMenuStyle>
                               <DynamicSelectedStyle BackColor="#1C5E55"></DynamicSelectedStyle>
                               <StaticHoverStyle BackColor="#1C5E55" ForeColor="White"></StaticHoverStyle>
                               <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px"></StaticMenuItemStyle>
                               <StaticSelectedStyle BackColor="White"></StaticSelectedStyle>
                           </asp:Menu>   

                  
                           </div>
                             </div>
                         <div class="col-sm-3">

                             </div>
                       </div>   
                  <div class="col-sm-1">

                      </div>
                   <div class="col-sm-6">
                     <div class="well well-lg" style="color:#1C5E55; border:1px solid #1C5E55; ">

                          <asp:Panel ID="pnlUserLoggedIn" runat="server">                                            
                                   <div class="alert alert-success" style="text-align:center;">
                                         <strong>
                                            You Have Logged In Successfully!
                                          </strong> 
                                      </div>
                           </asp:Panel>


                           <asp:Panel ID="pnlMyAvatar" runat="server">
                            <h3 style="text-align:center;">My Avatar</h3>

                           <asp:FileUpload ID="filUploadAvatar" runat="server" />
                           <asp:Button ID="btnUploadAvatar" runat="server" Text="Upload Avatar" OnClick="btnUploadAvatar_Click" />
                           <asp:Image ID="imgAvatar" runat="server" Width="50" Height="50" /> 

                               </asp:Panel>

           
                       <asp:Panel ID="pnlMyBets" runat="server">
                       
                     
                    <h3 style="text-align:center;">My Created Bets</h3>
                                <hr />

                      
                       <asp:GridView ID="gvMyCreatedBets" runat="server" Width="100%"  AutoGenerateColumns="false" AllowSorting="true" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvMyCreatedBets_RowDataBound" OnRowCommand="gvMyCreatedBets_RowCommand"  EmptyDataText="You Currently Have No Bets" >
                           <Columns>                            
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbViewBet" runat="server" >View Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:ImageButton ID="imgCancelBet" runat="server" ImageUrl="~/Icons/RedX.png" Width="30" Height="30" OnClientClick="return confirm('Are you sure you want to cancel this bet');"  CommandName="CancelBet"  CommandArgument='<%# Eval("PlayerOrTeamBet") %>' />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                   </ItemTemplate>
                               </asp:TemplateField>                        
                                 <asp:BoundField DataField="Wager"  ItemStyle-ForeColor="Green"  HeaderText="Wager" />
                                  <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Image ID="imgTeamOrPlayer" runat="server" Width="40" Height="40" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="Name"  HeaderText="Team"/>
                               <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                               <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                               <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       During Week
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="WeekNumber" HeaderText="Week" />
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Label ID="lblBetId" runat="server" Text='<%# Eval("BetId") %>' Visible="false"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
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





                           <h3 style="text-align:center;">My Accepted Bets</h3>


                        <hr />

                          


                       <asp:GridView ID="gvMyAcceptedBets" runat="server" Width="100%"  AutoGenerateColumns="false" AllowSorting="true" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gvMyAcceptedBets_RowDataBound" EmptyDataText="You Currently Have No Bets" OnRowCommand="gvMyAcceptedBets_RowCommand" >
                           <Columns>                               
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbViewBet" runat="server" PostBackUrl='<%# "ViewBet.aspx?BetId=" + Eval("BetId") %>'>View Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                      <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:ImageButton ID="imgCancelBet" runat="server" ImageUrl="~/Icons/RedX.png" Width="30" Height="30" OnClientClick="return confirm('Are you sure you want to cancel this bet');"  CommandName="CancelBet"  CommandArgument='<%# Eval("PlayerOrTeamBet") %>' />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                   </ItemTemplate>
                               </asp:TemplateField>                        
                                 <asp:BoundField DataField="Wager"  ItemStyle-ForeColor="Green"  HeaderText="Wager" />
                                  <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Image ID="imgTeamOrPlayer" runat="server" Width="40" Height="40" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:BoundField DataField="Name"  HeaderText="Team"/>
                               <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                               <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                               <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       During Week
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="WeekNumber" HeaderText="Week" />
                                   <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Label ID="lblBetId" runat="server" Text='<%# Eval("BetId") %>' Visible="false"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
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
                                 
                       <asp:Panel ID="pnlMyBetHistory" runat="server">
                             <h3 style="text-align:center;">My Bet History</h3>


                               <hr />

                           
                       <asp:GridView ID="gvMyBetHistory" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" >
                           <Columns>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton ID="lbViewBet" runat="server" PostBackUrl='<%# "ViewBet.aspx?BetId=" + Eval("BetId") %>'>View Bet</asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField>
                                   <ItemTemplate>
                                   </ItemTemplate>
                               </asp:TemplateField>                        
                                 <asp:BoundField DataField="Wager"  ItemStyle-ForeColor="Green"  HeaderText="Wager" />
                                  <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Image ID="imgTeamOrPlayer" runat="server" Width="40" Height="40" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:BoundField DataField="Name"  HeaderText="Team"/>
                               <asp:BoundField DataField="LogicArgument"  HeaderText="Logic"/>
                               <asp:BoundField DataField="ValueArgument"  HeaderText="Value"/>
                               <asp:BoundField DataField="StatArgument"  HeaderText="Stat"/>
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       During Week
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:BoundField DataField="WeekNumber" HeaderText="Week" />

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


                     <div class="well well-sm" style="color:#1C5E55; border:1px solid #1C5E55;" >

                           <asp:FormView ID="fvMyStats" runat="server">
                               <ItemTemplate>
                                Number Of Games Won   <%# Eval("intNumberOfGamesWon") %>

                                   <br />

                                 Number Of Games Lost  <%# Eval("intNumberOfGamesLost") %>

                                                                      <br />

                                  Overall Winning Percentage  <%# Eval("fltOverallWinningPercentage") %>

                                                                      <br />

                                 Team Winning Percentage  <%# Eval("fltTeamWinPercentage") %>

                                                                      <br />

                                 Player Winning Percentage  <%# Eval("fltPlayerWinPercentage") %>
                           


                               </ItemTemplate>
                           </asp:FormView>

                              </div>

                             </asp:Panel>

                      <asp:Panel ID="pnlMyPredictions" runat="server">

                                 <h3 style="text-align:center;">My Predictions</h3>


                        <hr />
                          
                          
                       <asp:GridView ID="gvMyPredictions" runat="server" Width="100%"  AutoGenerateColumns="false" AllowSorting="true" CellPadding="4" ForeColor="#333333" GridLines="None" >
                           <Columns>                                                 
                                  <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:Image ID="imgTeamImage" runat="server" Width="30" Height="30" />
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


                       </asp:Panel>
                       <asp:Panel ID="pnlMyPaymentInformation" runat="server">

                           <asp:FormView ID="fvPaymentInformation" runat="server" AllowPaging="false" OnModeChanging="fvPaymentInformation_ModeChanging" DataKeyNames="fkUserId" OnItemUpdating="fvPaymentInformation_ItemUpdating" OnItemUpdated="fvPaymentInformation_ItemUpdated">
                               <ItemTemplate>
                                   <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Edit">Edit</asp:LinkButton>    
                                       
                                  Account Number: <%# Eval("strAccountNumber") %> <br />
                                 First Name:  <%# Eval("strFirstName") %> <br />
                                 Last Name:  <%# Eval("strLastName") %> <br />

                               </ItemTemplate>
                               <EditItemTemplate>

                                  Account Number: <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" Text='<%# Bind("strAccountNumber") %>'></asp:TextBox>
                                  First Name: <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" Text='<%# Bind("strFirstName") %>'></asp:TextBox>
                                   Last Name:<asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" Text='<%# Bind("strLastName") %>'></asp:TextBox>

                                   <asp:Button ID="btnUpdate" runat="server" Text="Update"  CommandName="Update"/>
                                    <asp:Button ID="btnCancelUpdate" runat="server" Text="Cancel"  CommandName="Cancel"/>

                               </EditItemTemplate>
                           </asp:FormView>

                       </asp:Panel>

                 
                               </div>
                                 </div>


                    <div class="col-sm-1">

                      </div>
                    <div class="col-sm-2">

                      </div>
                     

                             </div>
                  </div>



</asp:Content>
