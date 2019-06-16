using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialSports
{
    public partial class Bets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    pnlMustBeLoggedId.Visible = true;
                    rdoTypeOfBet.Enabled = false;
                    ddlWeekNumber.Enabled = false;
                }
                else
                {
              
                    pnlMustBeLoggedId.Visible = false;

                }

                pnlTeamBet.Visible = false;
                pnlPlayerBet.Visible = false;
                pnlTeamBet.Visible = false;
                pnlBetCreationIsSuccessful.Visible = false;
                pnlBetCreationIsSuccessful.Visible = false;

            }

        }

        protected void ddlWeekNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

            pnlPlayerBet.Visible = false;
            pnlTeamBet.Visible = false;


            int WeekNumber = (sender as DropDownList).SelectedIndex;

            SqlContextDataContext cont = new SqlContextDataContext();

            using (cont)
            {
                var results = cont.tblNflSchedules.Where(x => x.intWeekNumber == WeekNumber)
                    .Select(z => new { HomeTeam = z.fkHomeTeam, VisitorTeam = z.fkVisitorTeam, NflGameId = z.NflGameId, WeekNumber = z.intWeekNumber });

                gvGamesForSelectedWeek.DataSource = results;
                gvGamesForSelectedWeek.DataBind();
            }     
        }

        private void LoadLogic(DropDownList ddl)
        {
             using (SqlContextDataContext context = new SqlContextDataContext())
                {
                    Dictionary<int, string> StatTypes = context.tblNflStatisticalCategoryTypes.ToDictionary(x => x.pkStatisticalCategoryTypeId, x => x.strStatisticalCategoryTypeName);

                    foreach (KeyValuePair<int, string> kvp in StatTypes)
                    {
                        ListItem li = new ListItem()
                        {
                            Text = kvp.Value,
                            Value = kvp.Key.ToString()
                        };

                        ddl.Items.Add(li);
                    }                 
                }
        }


       

        protected void gvGamesForSelectedWeek_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {


                case "MakeBet":

                    if (rdoTypeOfBet.SelectedIndex == 1)
                    {
                        pnlPlayerBet.Visible = true;
                        pnlTeamBet.Visible = false;
                    }
                    else
                    {
                        pnlPlayerBet.Visible = false;
                        pnlTeamBet.Visible = true;
                        ddlTeamArguments.Items.Clear();
                    }

                    int GameId = Convert.ToInt32(e.CommandArgument);

                    if (rdoTypeOfBet.SelectedIndex == 0)
                    {
                        if (ddlTeams.Items.Count == 0)
                        {
                            using (SqlContextDataContext con = new SqlContextDataContext())
                            {
                                var source = con.tblNflSchedules
                                             .Where(x => x.NflGameId == GameId)
                                             .Select(z => new { HomeTeam = z.fkHomeTeam, VisitorTeam = z.fkVisitorTeam });

                                foreach (var s in source)
                                {
                                    ddlTeams.Items.Add(s.HomeTeam);
                                    ddlTeams.Items.Add(s.VisitorTeam);
                                }
                            }
                        }

                        LoadLogic(ddlTeamArguments);
                        pnlTeamBet.Visible = true;
                    }
                    else
                    {
                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                                var psource = con.tblNflPlayers.Join(con.tblNflSchedules, 
                                x => x.fkPlayerTeam, y => y.fkHomeTeam, (Players, Team)
                                => new { pyrs = Players.strPlayerName, pyrsPos = Players.strPlayerPosition });

                                if (ddlPlayers.Items.Count == 0)
                                {

                                    ddlPlayers.Items.Insert(0, "Select Player");

                                    foreach (var s in psource)
                                    {
                                        ListItem li = new ListItem()
                                        {
                                            Text = s.pyrs,
                                            Value = s.pyrsPos.Trim()
                                        };


                                        ddlPlayers.Items.Add(li);
                                    }
                                }


                        }
                    }

                    break;
            }
        }



        protected void rdoTypeOfBet_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                pnlPlayerBet.Visible = false;
                pnlTeamBet.Visible = false;
                    
        }

      


        protected void ddlPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlPlayers.SelectedIndex != 0)
            {
                SqlContextDataContext con = new SqlContextDataContext();

                ddlPlayerArguments.Items.Clear();

                foreach (ListItem li in (sender as DropDownList).Items)
                {
                    if (li.Selected)
                    {

                        var results = con.tblNflStatisticalCategoryTypesOffensivePlayers.Where
                            (x => x.strOffensivePosition == li.Value.ToUpper()).Select(
                            y => new { y.strStatisticalCategoryTypeName });

                        foreach (var r in results)
                        {
                            ddlPlayerArguments.Items.Add(r.strStatisticalCategoryTypeName);
                        }

                    }

                }
            }
        }

        protected void btnCreatePlayerBet_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (rdoTypeOfBet.SelectedIndex == 1)
                {
                    this.Page.Validate("PlayerBet");

                    if (this.Page.IsValid)
                    {
                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                            tblPlayerBet NewBet = new tblPlayerBet
                            {
                                fkCreatorUserId = (int)Session["UserId"],
                                intWeekNumber = ddlWeekNumber.SelectedIndex,
                                strPlayerName = ddlPlayers.SelectedItem.Text,
                                fkStatType = ddlPlayerArguments.SelectedItem.Text,
                                strLogicArgument = ddlPlayersLogic.SelectedItem.Text,
                                decArgumentValue = Convert.ToDecimal(txtPlayerArgumentValue.Text),
                                decMoneyWager = Convert.ToDecimal(txtPlayerWagerAmount.Text.Trim()),
                                datDateTimeCreated = DateTime.Now,
                                blnBetIsOpened = true
                            };

                            con.tblPlayerBets.InsertOnSubmit(NewBet);
                            con.SubmitChanges();
                        }

                        pnlBetCreationIsSuccessful.Visible = true;

                    }
                }
            }
        }

        protected void btnCreateTeamBet_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (rdoTypeOfBet.SelectedIndex == 0)
                {
                    this.Page.Validate("TeamBet");

                    if (this.Page.IsValid)
                    {
                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                            tblTeamBet NewBet = new tblTeamBet
                            {
                                
                                fkCreatorUserId = (int)Session["UserId"],
                                intWeekNumber = ddlWeekNumber.SelectedIndex,
                                strTeam = ddlTeams.SelectedItem.Text,
                                fkStatType = ddlTeamArguments.SelectedItem.Text,
                                strLogicArgument = ddlTeamLogic.SelectedItem.Text,
                                decArgumentValue = Convert.ToDecimal(txtTeamArgumentValue.Text),
                                datDateTimeCreated = DateTime.Now,
                                decMoneyWager = Convert.ToDecimal(txtTeamWagerAmount.Text.Trim()),
                                blnBetIsOpened = true
                            };

                            con.tblTeamBets.InsertOnSubmit(NewBet);
                            con.SubmitChanges();
                        }

                        
                        pnlBetCreationIsSuccessful.Visible = true;
                    }
                }
            }
        }

      
     

       
    
    }
}