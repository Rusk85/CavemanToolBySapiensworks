using System.Collections.Generic;

namespace CavemanTools.Data
{
    public class JsonStruct
    {
        public string Status { get; set; }
        public Dictionary<string, object> Data { get; private set; }
        public string Message { get; set; }

        public JsonStruct(string message,JsonStatus status = JsonStatus.Ok):this(status)
        {
            Message = message;
            Data["Message"] = message;
        }
        public JsonStruct(JsonStatus status=JsonStatus.Ok)
        {
            Status = status.ToString();
            Data= new Dictionary<string, object>();
        }
    }

    //public class JsonMessageStruct
    //{
    //    public string Message { get; set; }
    //    public string Status { get; set; }

    //    public JsonMessageStruct(JsonStatus status = JsonStatus.Ok, string message = "")
    //    {
    //        Message = message;
    //        Status = status.ToString();
    //    }
    //}
}