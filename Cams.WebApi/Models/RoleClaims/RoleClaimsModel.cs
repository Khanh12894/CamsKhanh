// Auto-generated code - PLEASE DO NOT MODIFY.
// Generated date: 2019/09/04
#region Using

using System;
using System.Xml.Serialization;

#endregion Using
namespace XichLip.WebApi.Models.RoleClaims
{
    /// <summary>
    /// Entity for table RoleClaimsModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("RoleClaims")]
    public partial class RoleClaimsModel {
        #region Properties
        //
        public int Id {get; set;}
        //
        public int? RoleId {get; set;}
        //
        public string ClaimType {get; set;}
        //
        public string ClaimValue {get; set;}
        #endregion Properties        
    }     
}

