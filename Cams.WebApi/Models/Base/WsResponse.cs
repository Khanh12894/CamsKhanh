using System.Collections.Generic;

namespace XichLip.WebApi.Models.Base
{
    public class WsResponse
    {
        public string Status { get; set; }
        public object Data { get; set; }
        public List<WsError> Errors { get; set; }
    }
}
