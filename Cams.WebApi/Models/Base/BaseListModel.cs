using System.Collections.Generic;
using Newtonsoft.Json;

namespace XichLip.WebApi.Models.Base
{
    public class BaseListModel<T> where T : class
    {
        [JsonProperty("list")]
        public List<T> ListModels { get; set; }
        public PagerModel Paging { get; set; }
    }
}
