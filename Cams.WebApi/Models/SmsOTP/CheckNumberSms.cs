using System;
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
    [XmlRoot("CheckNumberSms")]
    public partial class CheckNumberSms
    {
        #region Properties
        public int TotalSmsInHour { get; set; }
        public DateTime TimeAvailable { get; set; }
        public string Message { get; set; }
        #endregion Properties        
    }
}
