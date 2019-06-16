<%@ Page Title="" Language="C#" MasterPageFile="~/LeftNav.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SocialSports.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <div class="container">
  <div class="row">
    <div class="col-sm-4">

        </div>
       <div class="col-sm-4">
            <div class="well well-lg" style="color:#1C5E55; border:1px solid #1C5E55; ">
                     
                    <h3 style="text-align:center;">Register</h3>
                                <hr />

                
                 <asp:Panel ID="pnlRegisterIsNotSuccessful" runat="server" BorderColor="DarkRed">

                                   <div class="alert alert-danger" style="text-align:center;">
                                          <strong>
                                         <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                          </strong> 
                                 </div>
                  </asp:Panel>
                   <asp:Panel ID="pnlRegisterIsSuccessful" runat="server">                                            
                                                                        <div class="alert alert-success" style="text-align:center;">
                                                                                 <strong>
                                                                     <asp:Label ID="lblRegisterSuccessful" runat="server" ></asp:Label>
                                                                                 </strong> 
                                                                        </div>
                                                                       </asp:Panel>






                               User Name:    <span style="color:red;">*</span>      
                
                <asp:TextBox ID="txtUserName" runat="server" ClientIDMode="AutoID" CssClass="form-control" MaxLength="20" ></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqtxtUserName" runat="server" ErrorMessage="UserName is Required"  ControlToValidate="txtUserName" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                               <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtUserName" ID="RegularExpressionValidator2" ValidationExpression = "^[\s\S]{6,}$" runat="server" ErrorMessage="Minimum 6 characters required."></asp:RegularExpressionValidator>

                                           <br />

                               User Email:   <span style="color:red;">*</span>      
                
                 <asp:TextBox ID="txtUserEmail" runat="server" ClientIDMode="AutoID" CssClass="form-control" ></asp:TextBox>

                                              <asp:RequiredFieldValidator ID="reqtxtUserEmail" runat="server" ErrorMessage="Email is Required"  ControlToValidate="txtUserEmail" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                   
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" 
                                            ControlToValidate="txtUserEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ErrorMessage="Invalid Email" ForeColor="Red"></asp:RegularExpressionValidator>


                           <br />

                               PassWord:    <span style="color:red;">*</span>      
                
                <asp:TextBox ID="txtPassWord" runat="server" ClientIDMode="AutoID" CssClass="form-control" MaxLength="20" TextMode="Password" ></asp:TextBox>
                                                  <asp:RequiredFieldValidator ID="reqtxtUserPassWord" runat="server" ErrorMessage="PassWord is Required"  ControlToValidate="txtPassWord" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <ajaxToolkit:PasswordStrength ID="PasswordStrength1" runat="server" MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumUpperCaseCharacters="1" TargetControlID="txtPassWord" RequiresUpperAndLowerCaseCharacters="true" PreferredPasswordLength="8" /> 
                <br />

                             Repeat PassWord:    <span style="color:red;">*</span>      
                
                <asp:TextBox ID="txtRepeatPassWord" runat="server" ClientIDMode="AutoID" CssClass="form-control" MaxLength="20" TextMode="Password" ></asp:TextBox>


                <asp:RequiredFieldValidator ID="reqtxtUserRepeatPassWord" runat="server" ErrorMessage="Repeat PassWord Is Required" ControlToValidate="txtRepeatPassWord" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="comreqtxtUserRepeatPassWord" runat="server" ErrorMessage="PassWords Must Match" ControlToValidate="txtRepeatPassWord" ControlToCompare="txtPassWord" Operator="Equal"  ForeColor="Red" SetFocusOnError="true"></asp:CompareValidator>                     
                
                 <asp:Button ID="btnRegister" runat="server" Text="Register"  ClientIDMode="AutoID"  OnClick="btnRegister_Click"   Font-Size="Small" CssClass="btn-lg" Font-Bold="true" ForeColor="White" BackColor="#1C5E55" Width="100%" />



                </div>

           </div>
      <div class="col-sm-4">

        </div>
      </div>
           </div>


</asp:Content>
