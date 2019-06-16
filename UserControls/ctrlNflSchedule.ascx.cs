using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialSports
{
    public partial class ctrlNflSchedule : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                LoadNflScedule();
            }
        }



        private event delNflSchedule OnNflSchedule;

        protected virtual void OnNflScheduleAction(object sender, NflScheduleArgs e)
        {
            if (OnNflSchedule != null)
            {
                OnNflSchedule(this, e);
            }
        }

        private void LoadNflScedule()
        {
            using (SqlContextDataContext context = new SqlContextDataContext())
            {
                var schedule = from x in context.tblNflSchedules
                               join tt in context.tblNflTeams on x.fkHomeTeam equals tt.strTeamName
                               join t in context.tblNflTeams on x.fkVisitorTeam equals t.strTeamName
                               select new
                               {
                                   GameId = x.NflGameId,
                                   HomeTeam = x.fkHomeTeam,
                                   VisitorTeam = x.fkVisitorTeam,
                                   DateOfGame = x.datDateOfGame,
                                   WeekNumber = x.intWeekNumber,
                                   IsBuy = x.blnIsBuy,
                                   HomeImage = tt.strTeamImage,
                                   VisitorImage = t.strTeamImage
                               };

                gvNflSchedule.DataSource = schedule;
                gvNflSchedule.DataBind();
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {


            SelectedDatesCollection DateTimes = Calendar1.SelectedDates;



            using (SqlContextDataContext context = new SqlContextDataContext())
            {

                if (DateTimes.Count > 1)
                {

                    var schedule = from x in context.tblNflSchedules
                                   join tt in context.tblNflTeams on x.fkHomeTeam equals tt.strTeamName
                                   join t in context.tblNflTeams on x.fkVisitorTeam equals t.strTeamName
                                   where x.datDateOfGame >= DateTimes[0] && x.datDateOfGame <= DateTimes[6]

                                   select new
                                   {
                                       GameId = x.NflGameId,
                                       HomeTeam = x.fkHomeTeam,
                                       VisitorTeam = x.fkVisitorTeam,
                                       DateOfGame = x.datDateOfGame,
                                       WeekNumber = x.intWeekNumber,
                                       IsBuy = x.blnIsBuy,
                                       HomeImage = tt.strTeamImage,
                                       VisitorImage = t.strTeamImage,
                                       Date = x.datDateOfGame
                                   };

                    gvNflSchedule.DataSource = schedule;
                    gvNflSchedule.DataBind();

                }
                else
                {

                    var Schedule = from x in context.tblNflSchedules
                                   join tt in context.tblNflTeams on x.fkHomeTeam equals tt.strTeamName
                                   join t in context.tblNflTeams on x.fkVisitorTeam equals t.strTeamName
                                   where x.datDateOfGame == DateTimes[0] 

                                   select new
                                   {
                                       GameId = x.NflGameId,
                                       HomeTeam = x.fkHomeTeam,
                                       VisitorTeam = x.fkVisitorTeam,
                                       DateOfGame = x.datDateOfGame,
                                       WeekNumber = x.intWeekNumber,
                                       IsBuy = x.blnIsBuy,
                                       HomeImage = tt.strTeamImage,
                                       VisitorImage = t.strTeamImage,
                                       Date = x.datDateOfGame
                                   };

                    gvNflSchedule.DataSource = Schedule;
                    gvNflSchedule.DataBind();

                }
            }
        }

        private void GetPlayers(string[] TeamNames)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {

                var Players = from x in con.tblNflPlayers
                              where TeamNames.Contains(x.fkPlayerTeam)
                              select new
                              {
                                  PlayerName = x.strPlayerName,
                                  PlayerImage = x.strPlayerImage,
                                  PlayerPosition = x.strPlayerPosition
                              };

                gvPlayers.DataSource = Players;
                gvPlayers.DataBind();
            }
        }

        public class NflScheduleArgs : EventArgs
        {
            private int _NflGameId;
            private string _HomeTeam;
            private string _VisitorTeam;
            private DateTime _DateOfGame;
            private int _WeekNumber;
            private bool _IsBuy;

            public NflScheduleArgs(int NflGameId, string HomeTeam, string VisitorTeam, DateTime DateOfGame, int WeekNumber, bool IsBuy)
            {
                this._NflGameId = NflGameId;
                this._HomeTeam = HomeTeam;
                this._VisitorTeam = VisitorTeam;
                this._DateOfGame = DateOfGame;
                this._WeekNumber = WeekNumber;
                this._IsBuy = IsBuy;
            }

            public int nflGameId
            {
                get { return this._NflGameId; }
            }
            public string homeTeam
            {
                get { return this._HomeTeam; }
            }
            public string visitorTeam
            {
                get { return this._VisitorTeam; }
            }
            public DateTime dateOfGame
            {
                get { return this._DateOfGame; }
            }
            public int weekNumber
            {
                get { return this._WeekNumber; }
            }
            public bool isBuy
            {
                get { return this._IsBuy; }
            }
        }

        public delegate void delNflSchedule(object sender, NflScheduleArgs e);

        protected void gvNflSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
          
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells.Clear();

                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = 5;
                tableCell.Text = "Showing Games For Week <b>" + Calendar1.SelectedDate.ToShortDateString() + "</b>";
                tableCell.HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells.Add(tableCell);

            }

        }

        protected void gvNflSchedule_DataBound(object sender, EventArgs e)
        {
          
        }

        protected void gvNflSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            string[] PlayerTeams = e.CommandArgument.ToString().Split('-');

            GetPlayers(PlayerTeams);

        }
    }
}