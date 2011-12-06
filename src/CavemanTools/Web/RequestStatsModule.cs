using System;
using System.Web;

namespace CavemanTools.Web
{
    public class RequestStatsModule:IHttpModule
    {
        public const string StatsKey = "_req-duration";
        public const string MvcActionDuration = "_req-mvcaction-duration";
        public const string MvcResultDuration = "_req-mvcresult-duration";
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += (o, args) =>
                                                    {
                                                       HandleDuration(StatsKey);
                                                    };
            context.PostRequestHandlerExecute += (o, args) =>
                                                     {
                                                       HandleDuration(StatsKey);  
                                                     };
            context.PreSendRequestContent += (o, args) =>
                                                 {
                                                     var ctx = HttpContext.Current;
                                                     if (ctx.Items.Contains(StatsKey))
                                                     {
                                                         ctx.Response.Write(FormatRequestDuration((TimeSpan)ctx.Items[StatsKey]));
                                                     }

                                                     if(ctx.Items.Contains(MvcActionDuration))
                                                     {
                                                         ctx.Response.Write(FormatActionDuration((TimeSpan)ctx.Items[MvcActionDuration]));
                                                     }

                                                     if(ctx.Items.Contains(MvcResultDuration))
                                                     {
                                                         ctx.Response.Write(FormatResultDuration((TimeSpan)ctx.Items[MvcResultDuration]));
                                                     }

                                                     ctx.Response.Write(string.Format("Approximate memory usage as returned by GC.GetTotalMemory: {0} MB ", GC.GetTotalMemory(false)/1024/1024));
                                                 };
        }

        string FormatRequestDuration(TimeSpan ts)
        {
            return string.Format("Request handler executed in {0} ms <br>", ts.TotalMilliseconds);
        }


        string FormatActionDuration(TimeSpan ts)
        {
            return string.Format("Mvc action executed in {0} ms <br>", ts.TotalMilliseconds);
        }

        string FormatResultDuration(TimeSpan ts)
        {
            return string.Format("Mvc result executed in {0} ms <br>", ts.TotalMilliseconds);
        }



        public static void HandleDuration(string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            var ctx = HttpContext.Current; 
            if (ctx.Items.Contains(key))
            {
                var f = (DateTime) ctx.Items[key];
                ctx.Items[key] = DateTime.Now.Subtract(f);
            }
            else
            {
                ctx.Items[key] = DateTime.Now;
            }
        }
       
        public void Dispose()
        {
            
        }
    }
}