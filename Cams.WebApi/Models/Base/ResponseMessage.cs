using System.Collections.Generic;

namespace XichLip.WebApi.Models.Base
{
    public class ResponseMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public int ReturnId { get; set; }
    }
}
