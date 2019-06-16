<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="ViewBet.aspx.cs" Inherits="SocialSports.ViewBet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




                             <style type="text/css">
        .StarCss { 
            background-image: url(star.png);
            height:24px;
            width:24px;
        }
        .FilledStarCss {
            background-image: url(filledstar.png);
            height:24px;
            width:24px;
        }
        .EmptyStarCss {
            background-image: url(star.png);
            height:24px;
            width:24px;
        }
        .WaitingStarCss {
            background-image: url(waitingstar.png);
            height:24px;
            width:24px;
        }
  .carousel-inner > .item > img,
  .carousel-inner > .item > a > img {
      width: 70%;
      margin: auto;
  }

    </style>       


      <div class="container-fluid">
              <div class="row">
                   <div class="col-sm-2">
                     

                               <div class="well well-sm" style="border:1px solid #1C5E55; ">
                   
                            <asp:Button ID="btnAcceptToBet" runat="server" Text="Accept Bet"  Width="100%" OnClick="btnAcceptToBet_Click"  Font-Size="Small" CssClass="btn-sm" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" /> 
                            <asp:Button ID="btnCopyBetModal" runat="server" Text="Copy Bet"   Width="100%"  OnClick="btnCopyBetModal_Click" Font-Size="Small" CssClass="btn-sm" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" />     
                            <asp:Button ID="btnAgreeWithBet" runat="server" Text="Aggree With Bet"   Width="100%"  OnClick="btnAgreeWithBet_Click" Font-Size="Small" CssClass="btn-sm" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" />   
                            <asp:Button ID="btnSendBetEmail" runat="server" Text="Send Bet Email"   Width="100%"  OnClick="btnSendBetEmail_Click" Font-Size="Small" CssClass="btn-sm" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" />   

                               

                       </div>
                      </div>

                   <div class="col-sm-2">


                       </div>
                   <div class="col-sm-4">
                             
                         <div class="well well-lg" style="color:#1C5E55; border:1px solid #1C5E55; ">
                                
                             <h3 style="text-align:center;">Bet Information</h3>
                             <asp:Label ID="Label1" runat="server" ></asp:Label>

                        <hr />

                                 
                     <asp:Panel ID="pnlMustBeLoggedId" runat="server" BorderColor="DarkRed">

                                       <div class="alert alert-danger" style="text-align:center;">
                                              <strong>
                                                  You Must Be Logged In. Click <a href="Login.aspx">Here</a> To Login.
                                              </strong> 
                                     </div>
                      </asp:Panel>

                         <asp:Panel ID="pnlBulletinPosted" runat="server">
                                                   <div class="alert alert-success" style="text-align:center;">
                                                         <strong>Bulletin Was Posted!</strong> 
                                                </div>
                         </asp:Panel>
                        <asp:Panel ID="pnlBetAccepted" runat="server">
                                                   <div class="alert alert-success" style="text-align:center;">
                                                         <strong>Bet was accepted successfully!</strong> 
                                                </div>
                         </asp:Panel>
                        <asp:Panel ID="pnlBetCopied" runat="server">
                                                   <div class="alert alert-success" style="text-align:center;">
                                                         <strong>Bet was copied successfully!</strong> 
                                                </div>
                         </asp:Panel>
                              <asp:Panel ID="pnlBetAgreed" runat="server">
                                                   <div class="alert alert-success" style="text-align:center;">
                                                         <strong>Bet was agreed with successfully!</strong> 
                                                </div>
                         </asp:Panel>
                              <asp:Panel ID="pnlEmailSent" runat="server">
                                                   <div class="alert alert-success" style="text-align:center;">
                                                         <strong>Bet email was sent successfully!</strong> 
                                                </div>
                         </asp:Panel>
                       <asp:Label ID="lblBetStatus" runat="server"  Font-Size="XX-Large" ForeColor="Red"></asp:Label>
                             <br />

                       <asp:Label ID="lblPageViews" runat="server"  Font-Size="Large"></asp:Label>




                              <asp:FormView ID="fvPlayerBet" runat="server">
                                    <ItemTemplate>

                                     <div  style="font-size:large; text-align:center;">

                                                <%# Eval("Bets.strPlayerName") %><br />

                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Players.strPlayerImage") %>' Width="100" Height="100" /><br />

                                                <%# Eval("Bets.strLogicArgument") %>
                                                <%# Eval("Bets.decArgumentValue") %>
                                                <%# Eval("Bets.fkStatType") %><br />
                                                During Week 
                                                <%# Eval("Bets.intWeekNumber") %>
                                                For 
                                                 <b style="color:green;font-size:x-large;"><%# Eval("Bets.decMoneyWager", "{0:c}") %></b>
                 
                                                    </div>     
                   
                                    </ItemTemplate>
                              </asp:FormView>

                             
                              <asp:FormView ID="fvTeamBet" runat="server">
                                    <ItemTemplate>

                                     <div  style="font-size:large; text-align:center;">

                                                <%# Eval("Bets.strTeam") %><br />

                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Teams.strTeamImage") %>' Width="100" Height="100" /><br />

                                                <%# Eval("Bets.strLogicArgument") %>
                                                <%# Eval("Bets.decArgumentValue") %>
                                                <%# Eval("Bets.fkStatType") %><br />
                                                During Week 
                                                <%# Eval("Bets.intWeekNumber") %>
                                                For 
                                                 <b style="color:green;font-size:x-large;"><%# Eval("Bets.decMoneyWager","{0:c}") %></b>
                 
                                                    </div>     
                   
                                    </ItemTemplate>
                              </asp:FormView>




                             Bulletin Message
                             <asp:TextBox ID="txtBulletinBoardMessage" runat="server" CssClass="form-control"  Rows="3" TextMode="MultiLine"></asp:TextBox>


                             <asp:Button ID="btnPostToBulletin" runat="server"  Text="Post To Bulletn"  Width="100%"   Font-Size="Small" CssClass="btn-sm" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" OnClick="btnPostToBulletin_Click" />



                             <asp:Literal ID="litBulletinBoard" runat="server"></asp:Literal>
                           



                             </div>
                       </div>
                   <div class="col-sm-2">


                       </div>
                   <div class="col-sm-2">


                       </div>
                  </div>

          </div>
                
   



    
    
                                    <div id="ModalSendBetEmail" class="modal fade" role="dialog">
                                      <div class="modal-dialog">

                                        <div class="modal-content">
                                              <div class="modal-header">
                                   
                                                                 
        <button type="button" class="close" data-dismiss="modal" runat="server" >&times;</button>
        <h4 class="modal-title">Send Bet Email</h4>
      </div>

        <div class="modal-body">
            

            
            <asp:FormView ID="fvBetEmail" runat="server">
                <ItemTemplate>
                   
                      <div  style="font-size:large; text-align:center;">

                    <%# Eval("Bets.strTeam") %><br />

                    <asp:Image ID="imgTeam" runat="server" ImageUrl='<%# Eval("Teams.strTeamImage") %>' Width="100" Height="100" /><br />

                    <%# Eval("Bets.strLogicArgument") %>
                    <%# Eval("Bets.decArgumentValue") %>
                    <%# Eval("Bets.fkStatType") %><br />
                    During Week 
                    <%# Eval("Bets.intWeekNumber") %>
                    For 
                     <b style="color:green;font-size:x-large;">$ <%# Eval("Bets.decMoneyWager") %></b>
                 
                        </div>                
                </ItemTemplate>
            </asp:FormView>

            <asp:FormView ID="fvPlayerBetEmail" runat="server">
                                    <ItemTemplate>

                                     <div  style="font-size:large; text-align:center;">

                                                <%# Eval("Bets.strPlayerName") %><br />

                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Players.strPlayerImage") %>' Width="100" Height="100" /><br />

                                                <%# Eval("Bets.strLogicArgument") %>
                                                <%# Eval("Bets.decArgumentValue") %>
                                                <%# Eval("Bets.fkStatType") %><br />
                                                During Week 
                                                <%# Eval("Bets.intWeekNumber") %>
                                                For 
                                                 <b style="color:green;font-size:x-large;">$ <%# Eval("Bets.decMoneyWager") %></b>
                 
                                                    </div>     
                   
                                    </ItemTemplate>
                              </asp:FormView>

        
                                        
                               User Email:
                  
                <asp:TextBox ID="txtUserEmail" runat="server" CssClass="form-control"></asp:TextBox>

                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" 
                                            ControlToValidate="txtUserEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ErrorMessage="Invalid Email" ForeColor="Red" ValidationGroup="Email"></asp:RegularExpressionValidator>
                              
                      <br />
                           Email Message:
                                   <asp:TextBox ID="txtEmailBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ></asp:TextBox>
                                                                             
                                  <br />


            <br />
            <br />

      </div>
      <div class="modal-footer">

        <asp:Button ID="btnSendEmail" runat="server" Text="Send Bet Email"  OnClick="btnSendEmail_Click" class="btn btn-default" ValidationGroup="Email"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

      </div>
    </div>
       </div>
</div>   







    
                                    <div id="ModalCopyBet" class="modal fade" role="dialog">
                                      <div class="modal-dialog">

                                        <div class="modal-content">
                                              <div class="modal-header">
                                   
                                                                 
        <button id="Button1" type="button" class="close" data-dismiss="modal" runat="server" >&times;</button>
        <h4 class="modal-title">Copy Bet</h4>
      </div>

        <div class="modal-body">
            
            Would You Like To Copy This Bet Or The Opposite Of It?

            <asp:RadioButtonList ID="rdoCopyBetSelection" runat="server" Font-Size="X-Small" Font-Bold="false">
            </asp:RadioButtonList>
            
       

     
                            
                
            <br />

      </div>
      <div class="modal-footer">

        <asp:Button ID="Button2" runat="server" Text="Copy Bet"  OnClick="btnCopyBet_Click" class="btn btn-default" ValidationGroup="Email"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

      </div>
    </div>
       </div>
</div>   







    
                                    <div id="modalReply" class="modal fade" role="dialog">
                                      <div class="modal-dialog">

                                        <div class="modal-content">
                                              <div class="modal-header">
                                   
                                                                 
        <button id="Button3" type="button" class="close" data-dismiss="modal" runat="server" >&times;</button>
        <h4 class="modal-title">Reply To Bulletin</h4>
      </div>

        <div class="modal-body">
            
            <asp:Literal ID="litReply" runat="server"></asp:Literal>

    
            <asp:TextBox ID="txtReplyBulletin" runat="server" MaxLength="500" ValidationGroup="Reply" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReplyBulletin" ErrorMessage="Reply Message Is Required"  ValidationGroup="Reply"></asp:RequiredFieldValidator>
     
                            
                
            <br />

      </div>
      <div class="modal-footer">

        <asp:Button ID="btnReply" runat="server" Text="Reply"  OnClick="btnReply_Click" class="btn btn-default"  ValidationGroup="Reply"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

      </div>
    </div>
       </div>
</div>   










</asp:Content>
