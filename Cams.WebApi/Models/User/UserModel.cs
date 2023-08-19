// Auto-generated code - PLEASE DO NOT MODIFY.
// Generated date: 2019/09/04
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using XichLip.WebApi.Models.Base;
#endregion Using
namespace XichLip.WebApi.Models.User
{
    /// <summary>
    /// Entity for table UserModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("User")]
    public partial class UserModel {
        #region Properties
        //
        public long UserId {get; set;}

        public long ClientUserId { get; set; }
        //
        public string UserName {get; set;}
        //
        public string NormalizedUserName {get; set;}

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Birthday { get; set; }

        public int UserType { get; set; }
        //
        public string Email {get; set;}
        //
        public string NormalizedEmail {get; set;}
        //
        public bool? EmailConfirmed {get; set;}
        //
        public string PasswordHash {get; set;}
        //
        public string SecurityStamp {get; set;}
        //
        public string ConcurrencyStamp {get; set;}
        //
        public string PhoneNumber {get; set;}
        //
        public bool? PhoneNumberConfirmed {get; set;}
        //
        public bool? TwoFactorEnabled {get; set;}
        //
        public DateTimeOffset? LockoutEnd {get; set;}
        //
        public bool? LockoutEnabled {get; set;}
        //
        public int? AccessFailedCount {get; set;}
        public int? RoleId { get; set; }
        public int? RoleLevel { get; set; }

        public string RoleName { get; set; }
        public string RoleCode { get; set; }
         public string avatar { get; set; }
         public string Address { get; set; }
        public bool? IsFirstLogin { get; set; }
        public bool? IsNotFirstLogin { get; set; }

        public bool? IsFinishInfo { set; get; }

        public bool? IsAddEmail { set; get; }
        //check comment account
        public bool? IsCommentAccount { set; get; }
        public bool? IsLock { set; get; }
        //check teacher transfer select subject
        public bool? IsCheckSubject { set; get; }
        //check teacher finish info
        public bool? IsFinishInfoTeacher { set; get; }
        //check school denied teacher info, teacher must change school
        public bool? IsMustChangeSchool { set; get; }
        //confirm have account or not have(true is confirm, false not yet confirm)
        public bool? IsConfirmAccount { set; get; }
        //check have noti not yet(role school)
        public bool? IsCheckHaveNoti { set; get; }
        //check school is choose subject yet?(role school)
        public bool? IsCheckSubjectSchool { set; get; }
        // check teacher in class 2, 6
        public bool? IsSpecialTeacher { set; get; }
        public string SsoIdentity { set; get; }
        public string OTP { get; set; }
        public bool? Activated { set; get; }
        public int? AdminTypeId { get; set; }
        // thông tin đăng nhập bằng các nền tảng
        public int CommunicationTypeId { get; set; }
        public string GoogleId { get; set; }
        public string FaceBookId { get; set; }
        public string id { get; set; }

        #endregion Properties        
        #region Methods
        public bool Validate()
        {
            bool isValid = true;

            return isValid;
        }
        #endregion
    }
    public class UserViewModel
    {
        #region Properties
        public long UserId { set; get; }
        public string UserName { set; get; }
        public string Avatar { set; get; }
        public string Email { set; get; }
        public string Name { set; get; }
        #endregion Properties 
    }
    public class UserViewListRequestModel : BaseRequestGetListModel
    {
        #region Properties
        public int TrainingId { set; get; }
        public int ClassId { set; get; }
        #endregion Properties 
    }

    public class UserSimpleModel
    {
        #region Properties
        public long UserId { set; get; }
        public string Email { set; get; }
        public int? AdminTypeId { set; get; }
        public string Name { set; get; }
        public bool? Activated { get; set; }
        #endregion Properties 
    }
    public partial class UserOTPModel
    {
        //
        public long UserId { get; set; }
        //
        public string OTP { get; set; }
        public string UserName { get; set; }
        public int Time { get; set; }
    }
    public class ActiveEmailModel
    {
        #region Properties
        public string Url { set; get; }
        public string Code { set; get; }
        public long UserId { set; get; }
        #endregion Properties 
    }

    public partial class UserSSOModel
    {
        #region Properties
        //
        public string UserNameSSO { get; set; }
        public string UserName { get; set; }
        //
        public long? SsoIdentityId { get; set; }
        public string Password { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public long ClientUserId { get; set; }
        public int ClientUpdate { get; set; }
        public string Email { get; set; }
        public int Count { get; set; }
        #endregion Properties        
    }

    public partial class ClientModel
    {
        #region Properties

        public long ClientId { get; set; }
        public string Name { get; set; }
        public string UrlWeb { get; set; }
        public bool IsSSO { get; set; }
        #endregion Properties        
    }

  
}

