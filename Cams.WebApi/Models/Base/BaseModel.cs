using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XichLip.WebApi.Models.Base
{
    public class BaseModel
    {
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
