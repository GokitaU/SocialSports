using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialSports
{
    public class PlayerBets
    {

        public static IEnumerable<object> GetPopularPlayers()
        {
            SqlContextDataContext con = new SqlContextDataContext();

            IEnumerable<object> PopularPlayerBets = from b in con.tblPlayerBets
                                               join
                                                   t in con.tblNflPlayers on b.strPlayerName equals t.strPlayerName
                                               group b by new
                                               {
                                                   PlayerName = t.strPlayerName,
                                                   PlayerImage = t.strPlayerImage,
                                                   PlayerTeam = t.fkPlayerTeam

                                               } into eGroup
                                               orderby eGroup.Key.PlayerName.Count() descending
                                               select new
                                               {

                                                   PlayerCount = eGroup.Key.PlayerName.Count(),
                                                   Player = HttpContext.Current.Server.UrlEncode(eGroup.Key.PlayerName),
                                                   PlayerName = eGroup.Key.PlayerName,
                                                   PlayerImage = eGroup.Key.PlayerImage,
                                                   PlayerTeam = eGroup.Key.PlayerTeam
                                               };

            return PopularPlayerBets.Take(20);

        }
    }
}