using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SocialSports
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                pnlLoginIsNotSuccessful.Visible = false;
                pnlLoginIsSuccessful.Visible = false;

                HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["LogInCookie"];

                if (currentUserCookie != null)
                {

                    txtPassWord.Text = currentUserCookie["PassWord"];
                    txtUserName.Text = currentUserCookie["UserName"];

                    chkRememberMe.Checked = true;
                    txtPassWord.Attributes.Add("value", currentUserCookie["PassWord"].ToString());

                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
            this.Page.Validate();
            if (this.Page.IsValid)
            {

                string UserName = txtUserName.Text.Trim();
                string PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassWord.Text.Trim(), "SHA1");

                using (SqlContextDataContext context = new SqlContextDataContext())
                {
               
                    int UserId = context.tblUsers.Where(x => x.strUserName == UserName && x.strPassword == PassWord).Select(x => x.pkUserId).SingleOrDefault();

                    bool IsAuthenticated = UserId != 0 ? true : false;

                    if (IsAuthenticated)
                    {
                        Session["UserId"] = UserId;

                        pnlLoginIsSuccessful.Visible = true;
                        pnlLoginIsNotSuccessful.Visible = false;

                        if (chkRememberMe.Checked)
                        {
                          

                            HttpCookie cookie = new HttpCookie("LogInCookie");

                            cookie["UserName"] = txtUserName.Text;
                            cookie["PassWord"] = txtPassWord.Text;

                            cookie.Expires = DateTime.Now.AddDays(30);
                            HttpContext.Current.Response.SetCookie(cookie);

                        }
                        else
                        {
                            if (HttpContext.Current.Request.Cookies["LogInCookie"] != null)
                            {
                                HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["LogInCookie"];
                                HttpContext.Current.Response.Cookies.Remove("LogInCookie");
                                currentUserCookie.Expires = DateTime.Now.AddDays(-10);

                                HttpContext.Current.Response.SetCookie(currentUserCookie);
                            }
                        }

                        Response.Redirect("MyProfile.aspx", true);

                    }
                    else
                    {
                        pnlLoginIsNotSuccessful.Visible = true;
                        lblStatus.Text = "Invalid UserName/PassWord";
                        lblStatus.ForeColor = System.Drawing.Color.Red;

                        pnlLoginIsSuccessful.Visible = false;
                        pnlLoginIsNotSuccessful.Visible = true;
                    }
                }
            }
        }
    }
}