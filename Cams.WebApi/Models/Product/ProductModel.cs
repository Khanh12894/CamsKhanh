using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XichLip.WebApi.Models.Product
{
    [XmlRoot("Product")]
    public class ProductModel
    {
        #region Properties
        public long ProductId { get; set; }
        public string ProductKey { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserId { get; set; }
        public int AlowedPC { get; set; }
        public string Purchaser { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        #endregion Properties   
    }
}
