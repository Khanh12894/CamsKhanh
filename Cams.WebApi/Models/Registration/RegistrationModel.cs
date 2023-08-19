using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XichLip.WebApi.Models.Registration
{
    [XmlRoot("Registration")]
    public class RegistrationModel
    {
        #region Properties
        public int No { get; set; }
        public string Version { get; set; }
        public string UploadDate { get; set; }
        public string ApplyDate { get; set; }
        public string UpdateFile { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDelete { get; set; }
        #endregion Properties   
    }
}
