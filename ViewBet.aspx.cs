using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;
using System.Data.Linq;

namespace SocialSports
{
    public partial class ViewBet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.Page.IsPostBack)
            {

                HidePanels();

                if (Request.QueryString["BetId"] != null)
                {
                    int betId = Convert.ToInt32(Request.QueryString["BetId"]);

                    bool IsTeamBet = Request.QueryString["Type"] == null ? true : false;


                    if (Session["UserId"] != null)
                    {
                        if (!IsAgreed((int)Session["UserId"], betId, IsTeamBet))
                        {
                            btnAgreeWithBet.Enabled = false;
                            btnAgreeWithBet.Attributes.CssStyle.Add("opacity", ".5");
                            btnAgreeWithBet.ToolTip = "Bet Is Already Agreed With";
                        }
                    }
                    else
                    {
                        btnAcceptToBet.Enabled = false;
                        btnAcceptToBet.Attributes.CssStyle.Add("opacity", ".5");
                        btnAcceptToBet.ToolTip = "Must Be Logged In";

                        btnCopyBetModal.Enabled = false;
                        btnCopyBetModal.Attributes.CssStyle.Add("opacity", ".5");
                        btnCopyBetModal.ToolTip = "Must Be Logged In";

                        btnAgreeWithBet.Enabled = false;
                        btnAgreeWithBet.Attributes.CssStyle.Add("opacity", ".5");
                        btnAgreeWithBet.ToolTip = "Must Be Logged In";

                        btnSendBetEmail.Enabled = false;
                        btnSendBetEmail.Attributes.CssStyle.Add("opacity", ".5");
                        btnSendBetEmail.ToolTip = "Must Be Logged In";


                    }

                    if (!UsersBets.IsBetOpen(betId, IsTeamBet))
                    {
                        lblBetStatus.Text = "Bet Is Closed";
                        btnAcceptToBet.Enabled = false;
                        btnAcceptToBet.Attributes.CssStyle.Add("opacity", ".5");
                    }


                    if (Session["UserId"] != null)
                    {
                        string BaseUrl = Request.RawUrl;
                        int BetId = int.Parse(Request.QueryString["BetId"]);

                        UserTracking.MemberTracking m = new UserTracking.MemberTracking();
                        m.InsertPageView((int)Session["UserId"], BaseUrl, BetId, IsTeamBet);
                    }

                    UserTracking Tracking = new UserTracking();

                    lblPageViews.Text = "# of Page Views: " + Tracking.PageViews(this.Page.Request.RawUrl).ToString();



                    if (Request.QueryString["Type"] == null)
                    {

                        GetTeamBet(betId);

                     

                        SqlContextDataContext context = new SqlContextDataContext();

                        IEnumerable<tblTeamBet> TeamBets = from t in context.tblTeamBets
                                                           where t.pkBetId == betId
                                                           select t;

                        foreach (var TeamBet in TeamBets)
                        {
                            if (HttpContext.Current.Request.Cookies["TeamBets"] == null)
                            {
                                HttpCookie cook = new HttpCookie("TeamBets");
                                cook.Expires = DateTime.Now.AddDays(30);
                                NameValueCollection coll = new NameValueCollection();
                                coll.Set("Team", TeamBet.strTeam);
                                cook.Values.Add(coll);
                                HttpContext.Current.Response.SetCookie(cook);
                            }
                            else
                            {
                                HttpCookie cookie = HttpContext.Current.Request.Cookies["TeamBets"];

                                NameValueCollection nvc = cookie.Values;
                                nvc.Add("Team", TeamBet.strTeam);

                                foreach (string value in nvc.GetValues("Team"))
                                {
                                    // Label1.Text += value;
                                }

                                cookie.Values.Add(nvc);
                                HttpContext.Current.Response.SetCookie(cookie);
                            }
                        }
                  
                    }
                    else
                    {
                        GetPlayerBet(betId);

                    }


                   
                       
                    
                }

            }

            if (Request.QueryString["BulletinId"] != null)
            {
                GetReplyData();
                GetModal("modalReply");
            }

            if (Request.QueryString["BetId"] != null)
            {
                int BetId = Convert.ToInt32(Request.QueryString["BetId"]);

                bool IsTeamBet = Request.QueryString["Type"] == null ? true : false;
             

                litBulletinBoard.Text = GetBulletinBoard(BetId, IsTeamBet).ToString();

                if (ViewState["BulletinWasPosted"] != null)
                {
                    pnlBulletinPosted.Visible = false;
                    ViewState["BulletinWasPosted"] = null;
                }
                else if (ViewState["MustBeLoggedIn"] != null)
                {
                    pnlBulletinPosted.Visible = false;
                    ViewState["MustBeLoggedIn"] = null;
                }

            }
        }


        private void GetPlayerBet(int BetId)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                var PlayerBets = con.tblPlayerBets.Join
                                (con.tblNflPlayers, p => p.strPlayerName, t => t.strPlayerName, (bets, players) => new
                                {
                                    Bets = bets,
                                    Players = players

                                }).Where(x => x.Bets.pkBetId == BetId);

                fvPlayerBet.DataSource = PlayerBets;
                fvPlayerBet.DataBind();

                foreach (var PlayerBet in PlayerBets)
                {
                    Session["UserIdWhoCreated"] = PlayerBet.Bets.fkCreatorUserId;
                }

            }
        }

        private bool IsBetOpen(bool IsTeamBet, int BetId)
        {
            SqlContextDataContext context = new SqlContextDataContext();
            bool IsOpen = true;

            if (IsTeamBet)
            {
                var IsTeamBetOpen = context.tblTeamBets.Where(x => x.pkBetId == BetId)
                              .Select(n => new { n.blnBetIsOpened });

                foreach (var r in IsTeamBetOpen)
                {
                    IsOpen = r.blnBetIsOpened;
                }
            }
            else
            {
                var IsPlayerBetOpen = context.tblPlayerBets.Where(x => x.pkBetId == BetId)
                              .Select(n => new { n.blnBetIsOpened });

                foreach (var r in IsPlayerBetOpen)
                {
                    IsOpen = r.blnBetIsOpened;
                }
            }

            return IsOpen;
        }

        private bool IsAgreed(int UserId, int BetId, bool IsTeamBet )
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                int r = con.tblBetApprovals.Where(
                                                    x => x.blnIsTeamBet == IsTeamBet 
                                                    && x.fkBetId == BetId 
                                                    && x.fkUserIdWhoApproved == UserId).Count();

                return Convert.ToBoolean(r);
            }

        }

        private void HidePanels()
        {
            pnlBetAccepted.Visible = false;
            pnlBetAgreed.Visible = false;
            pnlBetCopied.Visible = false;
            pnlEmailSent.Visible = false;
            pnlBulletinPosted.Visible = false;
            pnlMustBeLoggedId.Visible = false;
        }

        protected void btnAcceptToBet_Click(object sender, EventArgs e)
        {
            int QstringBetId;

            if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out QstringBetId))
            {
                if (Request.QueryString["Type"] != null)
                {
                    if (IsBetOpen(false, QstringBetId) == true)
                    {
                        AcceptBet(QstringBetId, "PlayerBet", (sender as Button));
                 
                        btnAcceptToBet.Enabled = false;
                        btnAcceptToBet.Attributes.CssStyle.Add("opacity", ".5");
                        btnAcceptToBet.ToolTip = "Bet Is Already Filled";
                    }
                }
                else
                {

                    if (IsBetOpen(true, QstringBetId) == true)
                    {
                        AcceptBet(QstringBetId, "TeamBet", (sender as Button));
                  
                        btnAcceptToBet.Enabled = false;
                        btnAcceptToBet.Attributes.CssStyle.Add("opacity", ".5");
                        btnAcceptToBet.ToolTip = "Bet Is Already Filled";
                    }
                }
            }
        }
        


        private void AcceptBet(int BetId, string TypeOfBet, Button But)
        {

            int Qstring;

            if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out Qstring))
            {
                Response.Write(Session["UserIdWhoCreated"].ToString());

                if (Session["UserId"] != null)
                {
                    if (Session["UserIdWhoCreated"] != null)
                    {
                        if (TypeOfBet == "PlayerBet")
                        {
                            UsersBets.UserAgreeToBet(Qstring, (int)Session["UserIdWhoCreated"], (int)Session["UserId"], false);
                        }
                        else
                        {
                            UsersBets.UserAgreeToBet(Qstring, (int)Session["UserIdWhoCreated"], (int)Session["UserId"], true);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                    ViewState["MustBeLoggedIn"] = true;
                }
            }
        }

        protected void btnCopyBet_Click(object sender, EventArgs e)
        {
            int QstringBetId;

            if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out QstringBetId))
            {
                if (Session["UserId"] != null)
                {
                    if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "PlayerBet")
                    {
                        if (rdoCopyBetSelection.SelectedIndex == 0)
                        {
                            UsersBets.MakeCopyOBet((int)Session["UserId"], QstringBetId, false, false);
                        }
                        else
                        {
                            UsersBets.MakeCopyOBet((int)Session["UserId"], QstringBetId, false, true);
                        }
                    }
                    else
                    {
                        if (rdoCopyBetSelection.SelectedIndex == 0)
                        {
                            UsersBets.MakeCopyOBet((int)Session["UserId"], QstringBetId, true, false);
                        }
                        else
                        {
                            UsersBets.MakeCopyOBet((int)Session["UserId"], QstringBetId, true, true);
                        }
                    }
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                    ViewState["MustBeLoggedIn"] = true;
                }
            }
        }

        protected void btnAgreeWithBet_Click(object sender, EventArgs e)
        {
            int Qstring;

            if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out Qstring))
            {
                if (Session["UserId"] != null)
                {
                    UsersBets.UserBetApprovals(Qstring, (int)Session["UserId"]);

                    btnAgreeWithBet.Enabled = false;
                    btnAgreeWithBet.Attributes.CssStyle.Add("opacity", ".5");
                    btnAgreeWithBet.ToolTip = "Bet Is Already Agreed With";
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                    ViewState["MustBeLoggedIn"] = true;
                }
            }
        }

        private void GetTeamBet(int BetId)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {


                var TeamBets = con.tblTeamBets.Join
                                (con.tblNflTeams, b => b.strTeam, t => t.strTeamName, (bets, teams) => new
                                {
                                    Bets = bets,
                                    Teams = teams
                                }).Where(x => x.Bets.pkBetId == BetId);

                fvTeamBet.DataSource = TeamBets;
                fvTeamBet.DataBind();

                foreach (var r in TeamBets)
                {
                    Session["UserIdWhoCreated"] = r.Bets.fkCreatorUserId;
                }

                if (Session["UserId"] != null)
                {
                    if ((int)Session["UserId"] == (int)Session["UserIdWhoCreated"])
                    {
                        btnAcceptToBet.Enabled = false;
                        btnAcceptToBet.Attributes.CssStyle.Add("opacity", ".5");
                        btnAcceptToBet.ToolTip = "Bet IWas Made By You";
                    }
                }
          
            }
        }

        private void DisableButton(Button Btn, string ToolTipText)
        {
            Btn.Enabled = false;
            Btn.Attributes.CssStyle.Add("opacity", ".5");
            Btn.ToolTip = ToolTipText;
        }

        private void GetModal(string ModalId)
        {
            string jquery = "$('#" + ModalId + "').modal('show');";
            ClientScript.RegisterStartupScript(typeof(Page), "a key",
           "<script type=\"text/javascript\">" + jquery + "</script>");
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
        }

        protected void btnSendBetEmail_Click(object sender, EventArgs e)
        {

            int BetId = Convert.ToInt32(Request.QueryString["BetId"]);

            if (Request.QueryString["BetId"] != null)
            {
                if (Session["UserId"] != null)
                {
                    GetModal("ModalSendBetEmail");


                    if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "PlayerBet")
                    {
                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {

                            var PlayerBets = con.tblPlayerBets.Join
                                            (con.tblNflPlayers, p => p.strPlayerName, t => t.strPlayerName, (bets, players) => new
                                            {
                                                Bets = bets,
                                                Players = players

                                            }).Where(x => x.Bets.pkBetId == BetId);

                            fvPlayerBetEmail.DataSource = PlayerBets;
                            fvPlayerBetEmail.DataBind();

                        }

                    }
                    else
                    {
                        SqlContextDataContext con = new SqlContextDataContext();

                        var TeamBets = con.tblTeamBets.Join
                                          (con.tblNflTeams, b => b.strTeam, t => t.strTeamName, (bets, teams) => new
                                          {
                                              Bets = bets,
                                              Teams = teams
                                          });

                        fvBetEmail.DataSource = TeamBets;
                        fvBetEmail.DataBind();

                    }
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                    ViewState["MustBeLoggedIn"] = true;
                }
            }
        }

        private void SendTeamBetEmail(string EmailAddressToSendTo, string EmailTopic, string EmailMessage, tblTeamBet Bet)
        {
            MailMessage mail = new MailMessage();

            StringBuilder message = new StringBuilder();

            message.Append(Bet.strLogicArgument);
            message.Append("<br />");
            message.Append("<br />");



            mail.IsBodyHtml = true;
            mail.Body = message.ToString();
            mail.Subject = "Social Sports PassWord Reset Link";

            mail.From = new MailAddress("NoReply@SocialSports", "SocialSports");
            mail.To.Add(new MailAddress(EmailAddressToSendTo));



            SmtpClient c = new SmtpClient();
            c.Port = 587;
            c.Host = "smtp.gmail.com";

            c.Credentials = new System.Net.NetworkCredential("testerjohn@gmail.com", "11111111");
            c.DeliveryMethod = SmtpDeliveryMethod.Network;
            c.EnableSsl = true;

            c.Send(mail);

        }

        private StringBuilder GetBulletinBoard(int BetId, bool IsTeamBet)
        {

            SqlContextDataContext con = new SqlContextDataContext();

            StringBuilder s = new StringBuilder();


            int i = 0;

            foreach (tblParentBulletinBoard pb in con.tblParentBulletinBoards.Where(x => x.intBetId == BetId))
            {
                var UserNameAndImage = from t in con.tblUsers join u in con.tblUsersBettingStatistics on t.pkUserId equals u.fkUserId 
                                       where t.pkUserId == pb.intUserIdWhoPostedBoard
                                       select new
                                       {
                                           UserName = t.strUserName,
                                           UserImage = t.bytUserImage,
                                           WinPerc = u.fltOverallWinningPercentage,

                                       };

                s.Append("<div class=\"well well-sm\" style=\"border:1px solid #1C5E55; \">");

                foreach (var v in UserNameAndImage)
                {


                    s.Append("Posted By" + v.UserName);
                    s.Append("<br />");
                    s.Append("<img src=\"data:Image/png;base64," + Convert.ToBase64String((byte[])v.UserImage.ToArray()) + "\"  class=\"img-circle\" width=\"50\"  height=\"50\" />");

                    if (v.WinPerc > .75)
                    {
                        s.Append("Win Perc=<span style=\"color:green;\">" + v.WinPerc.ToString() + "</span>");
                    }
                    else if (v.WinPerc < .25)
                    {
                        s.Append("Win Perc=<span style=\"color:green;\">" + v.WinPerc.ToString() + "</span>");
                    }

                

                }

                s.Append("@ " + pb.datDateTimeCreated.Value.ToShortDateString());
                s.Append("<br />");

                s.Append(pb.strBoardMessage);
                s.Append("<a href=\"ViewBet.aspx?BetId=" + pb.intBetId.ToString() + "&BulletinId=" + pb.intParentBoardId.ToString() + "\">Reply</a>");

                s.Append("</div>");


                i++;


                foreach (tblChildBulletinBoard cb in pb.tblChildBulletinBoards)
                {
                    var ChildUserNameAndImage = from t in con.tblUsers
                                                where t.pkUserId == cb.intBoardCreatorUserId
                                                select new
                                                {
                                                    UserName = t.strUserName,
                                                    UserImage = t.bytUserImage
                                                };

                    s.Append("<div style=\"border:1px solid #1C5E55; width:90%; margin-left:10%;\" >");


                    foreach (var c in UserNameAndImage)
                    {

                        s.Append("Posted By" + c.UserName);
                        s.Append("<img src=\"data:Image/png;base64," + Convert.ToBase64String((byte[])c.UserImage.ToArray()) + "\"  class=\"img-circle\" width=\"40\"  height=\"40\" />");


                    }

                    
                    s.Append(cb.strBoardMessage);

                    s.Append("</div>");


                }


            }



            return s;

        }

        protected void btnPostToBulletin_Click(object sender, EventArgs e)
        {
            bool IsTeamBet = false;

            if (Request.QueryString["Type"] != null)
            {
                IsTeamBet = true;
            }

            int BetId;

            if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out BetId))
            {
                if (Session["UserId"] != null)
                {
                    tblParentBulletinBoard Board = new tblParentBulletinBoard
                    {
                        blnIsTeamBet = IsTeamBet,
                        intBetId = Convert.ToInt32(Request.QueryString["BetId"]),
                        datDateTimeCreated = DateTime.Now,
                        intNumOfLikes = 0,
                        strBoardMessage = txtBulletinBoardMessage.Text.Trim(),
                        intUserIdWhoPostedBoard = (int)Session["UserId"],
                        intBoardAttachmentId = 0

                    };

                    using (SqlContextDataContext c = new SqlContextDataContext())
                    {
                        c.tblParentBulletinBoards.InsertOnSubmit(Board);
                        c.SubmitChanges();
                    }

                    litBulletinBoard.Text = GetBulletinBoard(BetId, IsTeamBet).ToString();

                    ViewState["BulletinWasPosted"] = true;
                    pnlBulletinPosted.Visible = true;
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                    ViewState["MustBeLoggedIn"] = true;
                }
            }
        }

        private void CopyModalContent()
        {

            rdoCopyBetSelection.Items.Clear();

            if (Session["UserId"] != null)
            {
                if (Request.QueryString["Type"] == null)
                {
                    int TeamBetId;

                    if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out TeamBetId))
                    {
                        GetModal("ModalCopyBet");

                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                            var b = from t in con.tblTeamBets
                                    where t.pkBetId == TeamBetId
                                    select t;

                            foreach (var r in b)
                            {
                                ListItem liCopyBet = new ListItem()
                                {
                                    Text = "Copy This Bet - " + r.strTeam + " " + r.strLogicArgument + " " +
                                    r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString()
                                };

                                ListItem liOppositeBet = new ListItem();

                                if (r.strLogicArgument.Trim() != "Will Have Equal To")
                                {
                                    if (r.strLogicArgument.Trim() == "Will Have More Than")
                                    {
                                        liOppositeBet.Text = "Copy Opposite Of This Bet - " + r.strTeam + " Will Have Less Than " +
                                        r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString();
                                    }
                                    else
                                    {
                                        liOppositeBet.Text = "Copy Opposite Of This Bet - " + r.strTeam + " Will Have More Than " +
                                        r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString();
                                    }
                                }

                                if (r.strLogicArgument.Trim() != "Will Have Equal To")
                                {
                                    rdoCopyBetSelection.Items.Add(liCopyBet);
                                    rdoCopyBetSelection.Items.Add(liOppositeBet);
                                }
                                else
                                {
                                    rdoCopyBetSelection.Items.Add(liCopyBet);
                                }

                            }
                        }
                    }
                }
                else
                {

                    int PlayerBetId;

                    if (Request.QueryString["BetId"] != null && int.TryParse(Request.QueryString["BetId"], out PlayerBetId))
                    {
                        GetModal("ModalCopyBet");

                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                            var b = from t in con.tblPlayerBets
                                    where t.pkBetId == PlayerBetId
                                    select t;

                            foreach (var r in b)
                            {
                                ListItem liCopyBet = new ListItem()
                                {
                                    Text = "Copy This Bet - " + r.strPlayerName + " " + r.strLogicArgument + " " +
                                    r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString()
                                };

                                ListItem liOppositeBet = new ListItem();

                                if (r.strLogicArgument.Trim() != "Will Have Equal To")
                                {
                                    if (r.strLogicArgument.Trim() == "Will Have More Than")
                                    {
                                        liOppositeBet.Text = "Copy Opposite Of This Bet - " + r.strPlayerName + " Will Have Less Than " +
                                        r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString();
                                    }
                                    else
                                    {
                                        liOppositeBet.Text = "Copy Opposite Of This Bet - " + r.strPlayerName + " Will Have More Than " +
                                        r.decArgumentValue.ToString() + " " + r.fkStatType + " during week " + r.intWeekNumber.ToString();
                                    }
                                }

                                if (r.strLogicArgument.Trim() != "Will Have Equal To")
                                {
                                    rdoCopyBetSelection.Items.Add(liCopyBet);
                                    rdoCopyBetSelection.Items.Add(liOppositeBet);
                                }
                                else
                                {
                                    rdoCopyBetSelection.Items.Add(liCopyBet);
                                }
                            }
                        }
                    }

                }

            }
            else
            {
                pnlMustBeLoggedId.Visible = true;
                ViewState["MustBeLoggedIn"] = true;
            }
        }



        protected void btnCopyBetModal_Click(object sender, EventArgs e)
        {
            CopyModalContent();         
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                tblChildBulletinBoard Child = new tblChildBulletinBoard()
                {
                    intBoardCreatorUserId = (int)Session["UserId"],
                    datDateTimeCreated = DateTime.Now,
                    intNumOfLikes = 0,
                    intParentBoardId = Convert.ToInt32(Request.QueryString["BulletinId"]),
                    strBoardMessage = txtReplyBulletin.Text.Trim()
                };

                con.tblChildBulletinBoards.InsertOnSubmit(Child);
                con.SubmitChanges();
            }

            ViewState["BulletinWasPosted"] = true;
            int BetId = Convert.ToInt32(Request.QueryString["BetId"]);
            bool IsTeamBet = Request.QueryString["Type"] == null ? true : false;

            if (IsTeamBet)
            {
                Response.Redirect("ViewBet.aspx?BetId=" + BetId.ToString());
            }
            else
            {
                Response.Redirect("ViewBet.aspx?Type=PlayerBet&BetId=" + BetId.ToString());
            }
        }

        private void GetReplyData()
        {
            int BoardId;

            if (int.TryParse(Request.QueryString["BulletinId"], out BoardId))
            {
                SqlContextDataContext con = new SqlContextDataContext();
                StringBuilder s = new StringBuilder();

                foreach (tblParentBulletinBoard pb in con.tblParentBulletinBoards.Where(x => x.intParentBoardId == BoardId))
                {
                    var UserNameAndImage = from t in con.tblUsers
                                           join u in con.tblUsersBettingStatistics on t.pkUserId equals u.fkUserId
                                           where t.pkUserId == pb.intUserIdWhoPostedBoard
                                           select new
                                           {
                                               UserName = t.strUserName,
                                               UserImage = t.bytUserImage,
                                               WinPerc = u.fltOverallWinningPercentage,

                                           };


                    foreach (var v in UserNameAndImage)
                    {


                        s.Append("Posted By" + v.UserName);
                        s.Append("<br />");
                        s.Append("<img src=\"data:Image/png;base64," + Convert.ToBase64String((byte[])v.UserImage.ToArray()) + "\"  class=\"img-circle\" width=\"50\"  height=\"50\" />");

                        if (v.WinPerc > .75)
                        {
                            s.Append("Win Perc=<span style=\"color:green;\">" + v.WinPerc.ToString() + "</span>");
                        }
                        else if (v.WinPerc < .25)
                        {
                            s.Append("Win Perc=<span style=\"color:green;\">" + v.WinPerc.ToString() + "</span>");
                        }



                    }

                    s.Append("@ " + pb.datDateTimeCreated.Value.ToShortDateString());
                    s.Append("<br />");

                    s.Append(pb.strBoardMessage);


                    litReply.Text = s.ToString();
                }



            }
        }
    }
}