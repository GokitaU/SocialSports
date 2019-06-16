<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SocialSports.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
       <div class="container">
  <div class="row">
    <div class="col-sm-4">

        </div>
       <div class="col-sm-4">
            <div class="well well-lg" style="color:#1C5E55; border:1px #1C5E55 solid; ">

                          


                    <h3 style="text-align:center;">Login</h3>
                                <hr />


                 <asp:Panel ID="pnlLoginIsNotSuccessful" runat="server" BorderColor="DarkRed">

                                   <div class="alert alert-danger" style="text-align:center;">
                                          <strong>
                                           <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                          </strong> 
                                 </div>
                          </asp:Panel>
                      <asp:Panel ID="pnlLoginIsSuccessful" runat="server">                                            
                                                                        <div class="alert alert-success" style="text-align:center;">
                                                                                 <strong>
                                                                     <asp:Label ID="lblLoginSuccessful" runat="server" ></asp:Label>
                                                                                 </strong> 
                                                                        </div>
                                                                       </asp:Panel>



                               UserName:    <span style="color:red;">*</span>      
                
                <asp:TextBox ID="txtUserName" runat="server" ClientIDMode="AutoID" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqUserName" runat="server" ErrorMessage="UserName Is Required" ControlToValidate="txtUserName" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>                             
                    <br />
                
                  PassWord:    <span style="color:red;">*</span>      
                
                <asp:TextBox ID="txtPassWord" runat="server" ClientIDMode="AutoID" CssClass="form-control" TextMode="Password" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqPassWord" runat="server" ErrorMessage="PassWord Is Required" ControlToValidate="txtPassWord" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>                             


                                     <asp:Button ID="btnLogin" runat="server" Text="Login"  ClientIDMode="AutoID"  OnClick="btnLogin_Click"   Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%" />
                
              

                  Remember Me <asp:CheckBox ID="chkRememberMe" runat="server" />
                          <br />
                                <asp:LinkButton ID="lnkForgot" runat="server" PostBackUrl="~/ForgotCredentials.aspx" CausesValidation="false" >Forgot PassWord or UserName?</asp:LinkButton>



                </div>

           </div>
      <div class="col-sm-4">

        </div>
      </div>
           </div>



</asp:Content>
