using System.Collections.Generic;
using System.ComponentModel;

namespace XichLip.WebApi.Models.Base
{
    public class BaseRequestGetListModel
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; }
        public string Direction { get; set; } = "DESC";

    }
    public class BaseRequestGetListsModel
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; }
        public string Direction { get; set; } = "DESC";
        public string Start { get; set; }
        public string End { get; set; }
    }
}
