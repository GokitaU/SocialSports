using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SocialSports
{
    public class UserInfo
    {
        public int UserId;
        SqlContextDataContext context = new SqlContextDataContext();

        public void PopulateControlWithUserTeamBets(Control[] Ctrls)
        {
            IEnumerable<tblTeamBet> UserCreatedBets = context.tblTeamBets.AsEnumerable()
                           .Where(b => b.fkCreatorUserId == UserId);
                          // .OrderByDescending(o => o.decArgumentValue).Select(n => new {n});

            var UserSignedUpBets = from b in context.tblTeamBets
                                   join
                                       l in context.tblLinkUsersToBets on
                                       b.pkBetId equals l.fkBetId
                                   where l.fkUserWhoAccepted == UserId orderby b.decArgumentValue descending
                                   select b;


            for (int i = 0; i < 2; i++)
            {
                if (Ctrls[0] is GridView && Ctrls[0].GetType() == typeof(GridView))
                {
                    GridView gv = (GridView)Ctrls[0];

                    gv.DataSource = UserCreatedBets;
                    gv.DataBind();
                }
                
               if(Ctrls[1] is GridView && Ctrls[1].GetType() == typeof(GridView))
                {
                    GridView gv = (GridView)Ctrls[1];

                    gv.DataSource = UserSignedUpBets;
                    gv.DataBind();
                }
            }
        }

        public static void InsertDefaultAvatar(int UserId)
        {
            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("Icons\\default-avatar.jpg"), FileMode.Open);

            using (fs)
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    Byte[] bytes = binaryReader.ReadBytes((int)fs.Length);

                    try
                    {
                        using (SqlContextDataContext con = new SqlContextDataContext())
                        {
                            tblUser user = con.tblUsers.First(x => x.pkUserId == UserId);
                            user.bytUserImage = bytes;
                            con.SubmitChanges();
                        }
                   
                    }
                    catch (Exception err)
                    {
                        throw new ApplicationException(err.Message);
                    }               
                }
            }
        }
    

        
    }
}