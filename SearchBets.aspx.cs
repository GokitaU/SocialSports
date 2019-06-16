using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.Linq;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace SocialSports
{
    public partial class SearchBets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.Page.IsPostBack)
            {
                LoadData();

                int WeekNumber;

                if (Request.QueryString["WeekNumber"] != null && int.TryParse(Request.QueryString["WeekNumber"], out WeekNumber))
                {
                    if (Request.QueryString["HomeTeam"] != null && Request.QueryString["VisitorTeam"] != null)
                    {
                        
                        ddlTeamWeekNumber.SelectedIndex = WeekNumber;

                        foreach (ListItem li in lbTeams.Items)
                        {
                            if (li.Text == Request.QueryString["HomeTeam"] || li.Text == Request.QueryString["VisitorTeam"])
                            {
                                li.Selected = true;
                            }
                        }

                        btnSearchTeamBets_Click(this, new EventArgs());
                    }
                }

                pnlMustBeLoggedId.Visible = false;
                pnlBetAccepted.Visible = false;
                pnlPlayerBets.Visible = false; 
                pnlTeamBets.Visible = true;
                pnlPlayerBetsGv.Visible = false;

                if (Session["UserId"] != null)
                {
                    LoadSuggestedBets((int)Session["UserId"]);

                }

                chkIsOpened.Checked = true;
                chkPlayerOnlyOpened.Checked = true;

                SearchTeamBets(1, "Team", true, 10);
                SearchPlayerBets(1, "Player", "ASC", Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));

                IsTeamBet();

                if (Request.QueryString["Team"] != null)
                {
                    string Team = Request.QueryString["Team"];

                    lbTeams.SelectedValue = Team;
                    SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
                    rdoTypeOfBet.SelectedIndex = 0;
                }
                else if (Request.QueryString["PlayerTeam"] != null && Request.QueryString["Player"] != null)
                {
                    string PlayerTeam = Request.QueryString["PlayerTeam"];
                    string Player = Request.QueryString["Player"];

                    pnlPlayerBets.Visible = true;
                    pnlTeamBets.Visible = false;
                    pnlTeamBetsGv.Visible = false;
                    pnlPlayerBetsGv.Visible = true;

                    rdoTypeOfBet.SelectedIndex = 1;
                    ddlPlayersTeam.SelectedValue = PlayerTeam;
                    ddlPlayersTeam_SelectedIndexChanged(this, new EventArgs());

                    lbPlayers.SelectedValue = Player;

                    lbPlayers_SelectedIndexChanged(lbPlayers, new EventArgs());


                    if (ddlTopSortByPlayer.SelectedIndex != 0)
                    {
                        SearchPlayerBets(1, ddlTopSortByPlayer.SelectedItem.Text, ddlTopAscOrDescPlayer.SelectedValue, Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
                    }
                    else
                    {
                        SearchPlayerBets(1, "Player", "ASC", Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
                    }


                }





                if (ViewState["BetAccepted"] != null)
                {
                    ViewState["BetAccepted"] = null;
                }

            }
        }


        private void LoadData()
        {
            using (SqlContextDataContext c = new SqlContextDataContext())
            {

                var r = c.tblNflStatisticalCategoryTypes.Select(x => new { x.strStatisticalCategoryTypeName });

                foreach (var arg in r)
                {
                    lbTeamStatArguments.Items.Add(arg.strStatisticalCategoryTypeName);
                }

                var q = c.tblNflTeams.Select(x => new { TeamId = x.pkTeamId, TeamName = x.strTeamName }).OrderBy(x => x.TeamName);

                foreach (var team in q)
                {
                    ListItem li = new ListItem();
                    li.Text = team.TeamName;
                    li.Value = team.TeamName;

                    lbTeams.Items.Add(li);

                    ddlPlayersTeam.Items.Add(team.TeamName);
                }
            }

        }

        protected void rdoTypeOfBet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButtonList).SelectedItem.Text == "Team Bet")
            {
                pnlTeamBetsGv.Visible = true;
                pnlTeamBets.Visible = true;

                pnlPlayerBetsGv.Visible = false;
                pnlPlayerBets.Visible = false;

            }
            else
            {
                pnlTeamBetsGv.Visible = false;
                pnlTeamBets.Visible = false;

                pnlPlayerBetsGv.Visible = true;
                pnlPlayerBets.Visible = true;
            }

            HttpCookie cook = new HttpCookie("TypeOfBet", rdoTypeOfBet.SelectedIndex.ToString());
            HttpContext.Current.Response.SetCookie(cook);
        }

        protected void btnSearchTeamBets_Click(object sender, EventArgs e)
        {
            SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));

            pnlPlayerBetsGv.Visible = false;
            pnlTeamBetsGv.Visible = true;
            pnlTeamBets.Visible = true;
            pnlPlayerBets.Visible = false;


            SaveSearchCriteria();

        }

        private void SearchTeamBets(int PageNumber, string OrderBy, bool IsAsc, int AdjPageSize)
        {

            List<string> TeamsSelected = new List<string>();
            List<string> StatsSelected = new List<string>();
            List<string> LogicArgumentsSelected = new List<string>();

            StringBuilder s = new StringBuilder();

            if (lbTeams.SelectedItem != null)
            {
                s.Append("The Following Teams: ");

                for (int i = 0; i < lbTeams.Items.Count; i++)
                {
                    if (lbTeams.Items[i].Selected)
                    {
                        TeamsSelected.Add(lbTeams.Items[i].Text);
                        s.Append(lbTeams.Items[i].Text);
                    }
                }
            }

            if (ddlTeamLogic.SelectedIndex != 0)
            {
                foreach (ListItem li in ddlTeamLogic.Items)
                {
                    if (li.Selected)
                    {
                        LogicArgumentsSelected.Add(li.Text);
                        s.Append(li.Text + " ");

                    }
                }
            }

            if (txtTeamArgumentValue.Text != string.Empty && !string.IsNullOrEmpty(txtTeamArgumentValue.Text))
            {
                s.Append(txtTeamArgumentValue.Text.Trim() + " ");

            }

            if (lbTeamStatArguments.SelectedItem != null)
            {
                foreach (ListItem li in lbTeamStatArguments.Items)
                {
                    if (li.Selected)
                    {
                        StatsSelected.Add(li.Text);
                        s.Append(li.Text);

                    }
                }
            }


            var ts = TeamsSelected;

            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                var query = con.tblTeamBets.AsQueryable<tblTeamBet>();

                if (StatsSelected.Count > 0)
                {

                    query = from bet in query
                            where StatsSelected.Contains(bet.fkStatType)
                            select bet;

                }

                if (TeamsSelected.Count > 0)
                {
                    query = from bet in query
                            where TeamsSelected.Contains(bet.strTeam)
                            select bet;
                }
                if (LogicArgumentsSelected.Count > 0)
                {
                    query = from bet in query
                            where LogicArgumentsSelected.Contains(bet.strLogicArgument)
                            select bet;

                }

                if (ddlTeamLogic.SelectedIndex != 0)
                {
                    if (txtTeamArgumentValue.Text != "")
                    {
                        decimal decValue;

                        if (decimal.TryParse(txtTeamArgumentValue.Text, out decValue))
                        {
                            switch (ddlTeamLogic.SelectedItem.Text)
                            {
                                case "Will Have More Than":

                                    query = from bet in query
                                            where bet.decArgumentValue < decValue
                                            select bet;

                                    break;
                                case "Will Have Equal To":

                                    query = from bet in query
                                            where bet.decArgumentValue == decValue
                                            select bet;

                                    break;
                                case "Will Have Less Than":

                                    query = from bet in query
                                            where bet.decArgumentValue > decValue
                                            select bet;

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }


                        }
                    }
                }

                if (ddlTeamWeekNumber.SelectedIndex != 0)
                {
                    query = from bet in query
                            where bet.intWeekNumber == ddlTeamWeekNumber.SelectedIndex
                            select bet;
                }

                decimal decWagerStart;
                decimal decWagerEnd;

                if (decimal.TryParse(txtWagerStart.Text, out decWagerStart) && decimal.TryParse(txtWagerEnd.Text, out decWagerEnd))
                {
                    query = from bet in query
                            where bet.decMoneyWager > decWagerStart && bet.decMoneyWager < decWagerEnd
                            select bet;

                }
                else if (decimal.TryParse(txtWagerStart.Text, out decWagerStart))
                {
                    query = from bet in query
                            where bet.decMoneyWager > decWagerStart
                            select bet;
                }

                else if (decimal.TryParse(txtWagerEnd.Text, out decWagerEnd))
                {
                    query = from bet in query
                            where bet.decMoneyWager < decWagerEnd
                            select bet;
                }

                switch (OrderBy)
                {
                    case "Wager Amount":

                        if (IsAsc)
                        {
                            query = from bet in query orderby bet.decMoneyWager ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.decMoneyWager descending select bet;
                        }

                        break;
                    case "Team":

                        if (IsAsc)
                        {
                            query = from bet in query orderby bet.strTeam ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.strTeam descending select bet;
                        }

                        break;
                    case "Logic Argument":

                        if (IsAsc)
                        {
                            query = from bet in query orderby bet.strLogicArgument ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.strLogicArgument descending select bet;
                        }

                        break;
                    case "Statistical Argument":

                        if (IsAsc)
                        {
                            query = from bet in query orderby bet.fkStatType ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.fkStatType descending select bet;
                        }

                        break;
                    case "Argument Value":

                        if (IsAsc)
                        {
                            query = from bet in query orderby bet.decArgumentValue ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.decArgumentValue descending select bet;
                        }

                        break;
                }

                if (chkIsOpened.Checked)
                {
                    query = from bet in query
                            where bet.blnBetIsOpened == chkIsOpened.Checked
                            select bet;
                }

                int Total = Convert.ToInt32(query.Count());
                int PageSize = AdjPageSize;
                int Page = PageNumber;

                var skip = PageSize * (Page - 1);

                var canPage = skip < Total;


                gvTeamBets.DataSource = query.Skip(skip).Take(PageSize);
                gvTeamBets.DataBind();

                int TotalPages = (Total / PageSize);
                List<ListItem> repeaterPages = new List<ListItem>();


                if (Total > 0)
                {
                    lblTeamResultsCount.Text = Total.ToString() + " Results Found!";
                    lblTeamResultsCount.ForeColor = System.Drawing.Color.Green;

                    for (int i = 1; i < TotalPages + 2; i++)
                    {
                        repeaterPages.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                }
                else
                {
                    lblTeamResultsCount.Text = "0 Results Found";
                    lblTeamResultsCount.ForeColor = System.Drawing.Color.Red;
                }

                rptBets.DataSource = repeaterPages;
                rptBets.DataBind();


            }


            decimal decAmount;

            decimal.TryParse(txtTeamArgumentValue.Text, out decAmount);

            string Team = lbTeams.SelectedItem == null ? null : lbTeams.SelectedItem.Text;
            string TeamLogicArg = ddlTeamLogic.SelectedItem == null ? null : ddlTeamLogic.SelectedItem.Text;
            string StatArg = lbPlayerStatArgument.SelectedItem == null ? null : lbPlayerStatArgument.SelectedItem.Text;

            if (Session["UserId"] != null)
            {

                UserTracking.MemberTracking t = new UserTracking.MemberTracking();
                Guid guid = Guid.NewGuid();

                foreach (ListItem li in lbTeams.Items)
                {
                    if (li.Selected)
                    {
                        t.InsertSearchQuery((int)Session["UserId"], false, guid, li.Text, string.Empty, TeamLogicArg, decAmount);
                    }
                }

                foreach (ListItem li in lbTeamStatArguments.Items)
                {
                    if (li.Selected)
                    {
                        t.InsertSearchQuery((int)Session["UserId"], false, guid, string.Empty, li.Text, TeamLogicArg, decAmount);
                    }
                }
            }



        }

        private void LoadSuggestedBets(int UserId)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                var searches = from t in con.tblStatsBetSearchQueries
                               where t.fkUserId == UserId
                               group t by new
                                   {
                                       t.strEntityName,
                                       t.strStatArgument,
                                       t.guidQuery,
                                       t.decWagerAmount

                                   }

                                   into eGroup
                                   orderby eGroup.Key.strEntityName, eGroup.Key.strStatArgument descending
                                   select new
                                   {
                                       TeamName = eGroup.Key.strEntityName,
                                       StatName = eGroup.Key.strStatArgument,
                                       TeamCount = eGroup.Key.strEntityName.Count(),
                                       TeamArgCount = eGroup.Key.strStatArgument.Count(),
                                       SearchGuid = eGroup.Key.guidQuery,
                                       WagerAmount = eGroup.Key.decWagerAmount
                                   };


                var betsVisited = from
                                       b in con.tblTeamBets
                                  join
                                      v in con.tblStatsPageVisits
                                      on b.pkBetId equals v.strQueryStringValue

                                  where v.fkUserId == UserId
                                  select new
                                  {
                                      b.pkBetId,
                                      b.strTeam,
                                      b.fkStatType

                                  };

                Dictionary<string, int> TeamNameAndCount = new Dictionary<string, int>();

                foreach (var s in searches)
                {
                    if (TeamNameAndCount.ContainsKey(s.TeamName))
                    {
                        int NumberOfTeamSearches;

                        TeamNameAndCount.TryGetValue(s.TeamName, out NumberOfTeamSearches);

                        int ModedNumberOfSearches = NumberOfTeamSearches + 1;

                        TeamNameAndCount[s.TeamName] = ModedNumberOfSearches;

                    }
                    else
                    {
                        TeamNameAndCount[s.TeamName] = 1;
                    }
                }

                Dictionary<string, decimal> TeamAndPercentage = new Dictionary<string, decimal>();
                List<int> ints = TeamNameAndCount.Values.ToList();
                int Total = 0;

                for (int i = 0; i < TeamNameAndCount.Count; i++)
                {
                    Total += ints[i];
                }

                foreach (KeyValuePair<string, int> kvp in TeamNameAndCount)
                {
                    TeamAndPercentage.Add(kvp.Key, (Convert.ToDecimal(kvp.Value) / Total));
                }

                using (SqlContextDataContext context = new SqlContextDataContext())
                {
                    var query = context.tblTeamBets.AsQueryable<tblTeamBet>();

                    List<string> NflTeams = new List<string>(20);

                    foreach (KeyValuePair<string, decimal> kvp in TeamAndPercentage)
                    {
                        int RoundedNumber = Convert.ToInt32(Math.Round(kvp.Value, 1) * 10);

                        Response.Write(kvp.Key.ToString() + "<br />");
                        Response.Write(kvp.Value.ToString());

                        for (int i = 0; i < RoundedNumber; i++)
                        {
                            NflTeams.Add(kvp.Key);
                        }
                    }

                    query = (from t in query
                            where NflTeams.Contains(t.strTeam)
                            select t).Take(20).OrderBy(x => x.strTeam);

                   

                    lvRecommendedBets.DataSource = query;
                    lvRecommendedBets.DataBind();




                }



            }
        }


        private RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        private int RandomInteger(int Min, int Max)
        {
            uint MaxValue = uint.MaxValue;
            while (MaxValue == uint.MaxValue)
            {

                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                MaxValue = BitConverter.ToUInt32(four_bytes, 0);
            }

            return (int)(Min + (Max - Min) * (MaxValue / (double)uint.MaxValue));
        }


        protected void ddlPlayersTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlContextDataContext c = new SqlContextDataContext();
            string TeamName = ddlPlayersTeam.SelectedItem.Text;

            var r = c.tblNflPlayers.Select(x => new { x.strPlayerName, x.strPlayerPosition, x.pkPlayerId, x.fkPlayerTeam }).Where(y => y.fkPlayerTeam == TeamName).OrderBy(o => o.strPlayerName);

            lbPlayers.Items.Clear();

            foreach (var arg in r)
            {
                ListItem li = new ListItem();
                li.Text = arg.strPlayerName;
                li.Value = arg.strPlayerName;

                lbPlayers.Items.Add(li);
            }
        }

        protected void btnSearchPlayerBets_Click(object sender, EventArgs e)
        {
            SearchPlayerBets(1, "Wager Amount", "DESC", Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));

            pnlPlayerBetsGv.Visible = true;
            pnlTeamBetsGv.Visible = false;
            pnlTeamBets.Visible = false;
            pnlPlayerBets.Visible = true;

            SaveSearchCriteriaForPlayerBets();
        }

        protected void lbPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                if (lbPlayers.SelectedItem != null)
                {

                    lbPlayerStatArgument.Items.Clear();

                    foreach (ListItem li in (sender as ListBox).Items)
                    {
                        if (li.Selected)
                        {

                            var results = con.tblNflStatisticalCategoryTypesOffensivePlayers.Join(con.tblNflPlayers, x => x.strOffensivePosition, y => y.strPlayerPosition, (positions, players) =>
                                new
                                {
                                    players.strPlayerName,
                                    positions.strOffensivePosition,
                                    positions.strStatisticalCategoryTypeName

                                }).Where
                                (z => z.strPlayerName == li.Value);

                            foreach (var r in results)
                            {
                                lbPlayerStatArgument.Items.Add(r.strStatisticalCategoryTypeName);
                            }

                        }


                    }
                }
            }
        }



        private void SearchPlayerBets(int PageNumber, string SortBy, string OrderBy, int PageSize)
        {
            List<string> PlayersSelected = new List<string>();
            List<string> StatsSelected = new List<string>();
            List<string> LogicArgumentsSelected = new List<string>();
            string TeamSelected = string.Empty;

            StringBuilder s = new StringBuilder();

            if (ddlPlayersTeam.SelectedIndex != 0)
            {
                TeamSelected = ddlPlayersTeam.SelectedItem.Text;
            }

            if (lbPlayers.SelectedItem != null)
            {

                for (int i = 0; i < lbPlayers.Items.Count; i++)
                {
                    if (lbPlayers.Items[i].Selected)
                    {
                        PlayersSelected.Add(lbPlayers.Items[i].Text);
                    }
                }
            }

            if (ddlPlayerLogic.SelectedIndex != 0)
            {
                foreach (ListItem li in ddlPlayerLogic.Items)
                {
                    if (li.Selected)
                    {
                        LogicArgumentsSelected.Add(li.Text);

                    }
                }
            }

            if (txtPlayerArgumentValue.Text != string.Empty && !string.IsNullOrEmpty(txtPlayerArgumentValue.Text))
            {

            }

            if (lbPlayerStatArgument.SelectedItem != null)
            {
                foreach (ListItem li in lbPlayerStatArgument.Items)
                {
                    if (li.Selected)
                    {
                        StatsSelected.Add(li.Text);

                    }
                }
            }


            var ts = PlayersSelected;

            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                var query = con.tblPlayerBets.AsQueryable<tblPlayerBet>();


                if (StatsSelected.Count > 0)
                {

                    query = from bet in query
                            where StatsSelected.Contains(bet.fkStatType)
                            select bet;
                }

                if (PlayersSelected.Count > 0)
                {
                    query = from bet in query
                            where PlayersSelected.Contains(bet.strPlayerName)
                            select bet;
                }
                else
                {
                    List<string> Players = new List<string>();

                    foreach (ListItem li in lbPlayers.Items)
                    {
                        Players.Add(li.Text);
                    }

                    query = from bet in query
                            where Players.Contains(bet.strPlayerName)
                            select bet;

                }

                if (LogicArgumentsSelected.Count > 0)
                {
                    query = from bet in query
                            where LogicArgumentsSelected.Contains(bet.strLogicArgument)
                            select bet;

                }

                if (ddlPlayerLogic.SelectedIndex != 0)
                {
                    if (txtPlayerArgumentValue.Text != "")
                    {
                        decimal decValue;

                        if (decimal.TryParse(txtPlayerArgumentValue.Text, out decValue))
                        {
                            switch (ddlPlayerLogic.SelectedItem.Text)
                            {
                                case "Will Have More Than":

                                    query = from bet in query
                                            where bet.decArgumentValue < decValue
                                            select bet;

                                    break;
                                case "Will Have Equal To":

                                    query = from bet in query
                                            where bet.decArgumentValue == decValue
                                            select bet;

                                    break;
                                case "Will Have Less Than":

                                    query = from bet in query
                                            where bet.decArgumentValue > decValue
                                            select bet;

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }


                        }
                    }
                }

                if (ddlPlayerWeekNumber.SelectedIndex != 0)
                {
                    query = from bet in query
                            where bet.intWeekNumber == ddlPlayerWeekNumber.SelectedIndex
                            select bet;
                }


                decimal decWagerStart;
                decimal decWagerEnd;

                if (decimal.TryParse(txtPlayerWageStart.Text, out decWagerStart) && decimal.TryParse(txtPlayerWageEnd.Text, out decWagerEnd))
                {
                    query = from bet in query
                            where bet.decMoneyWager > decWagerStart && bet.decMoneyWager < decWagerEnd
                            select bet;

                }
                else if (decimal.TryParse(txtPlayerWageStart.Text, out decWagerStart))
                {
                    query = from bet in query
                            where bet.decMoneyWager > decWagerStart
                            select bet;
                }

                else if (decimal.TryParse(txtPlayerWageEnd.Text, out decWagerEnd))
                {
                    query = from bet in query
                            where bet.decMoneyWager < decWagerEnd
                            select bet;
                }


                switch (SortBy)
                {
                    case "Wager Amount":

                        if (OrderBy == "ASC")
                        {
                            query = from bet in query orderby bet.decMoneyWager ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.decMoneyWager descending select bet;
                        }
                        break;

                    case "Player Name":

                        if (OrderBy == "ASC")
                        {
                            query = from bet in query orderby bet.strPlayerName ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.strPlayerName descending select bet;
                        }
                        break;

                    case "Logic Argument":

                        if (OrderBy == "ASC")
                        {
                            query = from bet in query orderby bet.strLogicArgument ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.strLogicArgument descending select bet;
                        }

                        break;

                    case "Statistical Argument":

                        if (OrderBy == "ASC")
                        {
                            query = from bet in query orderby bet.fkStatType ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.fkStatType descending select bet;
                        }

                        break;

                    case "Argument Value":

                        if (OrderBy == "ASC")
                        {
                            query = from bet in query orderby bet.decArgumentValue ascending select bet;
                        }
                        else
                        {
                            query = from bet in query orderby bet.decArgumentValue descending select bet;
                        }

                        break;
                }

                if (chkPlayerOnlyOpened.Checked)
                {
                    query = from bet in query
                            where bet.blnBetIsOpened == chkPlayerOnlyOpened.Checked
                            select bet;
                }

                int Total = Convert.ToInt32(query.Count());
                int Page = PageNumber;

                var skip = PageSize * (Page - 1);

                var canPage = skip < Total;


                gvPlayerBets.DataSource = query.Skip(skip).Take(PageSize);
                gvPlayerBets.DataBind();

                int TotalPages = (Total / PageSize);

                List<ListItem> repeaterPages = new List<ListItem>();

                if (Total > 0)
                {
                    lblPlayerBetResultCount.Text = Total.ToString() + " Results Found!";
                    lblPlayerBetResultCount.ForeColor = System.Drawing.Color.Green;

                    for (int i = 1; i < TotalPages + 2; i++)
                    {
                        repeaterPages.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                }
                else
                {
                    lblPlayerBetResultCount.Text = "0 Results Found";
                    lblPlayerBetResultCount.ForeColor = System.Drawing.Color.Red;
                }

                rptPlayerBets.DataSource = repeaterPages;
                rptPlayerBets.DataBind();



            }
        }

        protected void gvTeamBets_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            SqlContextDataContext con = new SqlContextDataContext();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool BetIsClosed = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "blnBetIsOpened").ToString());
                string TeamName = DataBinder.Eval(e.Row.DataItem, "strTeam").ToString();
                Image TeamImage = (Image)e.Row.FindControl("imgTeamImage");

                if (!BetIsClosed)
                {
                    LinkButton lb = (LinkButton)e.Row.FindControl("lbAcceptBet");
                    lb.Visible = false;

                    e.Row.Cells[1].Controls.Clear();
                    e.Row.Cells[1].Controls.Add(new LiteralControl() { Text = "Closed Bet", });
                }


                if (Session["UserId"] != null)
                {
                    if (Convert.ToInt32(((Label)e.Row.FindControl("UserCreatorId")).Text) == (int)Session["UserId"])
                    {
                        e.Row.Cells[1].Controls.Clear();
                        e.Row.Cells[1].Controls.Add(new LiteralControl() { Text = "My Bet", });
                    }
                }

                var teams = con.tblNflTeams.Where(y => y.strTeamName == TeamName).Select(y => y.strTeamImage);

                foreach (var image in teams)
                {
                    TeamImage.ImageUrl = image;
                }

            }

        }


        protected void lnkRepeater_Click(object sender, EventArgs e)
        {

            int PageIndex = int.Parse((sender as LinkButton).CommandArgument);
            SearchTeamBets(PageIndex, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));

            Session["PageIndex"] = PageIndex;

        }

        protected void gvPlayerBets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    bool BetIsOpen = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "blnBetIsOpened").ToString());
                    string PlayerName = DataBinder.Eval(e.Row.DataItem, "strPlayerName").ToString();
                    Image PlayerImage = (Image)e.Row.FindControl("imgPlayerImage");

                    if (!BetIsOpen)
                    {
                        LinkButton lb = (LinkButton)e.Row.FindControl("lbAcceptBet");
                        lb.Visible = false;

                        e.Row.Cells[1].Controls.Clear();
                        e.Row.Cells[1].Controls.Add(new LiteralControl() { Text = "Closed Bet", });
                    }

                    if (Session["UserId"] != null)
                    {
                        if (Convert.ToInt32(((Label)e.Row.FindControl("UserCreatorId")).Text) == (int)Session["UserId"])
                        {
                            e.Row.Cells[1].Controls.Clear();
                            e.Row.Cells[1].Controls.Add(new LiteralControl() { Text = "My Bet", });
                        }
                    }

                    var teams = con.tblNflPlayers.Where(y => y.strPlayerName == PlayerName).Select(y => y.strPlayerImage);

                    foreach (var image in teams)
                    {
                        PlayerImage.ImageUrl = image;
                    }

                }
            }
        }

        protected void ddlTopSortByPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTopSortByPlayer.SelectedIndex != 0)
            {
                SearchPlayerBets(1, (sender as DropDownList).SelectedItem.Text, ddlTopAscOrDescPlayer.SelectedValue, Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
            }
        }

        protected void ddlTopAscOrDescPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTopSortByPlayer.SelectedIndex != 0)
            {
                SearchPlayerBets(1, ddlTopSortByPlayer.SelectedItem.Text, ((DropDownList)sender).SelectedValue, Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
            }
        }

        protected void lnkPlayerBetsRepeater_Click(object sender, EventArgs e)
        {
            int PageIndex = int.Parse((sender as LinkButton).CommandArgument);
            SearchPlayerBets(PageIndex, ddlTopSortByPlayer.SelectedItem.Text, ddlTopAscOrDescPlayer.SelectedValue, Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));

            Session["PlayerPageIndex"] = PageIndex;
        }

        protected void gvPlayerBets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AcceptBet")
            {
                if (Session["UserId"] != null)
                {

                    int BetId = Convert.ToInt32(e.CommandArgument);
                    int RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                    int UserIdWhoCreatedBet = Convert.ToInt32(((Label)gvPlayerBets.Rows[RowIndex].FindControl("UserCreatorId")).Text);

                    using (SqlContextDataContext con = new SqlContextDataContext())
                    {
                        tblLinkUsersToBet NewPlayerBet = new tblLinkUsersToBet
                        {
                            fkBetId = BetId,
                            blnIsTeamBet = false,
                            datDateTimeAccepted = DateTime.Now,
                            fkUserWhoAccepted = (int)Session["UserId"],
                            fkUserWhoCreatedId = UserIdWhoCreatedBet
                        };

                        con.tblLinkUsersToBets.InsertOnSubmit(NewPlayerBet);
                        con.SubmitChanges();

                        tblPlayerBet PlayerBet = con.tblPlayerBets.SingleOrDefault(x => x.pkBetId == BetId);
                        PlayerBet.blnBetIsOpened = false;
                        con.SubmitChanges();

                    }


                    ViewState["BetAccepted"] = true;
                    pnlBetAccepted.Visible = true;


                    gvPlayerBets.Rows[RowIndex].Cells[1].Controls.RemoveAt(0);
                    gvPlayerBets.Rows[RowIndex].Cells[1].Controls.Add(new LiteralControl() { Text = "Closed Bet", });

                    if (Session["PlayerPageIndex"] == null)
                    {
                        SearchPlayerBets(1, ddlTopSortByPlayer.SelectedItem.Text, ddlTopAscOrDescPlayer.SelectedValue, Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
                    }
                    else
                    {
                        SearchPlayerBets((int)Session["PlayerPageIndex"], "Player", "ASC", Convert.ToInt32(ddlPlayerPageSize.SelectedItem.Text));
                    }


                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                }
            }
        }

        protected void ddlTeamBetPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
        }

        protected void ddlTeamSortByTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
        }

        protected void ddlTeamAscOrDescTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
        }

        protected void gvTeamBets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AcceptBet")
            {
                if (Session["UserId"] != null)
                {

                    int BetId = Convert.ToInt32(e.CommandArgument);
                    int RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                    int UserIdWhoCreatedBet = Convert.ToInt32(((Label)gvTeamBets.Rows[RowIndex].FindControl("UserCreatorId")).Text);

                    using (SqlContextDataContext con = new SqlContextDataContext())
                    {
                        tblLinkUsersToBet NewTeamBet = new tblLinkUsersToBet
                        {
                            fkBetId = BetId,
                            blnIsTeamBet = true,
                            datDateTimeAccepted = DateTime.Now,
                            fkUserWhoAccepted = (int)Session["UserId"],
                            fkUserWhoCreatedId = UserIdWhoCreatedBet
                        };

                        con.tblLinkUsersToBets.InsertOnSubmit(NewTeamBet);
                        con.SubmitChanges();

                        tblTeamBet PlayerBet = con.tblTeamBets.Single(x => x.pkBetId == BetId);
                        PlayerBet.blnBetIsOpened = false;
                        con.SubmitChanges();

                    }


                    ViewState["BetAccepted"] = true;
                    pnlBetAccepted.Visible = true;


                    gvTeamBets.Rows[RowIndex].Cells[1].Controls.RemoveAt(0);
                    gvTeamBets.Rows[RowIndex].Cells[1].Controls.Add(new LiteralControl() { Text = "Closed Bet", });


                    if (Session["PageIndex"] == null)
                    {
                        SearchTeamBets(1, ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
                    }
                    else
                    {
                        SearchTeamBets((int)Session["PageIndex"], ddlTeamSortByTop.SelectedItem.Text, Convert.ToBoolean(ddlTeamAscOrDescTop.SelectedIndex), Convert.ToInt32(ddlTeamBetPageSize.SelectedItem.Text));
                    }
                }
                else
                {
                    pnlMustBeLoggedId.Visible = true;
                }
            }
        }





        private void SaveSearchCriteria()
        {

            HttpContext.Current.Response.Cookies.Remove("SaveSearchCriteria");

            HttpCookie cook = new HttpCookie("SaveSearchCriteria");
            cook.Expires = DateTime.Now.AddDays(30);
            cook["txtWagerStarts"] = txtWagerStart.Text;
            cook["txtWagerEnds"] = txtWagerEnd.Text;
            cook["Logic"] = ddlTeamLogic.SelectedItem.Text;
            cook["LogicArgument"] = txtTeamArgumentValue.Text;
            cook["IsOpenBet"] = chkIsOpened.Checked.ToString();

            NameValueCollection Teams = new NameValueCollection();
            NameValueCollection Stats = new NameValueCollection();

            foreach (ListItem li in lbTeams.Items)
            {
                if (li.Selected)
                {
                    Teams.Add("Team", li.Text);
                }
            }
            foreach (ListItem li in lbTeamStatArguments.Items)
            {
                if (li.Selected)
                {
                    Stats.Add("StatArgs", li.Text);
                }
            }

            cook.Values.Add(Stats);
            cook.Values.Add(Teams);

            HttpContext.Current.Response.SetCookie(cook);

        }

        private void SaveSearchCriteriaForPlayerBets()
        {


            HttpContext.Current.Response.Cookies.Remove("SavePlayerSearchCriteria");

            HttpCookie cook = new HttpCookie("SavePlayerSearchCriteria");
            cook.Expires = DateTime.Now.AddDays(30);

            cook["txtWagerStarts"] = txtPlayerWageStart.Text;
            cook["txtWagerEnds"] = txtPlayerWageEnd.Text;
            cook["Logic"] = ddlPlayerLogic.SelectedItem.Text;
            cook["LogicArgument"] = txtPlayerArgumentValue.Text;
            cook["IsOpenBet"] = chkPlayerOnlyOpened.Checked.ToString();
            cook["Team"] = ddlPlayersTeam.SelectedItem.Text;

            NameValueCollection Stats = new NameValueCollection();
            NameValueCollection Players = new NameValueCollection();

            foreach (ListItem li in lbPlayerStatArgument.Items)
            {
                if (li.Selected)
                {
                    Stats.Add("StatArgs", li.Text);
                }
            }
            foreach (ListItem li in lbPlayers.Items)
            {
                if (li.Selected)
                {
                    Stats.Add("Players", li.Text);
                }
            }

            cook.Values.Add(Players);
            cook.Values.Add(Stats);

            HttpContext.Current.Response.SetCookie(cook);

        }

        private void GetSearchCriteriaForTeamBets()
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["SaveSearchCriteria"];

            if (cookie != null)
            {

                txtWagerStart.Text = cookie["txtWagerStarts"];
                txtWagerEnd.Text = cookie["txtWagerEnds"];
                ddlTeamLogic.SelectedValue = cookie["Logic"];
                txtTeamArgumentValue.Text = cookie["LogicArgument"];
                chkIsOpened.Checked = Convert.ToBoolean(cookie["IsOpenBet"]);

                NameValueCollection nvc = cookie.Values;

                if (nvc.GetValues("Team") != null)
                {
                    foreach (string Team in nvc.GetValues("Team"))
                    {
                        foreach (ListItem li in lbTeams.Items)
                        {
                            if (li.Text == Team)
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
                if (nvc.GetValues("StatArgs") != null)
                {
                    foreach (string Stat in nvc.GetValues("StatArgs"))
                    {
                        foreach (ListItem li in lbTeamStatArguments.Items)
                        {
                            if (li.Text == Stat)
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }

                btnSearchTeamBets_Click(this, new EventArgs());

            }

        }

        private void IsTeamBet()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["TypeOfBet"];
            
            if (cookie != null)
            {
                int Index = int.Parse(cookie.Value);
                rdoTypeOfBet.SelectedIndex = Index;
                cookie.Expires = DateTime.Now.AddDays(30);

                if (Index == 0)
                {
                    GetSearchCriteriaForTeamBets();
                }
                else
                {
                    GetSearchCriteriaForPlayerBets();
                }
            }
        }

        private void GetSearchCriteriaForPlayerBets()
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["SavePlayerSearchCriteria"];

            if (cookie != null)
            {
                NameValueCollection nvc = cookie.Values;

                ddlPlayersTeam.SelectedValue = cookie["Team"];

                ddlPlayersTeam_SelectedIndexChanged(this, new EventArgs());

                if (nvc.GetValues("Players") != null)
                {
                    foreach (string Stat in nvc.GetValues("Players"))
                    {
                        foreach (ListItem li in lbPlayers.Items)
                        {
                            if (li.Text == Stat)
                            {
                                li.Selected = true;
                            }
                        }
                    }

                    lbPlayers_SelectedIndexChanged(lbPlayers, new EventArgs());
                }

                txtPlayerWageStart.Text = cookie["txtWagerStarts"];
                txtPlayerWageEnd.Text = cookie["txtWagerEnds"];
                ddlPlayerLogic.SelectedValue = cookie["Logic"];
                txtPlayerArgumentValue.Text = cookie["LogicArgument"];
                chkPlayerOnlyOpened.Checked = Convert.ToBoolean(cookie["IsOpenBet"]);



                if (nvc.GetValues("StatArgs") != null)
                {
                    foreach (string Team in nvc.GetValues("StatArgs"))
                    {
                        foreach (ListItem li in lbPlayerStatArgument.Items)
                        {
                            if (li.Text == Team)
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }

                btnSearchPlayerBets_Click(this, new EventArgs());

            }

        }

        protected void lnkClearSelections_Click(object sender, EventArgs e)
        {
            ClearControls(this.Page);

        }

        private void ClearControls(Control ParentControl)
        {
            if (ParentControl.HasControls())
            {
                foreach (Control Ctrl in ParentControl.Controls)
                {
                    if (Ctrl is ListBox)
                    {
                        ((ListBox)Ctrl).ClearSelection();

                        lbPlayers.Items.Clear();
                        lbPlayerStatArgument.Items.Clear();

                    }
                    else if (Ctrl is DropDownList)
                    {
                        ((DropDownList)Ctrl).SelectedIndex = 0;
                    }
                    else if (Ctrl is TextBox)
                    {
                        ((TextBox)Ctrl).Text = string.Empty;
                    }
                    else if (Ctrl is CheckBox)
                    {
                        ((CheckBox)Ctrl).Checked = true;
                    }

                    ClearControls(Ctrl);
                }
            }
        }


    }
}