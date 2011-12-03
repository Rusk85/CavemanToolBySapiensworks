using System;
using CavemanTools.Extensions;

namespace CavemanTools.Web.Security
{
    public static class AuthenticationUtils
    {
         public static AuthenticationTicketData Unpack(this string ticket)
         {
             if (ticket == null) throw new ArgumentNullException("ticket");
             AuthenticationTicketData res=null;
             if (!string.IsNullOrEmpty(ticket))
             {
                 var items = ticket.Split(';');
                 try
                 {
                     res = new AuthenticationTicketData();
                     res.Id = items[0].ConvertTo<int>();
                     res.GroupId = items[1].ConvertTo<int>();
                 }
                 catch(IndexOutOfRangeException)
                 {
                     //nothing, invalid ticket
                     res = null;
                 }
             }
             return res;

         }

        public static string Pack(this AuthenticationTicketData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return string.Format("{0};{1}",data.Id,data.GroupId);
        }
    }

    public class AuthenticationTicketData
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
    }


}