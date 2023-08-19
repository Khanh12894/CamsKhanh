// Auto-generated code - PLEASE DO NOT MODIFY.
// Generated date: 2019/09/04
#region Using

using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using XichLip.WebApi.Models.User;

#endregion Using
namespace XichLip.WebApi.Models.UserTokens
{
    /// <summary>
    /// Entity for table UserTokensModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("UserTokens")]
    public partial class UserTokensModel {
        #region Properties
        //
        public long UserId {get; set;}
        //
        public string LoginProvider {get; set;}
        //
        public string Name {get; set;}
        //
        public string RefreshToken {get; set;}
        #endregion Properties        
    }     
}

