<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="ForgotCredentials.aspx.cs" Inherits="SocialSports.ForgotCredentials" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
<div class="container">
  <div class="row">
    <div class="col-sm-4">

        </div>
       <div class="col-sm-4">
            <div class="well well-lg" style="color:#1C5E55; border:1px solid #1C5E55; ">
              
                  <asp:Panel ID="pnlEmailSent" runat="server">
                         <div class="alert alert-success" style="text-align:center;">
                                <strong>
                                    Email was Sent!
                                </strong> 
                         </div>
                 </asp:Panel>     


                    <h3 style="text-align:center;">Forgot Credentials</h3>
               
                <hr />

                 <asp:RadioButtonList ID="rdoForgot" runat="server" >
                     <asp:ListItem>User Name Recovery</asp:ListItem>
                     <asp:ListItem>PassWord Recovery</asp:ListItem>
                </asp:RadioButtonList>   
                              
               
                    Email:
                     <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="Email Is Required" ControlToValidate="txtEmail" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>                             
                <br />
                <asp:Button ID="btnRecoverPassWord" runat="server" Text="Recover Credentials"  OnClick="btnRecoverPassWord_Click"   Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%"/>

                <asp:Label ID="lblStatus" runat="server" ForeColor="Red" ></asp:Label>

                </div>

           </div>
      <div class="col-sm-4">

        </div>
      </div>
</div>




</asp:Content>
