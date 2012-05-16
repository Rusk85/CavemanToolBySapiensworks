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
}