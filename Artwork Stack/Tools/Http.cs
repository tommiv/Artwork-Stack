using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;

namespace Artwork_Stack.Tools
{
   public static class Http
    {
       private const string useragent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; "
            + ".NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; "
            + "Tablet PC 2.0; OfficeLiveConnector.1.4; OfficeLivePatch.1.3)";

        public static WebResponse httpRequestGET(string URL, string cookies)
        {
            WebRequest reqGET = WebRequest.Create(URL);
            reqGET.Headers["UserAgent"] = useragent;
            if (cookies != null) { reqGET.Headers["Cookie"] = cookies; }
            return reqGET.GetResponse();
        }
        public static WebResponse httpRequestGET(string URL) { return httpRequestGET(URL, null); }

        public static WebResponse httpRequestPOST(string URL, byte[] sendBuffer, string cookies)
        {
            var reqPOST = WebRequest.Create(URL);
            reqPOST.Method = "POST";
            reqPOST.Timeout = 30000;
            reqPOST.Headers["UserAgent"] = useragent;
            if (cookies != null) { reqPOST.Headers["Cookie"] = cookies; }
            reqPOST.ContentType = "application/x-www-form-urlencoded";
            reqPOST.ContentLength = sendBuffer.Length;
            Stream sendStream = reqPOST.GetRequestStream();
            sendStream.Write(sendBuffer, 0, sendBuffer.Length);
            sendStream.Close();

            return reqPOST.GetResponse();
        }
        public static WebResponse httpRequestPOST(string URL, string postString, string cookies)
        {
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(postString);
            return httpRequestPOST(URL, byteArr, cookies);
        }
        public static WebResponse httpRequestPOST(string URL, string postString) { return httpRequestPOST(URL, postString, null); }

        public static string getResponseContent(WebResponse response)
        {
            var s = response.GetResponseStream();
            if (s == null)
            {
                return null;
            }
            var reader = new StreamReader(s, System.Text.Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        public static Image getPicture(string URL)
        {
            if (string.IsNullOrEmpty(URL)) return null;
            WebRequest request = WebRequest.Create(URL);
            try
            {
                Stream stream = request.GetResponse().GetResponseStream();
                Image image = Image.FromStream(stream);
                return image;
            }
            catch { return null; }
        }

        public static string getText(string URL)
        {
            if (string.IsNullOrEmpty(URL)) return null;
            WebRequest request = WebRequest.Create(URL);
            try
            {
                var resp = request.GetResponse();
                var br = new StreamReader(resp.GetResponseStream());
                return br.ReadToEnd();
            }
            catch { return null; }
        }

        public static XDocument getXmlDoc(string URL)
        {
            if (string.IsNullOrEmpty(URL)) return null;

            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.Headers["UserAgent"] = useragent;
            request.Accept = "text/html,application/xhtml+xml,application/xml";

            XDocument doc = null;
            try
            {
                var resp = request.GetResponse();
                var br = new StreamReader(resp.GetResponseStream());
                doc = XDocument.Load(br);
            }
            catch { return null; }
            return doc;
        }

       public static byte[] getStream(string URL)
        {
            if (URL == null) return null;
            WebRequest request = WebRequest.Create(URL);
            try
            {
                var resp    = request.GetResponse();
                int bufSize = Convert.ToInt32(resp.ContentLength);
                var br      = new BinaryReader(resp.GetResponseStream());
                return br.ReadBytes(bufSize);
            }
            catch { return null; }
        }
        
        public static bool checkInternetState() // True if connection OK
        {
            var checkReqGoogle = WebRequest.Create("http://google.com");
            try { checkReqGoogle.GetResponse(); return true; }
            catch { }
            var checkReqMS = WebRequest.Create("http://ya.ru");
            try { checkReqMS.GetResponse(); return true; }
            catch { return false; }
        }
        
        public static bool checkInternetStateViaPing() {
            var pingSender = new Ping();
            var options    = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
            PingReply reply;
            try { reply = pingSender.Send("vkontakte.ru", 300, buffer, options); }
            catch { return false; }
            if (reply.Status == IPStatus.Success) return true; else return false;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool checkInternetStateFast()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
    }
}
