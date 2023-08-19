using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XichLip.WebApi.Models.Base;

namespace XichLip.WebApi.Models.Registration
{
    [XmlRoot("RegistrationRequest")]
    public class RegistrationRequestModel : BaseRequestGetListModel
    {
        public string Version { get; set; }
        //public string PW { get; set; }
        //public int UserName { get; set; }
    }
}