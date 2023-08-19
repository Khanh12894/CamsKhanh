// Auto-generated code - PLEASE DO NOT MODIFY.
// Generated date: 2019/09/04
#region Using

using System;
using System.Xml.Serialization;

#endregion Using
namespace XichLip.WebApi.Models.UserClaims
{
    /// <summary>
    /// Entity for table UserClaimsModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("UserClaims")]
    public partial class UserClaimsModel {
        #region Properties
        //
        public long Id {get; set;}
        //
        public long? UserId {get; set;}
        //
        public string ClaimType {get; set;}
        //
        public string ClaimValue {get; set;}
        #endregion Properties        
    }     
}

