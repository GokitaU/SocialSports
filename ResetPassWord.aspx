<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="ResetPassWord.aspx.cs" Inherits="SocialSports.ResetPassWord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
<div class="container">
  <div class="row">
    <div class="col-sm-4">

        </div>
       <div class="col-sm-4">
            <div class="well well-lg" style="color:#669999; border-color:#1C5E55; border-width:1px;">
              
                  <asp:Panel ID="pnlPassWordReset" runat="server">
                           <div class="alert alert-success" style="text-align:center;">
                                 <strong>PassWord was changed successfully! Click <a href="Login.aspx">Here</a> To Log In</strong> 
                        </div>

                 </asp:Panel>     


                  <h3 style="text-align:center;">Reset PassWord</h3>
               
              
                <asp:Panel ID="pnlRecovery" runat="server">

                    Temporary PassWord

                     <asp:TextBox ID="txtTemporaryPassWord" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqTemp" runat="server" ErrorMessage="Temporary PassWord Is Required" ControlToValidate="txtTemporaryPassWord"></asp:RequiredFieldValidator>
                                                            <br />

                    PassWord:
                     <asp:TextBox ID="txtPassWord" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqPassWord" runat="server" ErrorMessage="PassWord Is Required" ControlToValidate="txtPassWord"></asp:RequiredFieldValidator>
                                                            <br />

                  Repeat PassWord:
                    <br />

                   <asp:TextBox ID="txtRepeatPassWord" runat="server" CssClass="form-control"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="reqRepeat" runat="server" ErrorMessage="Repeat PassWord Is Required" ControlToValidate="txtRepeatPassWord"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comPassWords" runat="server" ErrorMessage="PassWords Must Match" ControlToValidate="txtRepeatPassWord" ControlToCompare="txtPassWord"></asp:CompareValidator>                    
                                                            <br />

                <asp:Button ID="btnResetPassWord" runat="server" Text="Reset PassWord"  OnClick="btnResetPassWord_Click"  Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%"/>

                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                
                </asp:Panel>

                <asp:Panel ID="pnlExpired" runat="server">
                    Link Has Expired

                </asp:Panel>
                </div>

           </div>
      <div class="col-sm-4">

        </div>
      </div>
</div>



</asp:Content>
