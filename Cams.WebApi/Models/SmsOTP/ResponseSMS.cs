using System.Xml.Serialization;

namespace XichLip.WebApi.Models.SmsOTP
{
    /// <summary>
    /// Entity for table VehicleModel
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/02/22
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("ResponseSms")]
    public partial class ResponseSms
    {
        #region Properties
        public bool Status { get; set; }
        public bool IsSent { get; set; }
        public string Message { get; set; }
        public int Data { get; set; }
        #endregion Properties        
    }
}
