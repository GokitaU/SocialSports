using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Text;

namespace SocialSports
{
    public partial class ForgotCredentials : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                rdoForgot.SelectedIndex = 1;
                pnlEmailSent.Visible = false;
            }
        }

        protected void btnRecoverPassWord_Click(object sender, EventArgs e)
        {

            this.Page.Validate();

            if (this.Page.IsValid)
            {
                using (SqlContextDataContext context = new SqlContextDataContext())
                {

                    var results = from t in context.tblUsers
                                  where t.strEmail == txtEmail.Text
                                  select t;

                    if (results.Count() > 0)
                    {
                        foreach (var result in results)
                        {


                            Guid guid = Guid.NewGuid();
                            string TempPassWord = RandomPassWordGenerator(16);

                            tblCredentialsRecovery rec = new tblCredentialsRecovery
                            {
                                guiRecoveryGuid = guid,
                                fkUserId = result.pkUserId,
                                datResetRequestDateTime = DateTime.Now,
                                strTemporaryPassWord = TempPassWord,
                                blnIsPassWordRecovery = Convert.ToBoolean(rdoForgot.SelectedIndex)

                            };

                            context.tblCredentialsRecoveries.InsertOnSubmit(rec);
                            context.SubmitChanges();

                            SendPassWordResetLink(result.strEmail, result.strUserName, guid.ToString(), TempPassWord);

                            pnlEmailSent.Visible = true;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Email Was Not Found";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            
            }
        }

        private string RandomPassWordGenerator(int NumberOfBytes)
        {
            byte[] RandomPassWord = new byte[NumberOfBytes];
            RandomNumberGenerator gen = RandomNumberGenerator.Create();
            gen.GetBytes(RandomPassWord);

            return Convert.ToBase64String(RandomPassWord);

        }

    
        public static void SendPassWordResetLink(string ToEmail, string UserName, string UniqueId, string TemporaryPassWord)
        {
            MailMessage mail = new MailMessage();

            StringBuilder message = new StringBuilder();

            message.Append("Dear " + UserName + ",<br /><br />");
            message.Append("You have requested to reset your PassWord.");
            message.Append("<br />");
            message.Append("Please click on the following link to reset your password:");
            message.Append("<br />");
            message.Append("http://localhost:32550/ResetPassWord.aspx?guid=" + UniqueId);
            message.Append("<br />");
            message.Append("<br />");
            message.Append("<b>Temporary PassWord</b> : " + TemporaryPassWord);


            mail.IsBodyHtml = true;
            mail.Body = message.ToString();
            mail.Subject = "Social Sports PassWord Reset Link";

            mail.From = new MailAddress("NoReply@SocialSports", "SocialSports");
            mail.To.Add(new MailAddress(ToEmail));



            SmtpClient c = new SmtpClient();
            c.Port = 587;
            c.Host = "smtp.gmail.com";

            c.Credentials = new System.Net.NetworkCredential("testerjohnwtf@gmail.com", "1111111111");
            c.DeliveryMethod = SmtpDeliveryMethod.Network;
            c.EnableSsl = true;

            c.Send(mail);


        }



    }
}