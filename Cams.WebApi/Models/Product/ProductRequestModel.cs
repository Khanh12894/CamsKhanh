using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XichLip.WebApi.Models.Base;

namespace XichLip.WebApi.Models.Product
{
    [XmlRoot("ProductRequest")]
    public class ProductRequestModel: BaseRequestGetListModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public int ProductTypeId { get; set; }
        public bool? Expired { get; set; }
    }
}
