using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Web.Security;

namespace SocialSports
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                pnlRegisterIsNotSuccessful.Visible = false;
                pnlRegisterIsSuccessful.Visible = false;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            Page.Validate();
            if (this.Page.IsValid)
            {
                using (SqlContextDataContext context = new SqlContextDataContext())
                {

                    string UserName = txtUserName.Text.Trim();
                    string UserEmail = txtUserEmail.Text.Trim();

                    int UserNameCount = context.tblUsers.Where(x => x.strUserName == UserName).Count();
                    int UserEmailCount = context.tblUsers.Where(x => x.strEmail == UserEmail).Count();

                    if (UserNameCount == 0 && UserEmailCount == 0)
                    {
                        int UserId = 0;

                        tblUser NewUser = new tblUser
                        {
                            datDateRegistered = DateTime.Now,
                            strEmail = txtUserEmail.Text.Trim(),
                            strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassWord.Text.Trim(), "SHA1"),
                            strUserName = txtUserName.Text.Trim(),
                            
                        };


                        context.tblUsers.InsertOnSubmit(NewUser);
                        context.SubmitChanges();

                        UserId += NewUser.pkUserId;

                        lblRegisterSuccessful.Text = "Registered Successfully!";
                        lblRegisterSuccessful.ForeColor = System.Drawing.Color.Green;

                        pnlRegisterIsSuccessful.Visible = true;
                        pnlRegisterIsNotSuccessful.Visible = false;

                        UserInfo.InsertDefaultAvatar(UserId);

                    }
                    else if (UserNameCount == 1)
                    {
                        lblStatus.Text = "User Name Already In User";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        pnlRegisterIsNotSuccessful.Visible = true;
                        
                    }
                    else
                    {
                        lblStatus.Text = "User Email Already In User";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        pnlRegisterIsNotSuccessful.Visible = true;
                    }




                }

            }
        }

    }
}