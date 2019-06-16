using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace SocialSports
{
    public partial class BetBrilliantly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.Page.IsPostBack)
            {
                pnlLoginIsSuccessful.Visible = false;

                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    if(Request.QueryString["LogOut"] != null)
                    {               
                        pnlLoginIsSuccessful.Visible = true;
                    }
                }

                ListView1.DataSource = TeamBets.MostPopularTeams();
                ListView1.DataBind();

                lvPopularPlayers.DataSource = PlayerBets.GetPopularPlayers();
                lvPopularPlayers.DataBind();

                GetRecentlyCreatedBets();
                HighestWagers();
                SelectMostViewedBets();
                SelectMostViewedPlayerAndTeamBets();
            }

        //    BetResults B = new BetResults();
        //    B.MakePastBetsClosed(12);

  

        }

        private void SelectMostViewedBets()
        {
            SqlContextDataContext con = new SqlContextDataContext();

            using(con)
            {
                var r = con.tblTeamBets.GroupJoin(con.tblStatsPageVisits, 
                    b => b.pkBetId, v => v.strQueryStringValue, (bets, visits) => new
                {
                    Bets = bets,
                    Count = visits.Count()

                }).OrderByDescending(y => y.Count).Take(10);

             //   lvMostViewedTeamBets.DataSource = r;
            //    lvMostViewedTeamBets.DataBind();
            }

        }
      
        private void SelectMostViewedPlayerAndTeamBets()
        {

            using (SqlContextDataContext con = new SqlContextDataContext())
            {

             
               IEnumerable<object> Bets = con.tblPlayerBets.GroupJoin(con.tblStatsPageVisits,
                  b => b.pkBetId, v => v.strQueryStringValue, (x, visits) => new
                  {
                           Id = x.pkBetId,
                           Name = x.strPlayerName,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "PlayerBet",
                           Wager = x.decMoneyWager,
                           Count = visits.Count()

                  }).OrderByDescending(y => y.Count).Take(10);


               IEnumerable<object> TeamBets2 = con.tblTeamBets.GroupJoin(con.tblStatsPageVisits,
                  b => b.pkBetId, v => v.strQueryStringValue, (x, visits) => new
                  {
                           Id = x.pkBetId,
                           Name = x.strTeam,
                           StatArgument = x.fkStatType,
                           LogicArgument = x.strLogicArgument,
                           ValueArgument = x.decArgumentValue,
                           WeekNumber = x.intWeekNumber,
                           PlayerOrTeamBet = "TeamBet",
                           Wager = x.decMoneyWager,
                           Count = visits.Count()

                  }).OrderByDescending(y => y.Count).Take(10);

                IEnumerable<object> obj2 = Bets.Concat(TeamBets2);

                gvMostViewedBets.DataSource = obj2;
                gvMostViewedBets.DataBind();

            }
        }

        private void HighestWagers()
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {


                IEnumerable<object> Bets = con.tblPlayerBets.Select(x => new
                   {
                       Id = x.pkBetId,
                       Name = x.strPlayerName,
                       StatArgument = x.fkStatType,
                       LogicArgument = x.strLogicArgument,
                       ValueArgument = x.decArgumentValue,
                       WeekNumber = x.intWeekNumber,
                       PlayerOrTeamBet = "PlayerBet",
                       Wager = x.decMoneyWager

                   }).Concat(con.tblTeamBets.Select(x => new
                   {
                    Id = x.pkBetId,
                    Name = x.strTeam,
                    StatArgument = x.fkStatType,
                    LogicArgument = x.strLogicArgument,
                    ValueArgument = x.decArgumentValue,
                    WeekNumber = x.intWeekNumber,
                    PlayerOrTeamBet = "TeamBet",
                    Wager = x.decMoneyWager

                })).OrderByDescending(x => x.Wager).Take(10);


                gvHighestWagers.DataSource = Bets;
                gvHighestWagers.DataBind();

            }
        }

        private void GetRecentlyCreatedBets()
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                IEnumerable<object> Bets = con.tblPlayerBets.Select(x => new
                {
                    Id = x.pkBetId,
                    Name = x.strPlayerName,
                    StatArgument = x.fkStatType,
                    LogicArgument = x.strLogicArgument,
                    ValueArgument = x.decArgumentValue,
                    WeekNumber = x.intWeekNumber,
                    PlayerOrTeamBet = "PlayerBet",
                    Wager = x.decMoneyWager,

                }).Concat(con.tblTeamBets.Select(x => new
                {
                    Id = x.pkBetId,
                    Name = x.strTeam,
                    StatArgument = x.fkStatType,
                    LogicArgument = x.strLogicArgument,
                    ValueArgument = x.decArgumentValue,
                    WeekNumber = x.intWeekNumber,
                    PlayerOrTeamBet = "TeamBet",
                    Wager = x.decMoneyWager,

                })).OrderByDescending(x => x.Id).Take(10);


                gvRecentlyCreatedBets.DataSource = Bets;
                gvRecentlyCreatedBets.DataBind();

            }
        }

        protected void gvBets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                    SqlContextDataContext context = new SqlContextDataContext();

                    string TeamOrPlayer = DataBinder.Eval(e.Row.DataItem, "PlayerOrTeamBet").ToString();
                    Image Image = (Image)e.Row.FindControl("img");
                    LinkButton Link = (LinkButton)e.Row.FindControl("lbViewBet");
                    int BetId = Convert.ToInt32(((Label)e.Row.FindControl("lblBetId")).Text);

                    if (TeamOrPlayer == "TeamBet")
                    {
                        string TeamName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                        var teams = context.tblNflTeams.Where(y => y.strTeamName == TeamName).Select(y => y.strTeamImage);

                        foreach (var image in teams)
                        {
                            Image.ImageUrl = image;
                        }

                        Link.PostBackUrl = "ViewBet.aspx?BetId=" + BetId.ToString();
                    }
                    else
                    {
                        string PlayerName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                        var Players = context.tblNflPlayers.Where(y => y.strPlayerName == PlayerName).Select(y => y.strPlayerImage);

                        foreach (var image in Players)
                        {
                            Image.ImageUrl = image;
                        }

                        Link.PostBackUrl = "ViewBet.aspx?Type=PlayerBet&BetId=" + BetId.ToString();

                    }

            }
        }

       

    }
}