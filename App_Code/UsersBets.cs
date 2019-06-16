using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;


namespace SocialSports
{
    public class UsersBets
    {
        
        public static void UserAgreeToBet(int BetId, int UserIdWhoCreated, int UserIdWhoAccepted, bool IsTeamBet)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                tblLinkUsersToBet NewBetAcceptor = new tblLinkUsersToBet
                {
                    fkUserWhoCreatedId = UserIdWhoCreated,
                    fkUserWhoAccepted = UserIdWhoAccepted,
                    fkBetId = BetId,
                    datDateTimeAccepted = DateTime.Now,
                    blnIsTeamBet = IsTeamBet
                };

                con.tblLinkUsersToBets.InsertOnSubmit(NewBetAcceptor);
                con.SubmitChanges();

                if (IsTeamBet)
                {
                    tblTeamBet TeamBet = con.tblTeamBets.Single(x => x.pkBetId == BetId);
                    TeamBet.blnBetIsOpened = false;
                    con.SubmitChanges();
                }
                else
                {
                    tblPlayerBet PlayerBet = con.tblPlayerBets.Single(x => x.pkBetId == BetId);
                    PlayerBet.blnBetIsOpened = false;
                    con.SubmitChanges();

                }

            }

        }

        public static bool UserBetApprovals(int BetId, int UserId)
        {
            bool IsAlreadyApproved = false;

            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                int ApprovalCount = con.tblBetApprovals.Where(x => x.fkUserIdWhoApproved == UserId).Count();

                if (ApprovalCount == 0)
                {
                    tblBetApproval NewBetApproval = new tblBetApproval()
                    {
                        fkBetId = BetId,
                        fkUserIdWhoApproved = UserId
                    };

                    con.tblBetApprovals.InsertOnSubmit(NewBetApproval);
                    con.SubmitChanges();

                }
                else
                {
                    IsAlreadyApproved = true;
                }
            }

            return IsAlreadyApproved;
        }

        public static void LoadTeams(WebControl Control)
        {
            SqlContextDataContext c = new SqlContextDataContext();

            var q = c.tblNflTeams.Select(x => new { TeamId = x.pkTeamId, TeamName = x.strTeamName }).OrderBy(x => x.TeamName);
         
            foreach (var team in q)
            {
                ListItem li = new ListItem();
                li.Text = team.TeamName;
                li.Value = team.TeamName;

                if (Control is DropDownList && Control.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)Control).Items.Add(li);
                }
                else if (Control is ListBox && Control.GetType() == typeof(ListBox))
                {
                    ((ListBox)Control).Items.Add(li);

                }

            }
        }

        public static bool IsBetOpen(int BetId, bool IsTeamBet)
        {
            bool IsBetOpen = false;

            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                if (IsTeamBet)
                {

                    var IsOpen = con.tblTeamBets.Where(x => x.pkBetId == BetId)
                        .Select(y => y.blnBetIsOpened);

                    foreach (var r in IsOpen)
                    {
                        IsBetOpen = Convert.ToBoolean(r);
                    }
                    
                }
                else
                {
                    var IsOpen = con.tblPlayerBets.Where(x => x.pkBetId == BetId)
                       .Select(y => y.blnBetIsOpened);

                    foreach (var r in IsOpen)
                    {
                        IsBetOpen = Convert.ToBoolean(r);
                    }
                }

            }

            return IsBetOpen;
         
        }

        //finish

        public static void MakeCopyOBet(int UserId, int BetId, bool IsTeamBet, bool IsOpposite)
        {
            using (SqlContextDataContext context = new SqlContextDataContext())
            {
                if (IsTeamBet)
                {
                    var Results = context.tblTeamBets.Where(y => y.pkBetId == BetId)
                        .Select(g => new { TeamBets = g });

                    tblTeamBet NewTeamBet = new tblTeamBet();
                  
                    foreach (var Result in Results)
                    {
                        NewTeamBet.strTeam = Result.TeamBets.strTeam;
                        NewTeamBet.pkBetId = Result.TeamBets.pkBetId;
                        NewTeamBet.fkStatType = Result.TeamBets.fkStatType;
                        NewTeamBet.datDateTimeCreated = DateTime.Now;
                        NewTeamBet.decArgumentValue = Result.TeamBets.decArgumentValue;
                        NewTeamBet.intWeekNumber = Result.TeamBets.intWeekNumber;
                        NewTeamBet.fkCreatorUserId = UserId;

                        if (IsOpposite)
                        {
                            if (Result.TeamBets.strLogicArgument.Trim() == "Will Have More Than")
                            {
                                NewTeamBet.strLogicArgument = "Will Have Less Than";
                            }
                            else
                            {
                                NewTeamBet.strLogicArgument = "Will Have More Than";
                            }
                        }
                        else
                        {
                            NewTeamBet.strLogicArgument = Result.TeamBets.strLogicArgument;
                        }

                        NewTeamBet.decMoneyWager = Result.TeamBets.decMoneyWager;

                    }

                    context.tblTeamBets.InsertOnSubmit(NewTeamBet);
                    context.SubmitChanges();
                }
                else
                {
                    var Results = context.tblPlayerBets.Where(y => y.pkBetId == BetId)
                       .Select(g => new { PlayerBets = g });

                    tblPlayerBet NewPlayerBet = new tblPlayerBet();

                    foreach (var Result in Results)
                    {
                        NewPlayerBet.strPlayerName = Result.PlayerBets.strPlayerName;
                        NewPlayerBet.pkBetId = Result.PlayerBets.pkBetId;
                        NewPlayerBet.fkStatType = Result.PlayerBets.fkStatType;
                        NewPlayerBet.datDateTimeCreated = DateTime.Now;
                        NewPlayerBet.decArgumentValue = Result.PlayerBets.decArgumentValue;
                        NewPlayerBet.intWeekNumber = Result.PlayerBets.intWeekNumber;
                        NewPlayerBet.fkCreatorUserId = UserId;
                        NewPlayerBet.decMoneyWager = Result.PlayerBets.decMoneyWager;

                        if (IsOpposite)
                        {
                            if (Result.PlayerBets.strLogicArgument.Trim() == "Will Have More Than")
                            {
                                NewPlayerBet.strLogicArgument = "Will Have Less Than";
                            }
                            else
                            {
                                NewPlayerBet.strLogicArgument = "Will Have More Than";
                            }
                        }
                        else
                        {
                            NewPlayerBet.strLogicArgument = Result.PlayerBets.strLogicArgument;
                        }

                    }

                    context.tblPlayerBets.InsertOnSubmit(NewPlayerBet);
                    context.SubmitChanges();


                }
            }
        }

       public static void GetBetWinPercentages(int UserId, GridView Gv)
       {
           using(SqlContextDataContext con = new SqlContextDataContext())
           {
               var History = from t in con.tblBetHistories                            
                             where t.fkUserIdLoser == UserId ||
                             t.fkUserIdWinner == UserId group t by t.fkUserIdWinner into eGroup 
                             select new 
                             {
                                  WinPercentage = eGroup.Count(x => x.fkUserIdWinner == UserId) / (eGroup.Count(y => y.fkUserIdWinner == UserId) + eGroup.Count(t => t.fkUserIdLoser == UserId)),
                                  TotalWins = eGroup.Count(x => x.fkUserIdWinner == UserId),
                                  TotalLosses = eGroup.Count(y => y.fkUserIdLoser == UserId),
                                  BetId = eGroup.Select(x => x.fkBetId).Single()
                                 
                             };
                              
           
                                                        
           

                Gv.DataSource = History;
                Gv.DataBind();
           }

       }

       public static IEnumerable<tblTeamBet> GetTeamBetForWinOrLost(int BetId)
       {
           SqlContextDataContext con = new SqlContextDataContext();

           IEnumerable<tblTeamBet> TeamBet = con.tblTeamBets.AsEnumerable().Where(b => b.pkBetId == BetId);
           con.Dispose();

           return TeamBet;
               

       }

    


        public static IEnumerable<tblTeamBet> GetTeamBetInfo(int BetId)
        {

            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                

              /*  var BetInfo = con.tblTeamBets.Where(w => w.pkBetId == BetId).Select(bet => new
                {
                    BetId = bet.pkBetId,
                    UserWhoCreated = bet.fkCreatorUserId,
                    Team = bet.strTeam,
                    StatType = bet.fkStatType,
                    Logic = bet.strLogicArgument,
                    ArgumentValue = bet.decArgumentValue,
                    Date = bet.datDateTimeCreated
                });
                */

                IEnumerable<tblTeamBet> TeamBets = from t in con.tblTeamBets
                                                   where t.pkBetId == BetId
                                                   select t;

                return TeamBets;
            }
        }  

        
    }
}