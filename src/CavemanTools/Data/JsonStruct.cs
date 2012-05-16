using System.Collections.Generic;

namespace CavemanTools.Data
{
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