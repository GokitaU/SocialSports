using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialSports.UserControls
{
    public partial class ctrlWeeklyTopWinners : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlContextDataContext con = new SqlContextDataContext())
            {
                var r = from t in con.tblBetHistories
                        group t by
                            new
                            {
                                t.fkUserIdWinner
                            } into eGroup
                        select eGroup.Key;
                    

                lvWeeklyWinners.DataSource = r;
                this.Page.DataBind();


            }
                       
        }




    }

    public class TopWeeklyWinnersArgs : EventArgs
    {
        private decimal _WinPerc;
        private int _TotalWins;
        private int _TotalLosses;
        private int _WeekNumber;

        public TopWeeklyWinnersArgs(decimal WinPerc, int TotalWins, int TotalLosses, int WeekNumber)
        {
            this._WinPerc = WinPerc;
            this._WeekNumber = WeekNumber;
            this._TotalWins = TotalLosses;
            this._TotalLosses = TotalLosses;
        }

        public decimal winPerc
        {
            get { return _WinPerc; }
        }
        public int TotalWins
        {
            get { return _TotalWins; }
        }
        public int TotalLosses
        {
            get { return _TotalLosses; }
        }
        public int WeekNumber
        {
            get { return _WeekNumber; }
        }
    }

    public delegate void OnTopWeekly(object sender, TopWeeklyWinnersArgs e);
}