// Auto-generated code - PLEASE DO NOT MODIFY.
// Generated date: 2019/09/04
#region Using

using System;
using System.Xml.Serialization;

#endregion Using
namespace XichLip.WebApi.Models.UserLogins
{
    /// <summary>
    /// Entity for table UserLoginsModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("UserLogins")]
    public partial class UserLoginsModel {
        #region Properties
        //
        public long UserId {get; set;}
        //
        public string LoginProvider {get; set;}
        //
        public string ProviderKey {get; set;}
        //
        public string ProviderDisplayName {get; set;}
        #endregion Properties        
    }     
}

