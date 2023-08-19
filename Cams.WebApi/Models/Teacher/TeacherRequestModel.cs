// Generated date: 2021/03/24
#region Using

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using XichLip.WebApi.Models.Base;

#endregion Using
namespace XichLip.WebApi.Models.Teacher
{
    /// <summary>
    /// Entity for table Teacher
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2021/03/24
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("TeacherRequest")]
    public partial class TeacherRequestModel: BaseRequestGetListModel {
        #region Properties
        //
        public int TeacherId {get; set;}
        //
        public int? ActiveCodeId {get; set;}
        //
        public string Code {get; set;}
        //
        public string Name {get; set;}
        //
        public string ImageUrl {get; set;}
        //
        public string LastName {get; set;}
        //
        public string FirstName {get; set;}
        //
        public string Email {get; set;}
        //
        public DateTime? Birthday {get; set;}
        //
        public string PhoneNumber {get; set;}
        //
        public long? UserId {get; set;}
        //
        public int? SchoolId {get; set;}
        //
        public int? EducationDepartmentId {get; set;}
        //
        public int? SchoolTypeId {get; set;}
        //
        public int? Gender {get; set;}
        //
        public DateTime? CreateAt {get; set;}
        //
        public long? CreateBy {get; set;}
        //
        public DateTime? UpdateAt {get; set;}
        //
        public long? UpdateBy {get; set;}
        //
        public bool? IsDelete {get; set;}
        //
        public string RoleInSchool {get; set;}
        //
        public int? PhongGDDTId {get; set;}
        //
        public int? Status {get; set;}
        //
        public int? TotalTimeAccess {get; set;}
        //
        public string Address {get; set;}
        //
        public bool? IsFinishInfoTeacher {get; set;}
        //
        public bool? IsMustChangeSchool {get; set;}
        //
        public string RequestOfTeacher {get; set;}
        //
        public string Qualification {get; set;}
        //
        public bool? IsConfirmAccount {get; set;}
        #endregion Properties        
    }     
}

