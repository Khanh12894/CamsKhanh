using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XichLip.WebApi.Models.User
{
    [XmlRoot("Users")]
    public class UsersModel
    {
        #region Properties
        public int Seq { get; set; }
        public string ID { get; set; }
        public string PW { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDelete { get; set; }
        #endregion Properties   
    }
}
