using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SocialSports
{
    public partial class ResetPassWord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                Guid guid;
                pnlRecovery.Visible = false;
                pnlExpired.Visible = false;
                pnlPassWordReset.Visible = false;

                if (Request.QueryString["guid"] != null && Guid.TryParse(Request.QueryString["guid"], out guid))
                {
                    using (SqlContextDataContext context = new SqlContextDataContext())
                    {
                        var q = from t in context.tblCredentialsRecoveries where t.guiRecoveryGuid.ToString() == guid.ToString()
                                select t;
                     
                        foreach (var r in q)
                        {                        
                            if (r != null)
                            {
                                if (r.datResetRequestDateTime > DateTime.Now.AddHours(-2))
                                {
                                    pnlRecovery.Visible = true;
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                pnlRecovery.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        protected void btnResetPassWord_Click(object sender, EventArgs e)
        {
            Guid g;

            if (Guid.TryParse(Request.QueryString["guid"], out g))
            {
                using (SqlContextDataContext context = new SqlContextDataContext())
                {
                    var q = from t in context.tblCredentialsRecoveries
                            where t.guiRecoveryGuid.ToString() == g.ToString()
                            select t;

                    foreach (var r in q)
                    {
                        Page.Validate();

                        if (this.Page.IsValid)
                        {
                            if (txtTemporaryPassWord.Text.Trim() == r.strTemporaryPassWord)
                            {


                                tblUser user = context.tblUsers.SingleOrDefault(x => x.pkUserId == r.fkUserId);
                                user.strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassWord.Text.Trim(), "SHA1");
                                context.SubmitChanges();

                                pnlPassWordReset.Visible = true;

                            }
                            else
                            {
                                lblStatus.Text = "Invalid Temporary PassWord";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
            }
        }

        
    }
}