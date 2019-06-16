using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialSports.MasterPages
{
    public partial class LeftNav : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                HighLightNav();
                
                bool IsLoggedIn = Session["UserId"] == null ? false : true;

                ShowHideNavItems(IsLoggedIn);
                
            }

            if (Application["UsersOnline"] != null)
            {
                lblUserCount.Text = Application["UsersOnline"].ToString() + " Users Online";
            }
        }


        private void HighLightNav()
        {
           
            if (Request.RawUrl.Contains("CreateBet"))
            {
                aCreateBet.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aCreateBet.Attributes.CssStyle.Add("color", "White");

            }
            else if (Request.RawUrl.Contains("MyProfile"))
            {
                aMyProfile.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aMyProfile.Attributes.CssStyle.Add("color", "White");
            }
            else if (Request.RawUrl.Contains("SearchBet"))
            {
                aSearchBet.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aSearchBet.Attributes.CssStyle.Add("color", "White");
            }          
            else if (Request.RawUrl.Contains("Login"))
            {
                aLogin.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aLogin.Attributes.CssStyle.Add("color", "White");
            }
            else if (Request.RawUrl.Contains("BetBrilliantly"))
            {
                aBetBrilliantly.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aBetBrilliantly.Attributes.CssStyle.Add("color", "White");
            }                
            else if (Request.RawUrl.Contains("ViewBet"))
            {
                aSearchBet.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aSearchBet.Attributes.CssStyle.Add("color", "White");
            }
            else if (Request.RawUrl.Contains("ResetPassWord"))
            {
                aLogin.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aLogin.Attributes.CssStyle.Add("color", "White");
            }
            else if (Request.RawUrl.Contains("ForgotCredentials"))
            {
                aLogin.Attributes.CssStyle.Add("background-color", "#1C5E55");
                aLogin.Attributes.CssStyle.Add("color", "White");
            }
        }

        private void ShowHideNavItems(bool IsLoggedIn)
        {
            if (IsLoggedIn)
            {
                aMyProfile.Visible = true;
                aRegister.Visible = false;
                aLogin.Visible = false;
                aLogOut.Visible = true;
            }
            else
            {
                aMyProfile.Visible = false;
                aRegister.Visible = true;
                aLogin.Visible = true;
                aLogOut.Visible = false;
            }
        }
    }
}