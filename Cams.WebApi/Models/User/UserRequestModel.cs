#region Using

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using XichLip.WebApi.Models.Base;

#endregion Using
namespace XichLip.WebApi.Models.User
{
    /// <summary>
    /// Entity for table UserModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/12
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("UserRequest")]
    public partial class UserRequestModel : BaseRequestGetListModel
    {
        #region Properties
        //

        public string UserName { get; set; }
        public string FullName { get; set; }
        #endregion Properties        
    }     
}

