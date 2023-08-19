using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XichLip.WebApi.Models.Base;

namespace XichLip.WebApi.Models.ProductType
{
    public class ProductTypeModel:BaseModel
    {
        #region Properties
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        #endregion Properties   
    }
}
