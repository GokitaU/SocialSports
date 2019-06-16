<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="CreateBet.aspx.cs" Inherits="SocialSports.Bets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Javascript/Bets.js"></script>


        <div class="container-fluid">
              <div class="row">
                    <div class="col-sm-3">

                        </div>
                   <div class="col-sm-6">
              
                 
                           <div class="col-sm-3">
                               </div>
                           <div class="col-sm-6">

                                <div class="well well-sm" style="color:#1C5E55; border:1px solid #1C5E55; background-color:white;">
                    <asp:Panel ID="pnlBetCreationIsSuccessful" runat="server" BorderColor="DarkGreen">

                        <div class="alert alert-success" style="text-align:center;">
                                 <strong>Bet was created successful!</strong> 
                        </div>

                  </asp:Panel>

                                        <asp:Panel ID="pnlMustBeLoggedId" runat="server" BorderColor="DarkRed">

                                       <div class="alert alert-danger" style="text-align:center;">
                                              <strong>
                                                  You Must Be Logged In. Click <a href="Login.aspx">Here</a> To Login or <a href="Register.aspx">Here</a> To Register.
                                              </strong> 
                                     </div>
                                      </asp:Panel>


                                                        <h3 style="text-align:center;">Create Bet</h3>
                                    <hr />


                  Select Type Of Bet:
                            <asp:RadioButtonList ID="rdoTypeOfBet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoTypeOfBet_SelectedIndexChanged">
                                <asp:ListItem>Team Bet</asp:ListItem>
                                <asp:ListItem>Player Bet</asp:ListItem>
                            </asp:RadioButtonList>


              

                           <h5> Select Nfl Week For Bet:</h5>
                            <asp:DropDownList ID="ddlWeekNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWeekNumber_SelectedIndexChanged" CssClass="form-control">
                                <asp:ListItem>Select Nfl Week</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                            </asp:DropDownList>

                               <br />
                            
                                    <asp:GridView ID="gvGamesForSelectedWeek" runat="server" OnRowCommand="gvGamesForSelectedWeek_RowCommand" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" >
                                        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgMakeTeamBet" runat="server" Width="40" Height="40" ImageUrl="~/Icons/06-512.png" CommandArgument='<%# Eval("NflGameId") %>' CausesValidation="false" CommandName="MakeBet" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="HomeTeam" HeaderText="Home Team" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                VS
                                              </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VisitorTeam"  HeaderText="Visitor Team"/>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                During Week &nbsp&nbsp
                                              </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="WeekNumber"  HeaderText="Week #" />

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
                  
                                
                                       <br />
                         
                                <asp:Panel ID="pnlTeamBet" runat="server">

                                     Select Team For Bet:

                                        <asp:DropDownList ID="ddlTeams" runat="server" CssClass="form-control">
                                        </asp:DropDownList>                                      
                                                   <br />
                                              Select Logic Argument:
                          
                                            <asp:DropDownList ID="ddlTeamLogic" runat="server" CssClass="form-control">
                                                <asp:ListItem>Will Have More Than</asp:ListItem>
                                                <asp:ListItem>Will Have Equal To</asp:ListItem>
                                                <asp:ListItem>Will Have Less Than</asp:ListItem>
                                            </asp:DropDownList>
                                                  <br />
                                             Enter Argument Value:

                                            <asp:TextBox ID="txtTeamArgumentValue" runat="server" CssClass="form-control"></asp:TextBox>
                                               <br /> 
                                      Select Stat Argument:

                                            <asp:DropDownList ID="ddlTeamArguments" runat="server" CssClass="form-control">
                                            </asp:DropDownList>                                                               
                                           <br />
                                     Wager Amount ( in $ ) :

                                      <asp:TextBox ID="txtTeamWagerAmount" runat="server" ForeColor="Green" CssClass="form-control"></asp:TextBox>
                                         <br />

                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Team" ForeColor="Red" DisplayMode="BulletList" />


                                    <asp:RequiredFieldValidator ID="reqTeam" runat="server" ErrorMessage="Team Is Required" ControlToValidate="ddlTeams" ForeColor="Red" SetFocusOnError="true" ValidationGroup="Team" Display="None"  ></asp:RequiredFieldValidator>     
                                    <asp:CompareValidator ID="comTeamArgumentValue" runat="server"  ErrorMessage="Team Argument Value Must Be A Number" ControlToValidate="txtTeamArgumentValue" ForeColor="Red"  Display="None" SetFocusOnError="true" ValidationGroup="Team" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>                                                           
                                    <asp:RequiredFieldValidator ID="reqTeamArgumentValue" runat="server" ErrorMessage="Team Argument Value Is Required" ControlToValidate="txtTeamArgumentValue" ForeColor="Red"  Display="None" SetFocusOnError="true" ValidationGroup="Team"></asp:RequiredFieldValidator>   
                                    <asp:RequiredFieldValidator ID="reqTeamArguments" runat="server" ErrorMessage="Argument Value Is Required" ControlToValidate="ddlTeamArguments" ForeColor="Red" Display="None" SetFocusOnError="true" ValidationGroup="Team"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Wager Must Be A Number" ForeColor="Red" SetFocusOnError="true" Operator="DataTypeCheck" Type="Currency"  Display="None" ControlToValidate="txtTeamWagerAmount" ValidationGroup="Team"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Wager Is Required" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtTeamWagerAmount"  Display="None" ValidationGroup="Team"></asp:RequiredFieldValidator>

                                    <asp:Button ID="btnCreateTeamBet" runat="server" Text="Create Team Bet" OnClick="btnCreateTeamBet_Click"   Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%" ValidationGroup="Team"/>
                            
                          </asp:Panel>

                                <asp:Panel ID="pnlPlayerBet" runat="server">


                               Select Player For Bet:

                                   <asp:DropDownList ID="ddlPlayers" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPlayers_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>

                                            <br />
                   
                                     Select Logic Argument:

                                            <asp:DropDownList ID="ddlPlayersLogic" runat="server" CssClass="form-control">
                                                <asp:ListItem>Will Have More Than</asp:ListItem>
                                                <asp:ListItem>Will Have Equal To</asp:ListItem>
                                                <asp:ListItem>Will Have Less Than</asp:ListItem>
                                            </asp:DropDownList>

                                              <br />

                                     Enter Argument Value:


                                    <asp:TextBox ID="txtPlayerArgumentValue" runat="server" CssClass="form-control"></asp:TextBox>

                                     <br />


                                  Select Statistical Argument:


                                            <asp:DropDownList ID="ddlPlayerArguments" runat="server" CssClass="form-control">
                                            </asp:DropDownList>

                                            <br />

                                 Enter Wager Amount (in $):

                                    <asp:TextBox ID="txtPlayerWagerAmount" runat="server" ForeColor="Green" CssClass="form-control"></asp:TextBox>
                                   
                                      <br />
               

                                    <asp:ValidationSummary ID="valSummaryPlayers" runat="server" ValidationGroup="Player" DisplayMode="BulletList" ForeColor="Red"  />


                                    <asp:RequiredFieldValidator ID="reqPlayerArgument" runat="server" ErrorMessage="Player Stat Argument Is Required" ControlToValidate="ddlPlayerArguments" ForeColor="Red" SetFocusOnError="true" ValidationGroup="Player" Display="None" ></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Wager Must Be A Number" ForeColor="Red" SetFocusOnError="true" Operator="DataTypeCheck" Type="Currency" ControlToValidate="txtPlayerWagerAmount" Display="None" ValidationGroup="Player" ></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="reqVal" runat="server" ErrorMessage="Wager Is Required" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtPlayerWagerAmount" Display="None" ValidationGroup="Player" ></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="reqPlayerArgumentValue" runat="server" ErrorMessage="Player Argument Value Is Required" ControlToValidate="txtPlayerArgumentValue" ForeColor="Red" SetFocusOnError="true" ValidationGroup="Player" Display="None" ></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="comPlayerArgumentValue" runat="server"  ErrorMessage="Player Argument Value Must Be A Number" ControlToValidate="txtPlayerArgumentValue" ForeColor="Red" SetFocusOnError="true" ValidationGroup="Player"  Display="None" Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="reqPlayer" runat="server" ErrorMessage="Player Is Required" ControlToValidate="ddlPlayers" ForeColor="Red" SetFocusOnError="true" ValidationGroup="Player" Display="None" ></asp:RequiredFieldValidator>


                                    <asp:Button ID="btnCreatePlayerBet" runat="server" Text="Create Player Bet" OnClick="btnCreatePlayerBet_Click"   Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%" ValidationGroup="Player"/>

                                    </asp:Panel>


                                 </div>
                               </div>
                                                  


                             <div class="col-sm-3">

                                                </div>
                    
                                        </div> 
                
                       <div class="col-sm-3">

                        </div>
                       
                             </div>
 </div>

</asp:Content>
