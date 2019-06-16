using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialSports
{
    public class TeamBets
    {
        public static IEnumerable<object> MostPopularTeams()
        {

            SqlContextDataContext con = new SqlContextDataContext();

            IEnumerable<object> PopularTeams = from b in con.tblTeamBets
                                               join
                                                   t in con.tblNflTeams on b.strTeam equals t.strTeamName
                                               group b by new
                                               {
                                                   TeamName = t.strTeamName,
                                                   TeamImage = t.strTeamImage

                                               } into eGroup
                                               orderby eGroup.Key.TeamName.Count() descending
                                               select new
                                               {

                                                   TeamCount = eGroup.Key.TeamName.Count(),
                                                   Team = HttpContext.Current.Server.UrlEncode(eGroup.Key.TeamName),
                                                   TeamImage = eGroup.Key.TeamImage,
                                                   TeamName = eGroup.Key.TeamName
                                               };

            return PopularTeams.Take(20);


        }
    }
}