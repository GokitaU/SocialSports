using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;

namespace SocialSports
{
    public partial class MyProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (Request.UrlReferrer.ToString() == "http://localhost:32550/Login.aspx")
                {
                    pnlUserLoggedIn.Visible = true;
                }
                else
                {
                    pnlUserLoggedIn.Visible = false;
                }

                int UserId = Convert.ToInt32(Session["UserId"]);

                if (!this.Page.IsPostBack)
                {

                    //     UsersBets.GetBetWinPercentages(UserId, gvMyBetHistory);

                    LoadTreeView();

                    foreach (Panel pnl in PagePanels)
                    {
                        if (pnl.ID != "pnlMyBets")
                        {
                            pnl.Visible = false;
                        }
                    }

                    UserInfo ui = new UserInfo();
                    // Control[] arrControl = { gvMyCreatedBets, gvMyAcceptedBets };
                    GetMyCreatedBets(UserId, 1);
                    ui.UserId = UserId;
                    // ui.PopulateControlWithUserTeamBets(arrControl);
                    GetBetPredictions();
                    GetUserBetHistory(UserId);
                    GetMyAcceptedBets(UserId);
                    GetBetHistory(UserId);
                    GetUserAvatar((int)Session["UserId"]);
                    GetMyStats((int)Session["UserId"]);
                }
                else
                {

                }


                SqlContextDataContext db = new SqlContextDataContext();
                fvPaymentInformation.DataSource = from t in db.tblUsersPaymentInformations
                                                  where t.fkUserId == UserId
                                                  select t;

                fvPaymentInformation.DataBind();
            }
        }


        private void InsertUserFavoriteNflTeam(int UserId, int FavoriteTeamId)
        {
            using (SqlContextDataContext context = new SqlContextDataContext())
            {
                tblUsersFavoriteNflTeam NewUsersFavoriteTeam = new tblUsersFavoriteNflTeam
                {
                    fkSportsTeam = FavoriteTeamId,
                    fkUserId = UserId

                };

                context.tblUsersFavoriteNflTeams.InsertOnSubmit(NewUsersFavoriteTeam);
                context.SubmitChanges();

            }
        }

        private void GetBetHistory(int UserId)
        {
            using (SqlContextDataContext context = new SqlContextDataContext())
            {


                IEnumerable<int> BetIdsIsTeam = from t in context.tblBetHistories
                                                where (t.fkUserIdLoser == UserId
                                                   || t.fkUserIdWinner == UserId) && t.IsTeamBet == true
                                                select t.fkBetId;

                IEnumerable<int> BetIdsForPlayerBets = from t in context.tblBetHistories
                                                       where (t.fkUserIdLoser == UserId
                                                          || t.fkUserIdWinner == UserId) && t.IsTeamBet == false
                                                       select t.fkBetId;


                IEnumerable<object> MyBetHistory = context.tblTeamBets.Select(x =>
                    new
                    {
                        BetId = x.pkBetId,
                        Name = x.strTeam,
                        StatArgument = x.fkStatType,
                        LogicArgument = x.strLogicArgument,
                        ValueArgument = x.decArgumentValue,
                        WeekNumber = x.intWeekNumber,
                        PlayerOrTeamBet = "TeamBet",
                        UserId = x.fkCreatorUserId,
                        Wager = x.decMoneyWager
                    }).Where(x => BetIdsIsTeam.Contains(x.BetId))
                    .Union(context.tblPlayerBets.Select(x => new
                    {
                        BetId = x.pkBetId,
                        Name = x.strPlayerName,
                        StatArgument = x.fkStatType,
                        LogicArgument = x.strLogicArgument,
                        ValueArgument = x.decArgumentValue,
                        WeekNumber = x.intWeekNumber,
                        PlayerOrTeamBet = "PlayerBet",
                        UserId = x.fkCreatorUserId,
                        Wager = x.decMoneyWager

                    }).Where(x => BetIdsForPlayerBets.Contains(x.BetId)));





                gvMyBetHistory.DataSource = MyBetHistory;
                gvMyBetHistory.DataBind();



            }
        }

        private void GetMyStats(int UserId)
        {
            using (SqlContextDataContext context = new SqlContextDataContext())
            {
                IEnumerable<tblUsersBettingStatistic> stats = from u in context.tblUsersBettingStatistics where u.fkUserId == UserId
                            select u;

                    fvMyStats.DataSource = stats;
                    fvMyStats.DataBind();
                

            }
        }

        public void LoadTreeView()
        {
            SqlContextDataContext con = new SqlContextDataContext();

            var result = con.tblUserTreeViewItems.
            GroupJoin(con.tblUserTreeViewItems, p =>
            p.ID, c => c.ParentId, (parent, child) => new
            {
                Parent = parent,
                Child = child

            });

            foreach (var Parentt in result)
            {

                TreeNode node = new TreeNode() { Text = Parentt.Parent.TreeViewText };

                foreach (var Child in Parentt.Child)
                {
                    node.ChildNodes.Add(new TreeNode() { Text = Child.TreeViewText });
                }



            }
        }


        private void GetMyCreatedBets(int UserId, int PageIndex)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                IEnumerable<object> MyCreatedBets = con.tblTeamBets.Select(x =>
                       new
                       {
                           BetId = x.pkBetId,
                           Name = x.strTeam,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "TeamBet",
                           UserId = x.fkCreatorUserId,
                           Wager = x.decMoneyWager
                       })
                       .Concat(con.tblPlayerBets.Select(x => new
                       {
                           BetId = x.pkBetId,
                           Name = x.strPlayerName,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "PlayerBet",
                           UserId = x.fkCreatorUserId,
                           Wager = x.decMoneyWager

                       })).Where(x => x.UserId == UserId);

                int Total = Convert.ToInt32(MyCreatedBets.Count());
                int PageSize = 50;
                int Page = PageIndex;

                var skip = PageSize * (Page - 1);

                var canPage = skip < Total;


                gvMyCreatedBets.DataSource = MyCreatedBets.Skip(skip).Take(PageSize);
                gvMyCreatedBets.DataBind();

                int TotalPages = (Total / PageSize);
                List<ListItem> repeaterPages = new List<ListItem>();


                if (Total > 0)
                {
                  
                    for (int i = 1; i < TotalPages + 2; i++)
                    {
                        repeaterPages.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                }
                else
                {
                   

                }

                rptBets.DataSource = repeaterPages;
                rptBets.DataBind();



            }
        }

        private void GetMyAcceptedBets(int UserId)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                IQueryable<int> BetIds = from l in con.tblLinkUsersToBets
                                         where l.fkUserWhoAccepted == UserId
                                         select l.fkBetId;


                IEnumerable<object> MyAcceptedBets = con.tblTeamBets.Select(x =>
                       new
                       {
                           BetId = x.pkBetId,
                           Name = x.strTeam,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "TeamBet",
                           UserId = x.fkCreatorUserId,
                           Wager = x.decMoneyWager
                       })
                       .Concat(con.tblPlayerBets.Select(x => new
                       {
                           BetId = x.pkBetId,
                           Name = x.strPlayerName,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "PlayerBet",
                           UserId = x.fkCreatorUserId,
                           Wager = x.decMoneyWager

                       })).Where(x => BetIds.Contains(x.BetId));


                gvMyAcceptedBets.DataSource = MyAcceptedBets;
                gvMyAcceptedBets.DataBind();

            }
        }


        private void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SqlContextDataContext context = new SqlContextDataContext();

                string TeamOrPlayer = DataBinder.Eval(e.Row.DataItem, "PlayerOrTeamBet").ToString();
                Image Image = (Image)e.Row.FindControl("imgTeamOrPlayer");
                LinkButton ViewBet = (LinkButton)e.Row.FindControl("lbViewBet");

                if (TeamOrPlayer == "TeamBet")
                {
                    string TeamName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    var teams = context.tblNflTeams.Where(y => y.strTeamName == TeamName).Select(y => y.strTeamImage);

                    foreach (var image in teams)
                    {
                        Image.ImageUrl = image;

                    }

                    ViewBet.PostBackUrl = "ViewBet.aspx?BetId=" + DataBinder.Eval(e.Row.DataItem, "BetId").ToString();

                }
                else
                {
                    string PlayerName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    var Players = context.tblNflPlayers.Where(y => y.strPlayerName == PlayerName).Select(y => y.strPlayerImage);

                    foreach (var image in Players)
                    {
                        Image.ImageUrl = image;
                    }

                    ViewBet.PostBackUrl = "ViewBet.aspx?Type=PlayerBet&BetId=" + DataBinder.Eval(e.Row.DataItem, "BetId").ToString();

                }

            }


        }



        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            Menu m = sender as Menu;

            switch (m.SelectedItem.Text)
            {
                case "My Bets":

                    ShowHidePanels(PagePanels, "pnlMyBets");

                    break;
                case "My Predictions":
                    ShowHidePanels(PagePanels, "pnlMyPredictions");

                    break;

                case "My Payment Information":
                    ShowHidePanels(PagePanels, "pnlMyPaymentInformation");

                    break;
                case "My Bet History \\ History":
                    ShowHidePanels(PagePanels, "pnlMyBetHistory");

                    break;

                case "My Avatar":
                    ShowHidePanels(PagePanels, "pnlMyAvatar");

                    break;

            }

        }
        private void GetBetPredictions()
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                gvMyPredictions.DataSource = from t in con.tblTeamBets
                                             where
                                                 t.fkCreatorUserId == (int)Session["UserId"] && t.decMoneyWager == null
                                             select t;
                gvMyPredictions.DataBind();

            }
        }

        private Panel[] PagePanels
        {
            get
            {
                return

                    new Panel[]
                    {
                        pnlMyBetHistory,
                        pnlMyBets,
                        pnlMyPaymentInformation,
                        pnlMyPredictions,
                        pnlMyAvatar
                    };

            }
        }

        private void ShowHidePanels(Panel[] Panels, string PanelToShow)
        {
            for (int i = 0; i < Panels.Length; i++)
            {
                if (Panels[i].ID == PanelToShow)
                {
                    Panels[i].Visible = true;
                }
                else
                {
                    Panels[i].Visible = false;
                }
            }
        }

        private string GetHttpRequestHtml(string url, bool IsHtmlEncoded)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            string html = string.Empty;

            using (StreamReader rdr = new StreamReader(resp.GetResponseStream()))
            {

                if (IsHtmlEncoded)
                {
                    html += HttpUtility.HtmlEncode(rdr.ReadToEnd());
                }
                else
                {
                    html += rdr.ReadToEnd();
                }

                resp.Close();

            }

            return html;

        }

        private string PostSomething(string PostString, string Url)
        {
            string tag = "tag=APS.NET";
            string key = "key=[developerkey]";
            string PostData = string.Format("{0}&{1}", tag, key);

            Uri uri = new Uri(Url);

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);

                req.Method = WebRequestMethods.Http.Post;

                req.ContentLength = PostData.Length;
                req.ContentType = "application/x-www-form-urlencoded";

                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.Write(PostData);

                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                StreamReader rdr = new StreamReader(resp.GetResponseStream());
                string ResponseData = rdr.ReadToEnd();


                resp.Close();
                rdr.Close();

                return ResponseData;

            }

            return string.Empty;

        }

        private string DownloadOverFtp(string Url)
        {
            Uri uri = new Uri(Url);
            string returnContent = string.Empty;

            if (uri.Scheme == Uri.UriSchemeFtp)
            {
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uri);

                req.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse resp = req.GetResponse() as FtpWebResponse;

                using (StreamReader rdr = new StreamReader(resp.GetResponseStream()))
                {
                    returnContent = rdr.ReadToEnd();

                }

                resp.Close();

            }

            return returnContent;
        }


        private string MakeFileRequest(string Url)
        {
            Uri uri = new Uri(Url);
            string Content = string.Empty;

            if (uri.Scheme == Uri.UriSchemeFile)
            {
                FileWebRequest req = (FileWebRequest)FileWebRequest.Create(uri);

                FileWebResponse resp = req.GetResponse() as FileWebResponse;

                Stream stream = resp.GetResponseStream();

                using (StreamReader reader = new StreamReader(stream))
                {
                    Content = reader.ReadToEnd();
                }

                resp.Close();
            }

            return Content;
        }



        protected void fvPaymentInformation_ModeChanging(object sender, FormViewModeEventArgs e)
        {

            (sender as FormView).ChangeMode(e.NewMode);

            SqlContextDataContext db = new SqlContextDataContext();
            fvPaymentInformation.DataSource = from t in db.tblUsersPaymentInformations
                                              where t.fkUserId == (int)Session["UserId"]
                                              select t;

            fvPaymentInformation.DataBind();
        }

        protected void fvPaymentInformation_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView fv = (FormView)sender;
            DataKey key = fv.DataKey;
            string txtAccountNumber = ((TextBox)fv.FindControl("txtAccountNumber")).Text;
            string txtFirstName = ((TextBox)fv.FindControl("txtFirstName")).Text;
            string txtLastName = ((TextBox)fv.FindControl("txtLastName")).Text;

            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                tblUsersPaymentInformation pay = con.tblUsersPaymentInformations.SingleOrDefault(x => x.fkUserId == (int)Session["UserId"]);
                pay.strAccountNumber = txtAccountNumber.Trim();
                pay.strFirstName = txtFirstName.Trim();
                pay.strLastName = txtLastName.Trim();

                con.SubmitChanges();
            }

        }

        private void GetUserBetHistory(int UserId)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                /*  var bets = from b in con.tblTeamBets
                             join
                                 h in con.tblBetHistories on b.pkBetId equals
                                 h.fkBetId
                             where h.fkUserIdLoser == UserId ||
                                 h.fkUserIdWinner == UserId
                             select new { b.pkBetId, h.fkUserIdWinner };
                  */

                var Stats = from b in con.tblTeamBets
                            join
                                h in con.tblBetHistories on b.pkBetId equals
                                h.fkBetId
                            where h.fkUserIdWinner == UserId || h.fkUserIdLoser == UserId
                            group h by new
                            {
                                WinnerId = h.fkUserIdWinner,
                                LoserId = h.fkUserIdWinner,
                                BetId = h.fkBetId
                            } into eGroup
                            select new
                            {
                                Key = eGroup.Key,
                                WinCount = eGroup.Count(x => x.fkUserIdWinner == UserId),
                                LostCount = eGroup.Count(y => y.fkUserIdLoser == UserId)
                            };

                foreach (var Stat in Stats)
                {
                    Response.Write((Stat.WinCount + " --- " + Stat.LostCount));
                }

                //  gvMyBetHistory.DataSource = bets;
                //  gvMyBetHistory.DataBind();
            }


        }

        protected void fvPaymentInformation_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            (sender as FormView).ChangeMode(FormViewMode.ReadOnly);
        }

        protected void gvMyCreatedBets_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            OnRowDataBound(sender, e);


        }

        protected void gvMyAcceptedBets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            OnRowDataBound(sender, e);

        }

        protected void gvMyCreatedBets_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int RowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            string TeamOrPlayer = e.CommandArgument.ToString();
            GridView gv = (GridView)sender;
            int BetId = Convert.ToInt32(((Label)gv.Rows[RowIndex].FindControl("lblBetId")).Text);

            if (e.CommandName == "CancelBet")
            {

                using (SqlContextDataContext context = new SqlContextDataContext())
                {
                    if (TeamOrPlayer == "TeamBet")
                    {
                      
                            tblTeamBet TeamBet = context.tblTeamBets.SingleOrDefault(x => x.pkBetId == BetId);
                            context.tblTeamBets.DeleteOnSubmit(TeamBet);
                            context.SubmitChanges();

                            int TeamLinkCount = context.tblLinkUsersToBets.Count(x => x.fkBetId == BetId && x.blnIsTeamBet == true);

                            if (TeamLinkCount != 0)
                            {
                                tblLinkUsersToBet DeleteBet = context.tblLinkUsersToBets.SingleOrDefault(x => x.fkBetId == BetId && x.blnIsTeamBet == true);
                                context.tblLinkUsersToBets.DeleteOnSubmit(DeleteBet);
                                context.SubmitChanges();
                            }
                    }
                    else
                    {

                        tblPlayerBet PlayerBet = context.tblPlayerBets.SingleOrDefault(x => x.pkBetId == BetId);
                        context.tblPlayerBets.DeleteOnSubmit(PlayerBet);
                        context.SubmitChanges();

                        int PlayerLinkCount = context.tblLinkUsersToBets.Count(x => x.fkBetId == BetId && x.blnIsTeamBet == false);

                        if (PlayerLinkCount != 0)
                        {
                         
                            tblLinkUsersToBet DeleteBet = context.tblLinkUsersToBets.SingleOrDefault(x => x.fkBetId == BetId && x.blnIsTeamBet == false);
                            context.tblLinkUsersToBets.DeleteOnSubmit(DeleteBet);
                            context.SubmitChanges();

                        }
                    }                                
                }

                GetMyCreatedBets((int)Session["UserId"], 1);
            }
            else if (e.CommandName == "SendBetEmail")
            {

            }
        }

        private void GetUserAvatar(int UserId)
        {
            SqlContextDataContext con = new SqlContextDataContext();

            var Image = from user in con.tblUsers
                        where user.pkUserId == UserId
                        select new
                        {
                            UserImage = user.bytUserImage
                        };
            foreach (var img in Image)
            {
                imgAvatar.ImageUrl = "data:Image/png;base64," + Convert.ToBase64String((byte[])img.UserImage.ToArray());
            }

        }

        protected void btnUploadAvatar_Click(object sender, EventArgs e)
        {
            HttpPostedFile Avatar = filUploadAvatar.PostedFile;
            string FileName = Path.GetFileName(filUploadAvatar.PostedFile.FileName);
            string FileExtension = Path.GetExtension(FileName);
            int FileSize = Avatar.ContentLength;


            if (FileExtension == ".jpg" || FileExtension == ".gif"
                || FileExtension == ".png" || FileExtension == ".bmp")
            {
                Stream s = Avatar.InputStream;
                BinaryReader rdr = new BinaryReader(s);
                byte[] ImageBytes = rdr.ReadBytes((int)s.Length);

                using (SqlContextDataContext con = new SqlContextDataContext())
                {
                    tblUser User = con.tblUsers.Single(x => x.pkUserId == (int)Session["UserId"]);
                    User.bytUserImage = ImageBytes;
                    con.SubmitChanges();
                }

                GetUserAvatar((int)Session["UserId"]);
            }
        }

        protected void gvMyAcceptedBets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            string TeamOrPlayer = e.CommandArgument.ToString();
            GridView gv = (GridView)sender;
            int BetId = Convert.ToInt32(((Label)gv.Rows[RowIndex].FindControl("lblBetId")).Text);

            if (e.CommandName == "CancelBet")
            {
                using (SqlContextDataContext context = new SqlContextDataContext())
                {

                    if (TeamOrPlayer == "TeamBet")
                    {


                        tblTeamBet TeamBet = context.tblTeamBets.SingleOrDefault(x => x.pkBetId == BetId);
                        TeamBet.blnBetIsOpened = true;
                        context.SubmitChanges();

                        tblLinkUsersToBet DeleteBet = context.tblLinkUsersToBets.SingleOrDefault(x => x.fkBetId == BetId && x.blnIsTeamBet == true);
                        context.tblLinkUsersToBets.DeleteOnSubmit(DeleteBet);
                        context.SubmitChanges();


                    }
                    else
                    {

                        tblLinkUsersToBet DeleteBet = context.tblLinkUsersToBets.SingleOrDefault(x => x.fkBetId == BetId && x.blnIsTeamBet == false);
                        context.tblLinkUsersToBets.DeleteOnSubmit(DeleteBet);
                        context.SubmitChanges();

                        tblPlayerBet PlayerBet = context.tblPlayerBets.SingleOrDefault(x => x.pkBetId == BetId);
                        PlayerBet.blnBetIsOpened = true;
                        context.SubmitChanges();

                    }
                }

                GetMyAcceptedBets((int)Session["UserId"]);
            }
        }

        protected void lnkRepeater_Click(object sender, EventArgs e)
        {
            int PageIndex = int.Parse((sender as LinkButton).CommandArgument);
            GetMyCreatedBets((int)Session["UserId"], PageIndex);

            Session["PageIndex"] = PageIndex;
        }

   

    }
}
