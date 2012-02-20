using System;
using System.Diagnostics;
using System.Web;


namespace CavemanTools.Web
{
    public class RequestMetricsModule:IHttpModule
    {
        public const string StatsKey = "_req-duration";
        public const string MvcActionDuration = "_req-mvcaction-duration";
        public const string MvcResultDuration = "_req-mvcresult-duration";
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (o, args) =>
                                                    {
                                                       HandleDuration(StatsKey);
                                                    };
        
            context.EndRequest += (o, args) =>
                                                 {
                                                     HandleDuration(StatsKey); 
                                                     var ctx = HttpContext.Current;
                                                     if (!CanAppendStats(ctx)) return;
                                                   
                                                     HandleTimer(StatsKey,FormatRequestDuration);
                                                     HandleTimer(MvcActionDuration,FormatActionDuration);
                                                     HandleTimer(MvcResultDuration,FormatResultDuration);
                                                     
                                                     
                                                     ctx.Response.Write(string.Format("Approximate memory usage as returned by GC.GetTotalMemory: {0} MB", GC.GetTotalMemory(false)/1024/1024));
                                                 };
        }

        static bool CanAppendStats(HttpContext ctx)
        {
            var ctype = ctx.Response.Headers["Content-Type"];
            if (ctype == null || !ctype.Contains("text/html")) return false;
            return true;
        }


        static void HandleTimer(string key,Func<TimeSpan,string> act)
        {
            var ctx = HttpContext.Current;
            if (ctx.Items.Contains(key))
            {
                var obj = ctx.Items[key];
                TimeSpan? t=null;
                if (obj is TimeSpan)
                {
                    t = (TimeSpan) obj;
                }
                if (obj is Stopwatch)
                {
                    var s = obj as Stopwatch;
                    s.Stop();
                    t = s.Elapsed;
                }
                if (t.HasValue)
                {
                    ctx.Response.Write(act(t.Value));
                }
            }
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
                var f = (Stopwatch) ctx.Items[key];
                f.Stop();
                ctx.Items[key] = f.Elapsed;
                
            }
            else
            {
                var s = new Stopwatch();
                s.Start();
                ctx.Items[key] = s;
            }
        }
       
        public void Dispose()
        {
            
        }
    }
}