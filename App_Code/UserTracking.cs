using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SocialSports
{
    public class UserTracking
    {
        public class MemberTracking
        {
       
            public void InsertPageView(int UserId, string BaseUrl, int QueryStringValue, bool IsTeamBet)
            {
                using (SqlContextDataContext c = new SqlContextDataContext())
                {
                    tblStatsPageVisit NewPageVisit = new tblStatsPageVisit
                    {
                        fkUserId = UserId,
                        strBaseUrlVisited = BaseUrl,
                        strQueryStringValue = QueryStringValue,
                        datDateTimeVisited = DateTime.Now,
                        blnIsTeamBet = IsTeamBet
                    };

                    c.tblStatsPageVisits.InsertOnSubmit(NewPageVisit);
                    c.SubmitChanges();              
                }
            }
        
            public void InsertSearchQuery(int UserId, bool IsPlayerBet, Guid guid, string EntityName, string StatArgument, string LogicArgument, decimal? ValueArgument)
            {
                using (SqlContextDataContext con = new SqlContextDataContext())
                {                  
                    tblStatsBetSearchQuery NewBetSearch = new tblStatsBetSearchQuery
                    {
                        fkUserId = UserId, 
                        IsPlayerBetSearch = IsPlayerBet,
                        strEntityName = EntityName,
                        strStatArgument = StatArgument,
                        strLogicArgument = LogicArgument,
                        strValueArgument = ValueArgument,
                        datDateTimeCreated = DateTime.Now,
                        guidQuery = guid

                    };

                    con.tblStatsBetSearchQueries.InsertOnSubmit(NewBetSearch);
                    con.SubmitChanges();
                }
            }

         
        }

        public class NonMemberTracking
        {

            public Dictionary<string, int> RequestCookies(HttpCookie Cookie)
            {

                Dictionary<string, int> Dictionary = new Dictionary<string, int>();
                List<string> TeamsViewed = new List<string>();

                var resultNfl = from country in TeamsViewed
                                group country by country into eGroup
                                select new
                                {
                                    Group = eGroup.Key,
                                    Count = eGroup.Count()
                                };

                foreach (var result in resultNfl)
                {
                    Dictionary.Add(result.Group, result.Count);
                }

                return Dictionary;
            }
        }

        public int PageViews(string RawUrl)
        {
            SqlContextDataContext con = new SqlContextDataContext();

            int count = con.tblStatsPageVisits.Where(w => w.strBaseUrlVisited == RawUrl).Count();

            return count;

  
        }


    }
}