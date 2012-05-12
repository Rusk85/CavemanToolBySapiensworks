using System.Collections.Generic;

namespace CavemanTools.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonMessageStruct
    {
        public string Message { get; set; }
        public string Status { get; set; }

        public JsonMessageStruct(JsonStatus status=JsonStatus.Ok,string message="")
        {
            Message = message;
            Status = status.ToString();
        }
    }

    public class JsonStruct
    {
        public string Status { get; set; }
        public IDictionary<string, object> Data { get; private set; }
        public JsonStruct(JsonStatus status=JsonStatus.Ok)
        {
            Status = status.ToString();
            Data= new Dictionary<string, object>();
        }
    } 
}