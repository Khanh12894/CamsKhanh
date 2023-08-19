using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace XichLip.WebApi.Utilities
{
    
    public static class SendSMSUtils
    {
       
        private static string APIKey = "E70BA9A70A1EEC1CC64A9B29D18AA3";
        private static string SecretKey = "1502F47C64921E4A17BEB8E941A1A3";
        public static bool Send(string phone, string message,out string resOut)
        {
            bool success = true;
            try
            {

                //string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V4/";
                string url = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_post/";
                // declare ascii encoding
                UTF8Encoding encoding = new UTF8Encoding();

                string strResult = string.Empty;


                string customers = "";

                string[] lstPhone = phone.Split(',');

                for (int i = 0; i < lstPhone.Count(); i++)
                {
                    customers = customers + @"<CUSTOMER>"
                                    + "<PHONE>" + lstPhone[i] + "</PHONE>"
                                    + "</CUSTOMER>";
                }

                //string SampleXml = @"<RQST>"


                string SampleXml = @"<RQST>"
                                   + "<APIKEY>" + APIKey + "</APIKEY>"
                                   + "<SECRETKEY>" + SecretKey + "</SECRETKEY>"
                                   //+ "<ISFLASH>0</ISFLASH>"
                                   //+ "<BRANDNAME>QCAO_ONLINE</BRANDNAME>"  //De dang ky brandname rieng vui long lien he hotline 0902435340 hoac nhan vien kinh Doanh cua ban
                                   + "<BRANDNAME>Verify</BRANDNAME>"
                                   + "<SMSTYPE>2</SMSTYPE>"
                                   + "<CONTENT>" + message + "</CONTENT>"
                                   + "<CONTACTS>" + customers + "</CONTACTS>"


               + "</RQST>";
                string postData = SampleXml.Trim().ToString();
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                webrequest.Timeout = 500000;
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream =
                    new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // below steps remove unwanted data from response string
                strResult = strResult.Replace("</string>", "");
                resOut = strResult;
            }
            catch(Exception ex)
            {
                resOut = ex.Message + " " +ex.StackTrace;
                success = false;
                throw ex;
            }
            return success;
        }

        public static bool SendGet(string phone, string message, out string resOut)
        {
            bool success = true;
            try
            {

                //string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V4/";
                string url = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get";
                // declare ascii encoding
                UTF8Encoding encoding = new UTF8Encoding();

                string strResult = string.Empty;

                string[] lstPhone = phone.Split(',');
                url += "?Phone=" + lstPhone[0] + "&Content=" + message + "&ApiKey=" + APIKey + "&SecretKey=" + SecretKey + "&IsUnicode=0&Brandname=LiberEduVn&SmsType=2";
                url = url.Trim().ToString();
                //url = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=0967283495&Content=Ma%20OTP%20cua%20ban%20la%20123456&ApiKey=A0393813A548E338F4F955F94B9B76&SecretKey=3B6C5BE27E8FD4F8EC908D8A168118&IsUnicode=0&Brandname=Verify&SmsType=2";
                HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                // set method as get
                webrequest.Method = "GET";
                webrequest.Timeout = 500000;
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                //webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                //Stream newStream = webrequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream =
                    new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // below steps remove unwanted data from response string
                strResult = strResult.Replace("</string>", "");
                resOut = strResult;
            }
            catch (Exception ex)
            {
                resOut = ex.Message + " " + ex.StackTrace;
                success = false;
                throw ex;
            }
            return success;
        }
    }
}
