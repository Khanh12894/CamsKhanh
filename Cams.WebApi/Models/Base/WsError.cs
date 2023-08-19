using System;
using System.Collections.Generic;
using System.Text;

namespace XichLip.WebApi.Models.Base
{
    public class WsError
    {
        public int Code { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
