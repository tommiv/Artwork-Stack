using System.Drawing;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace Artwork_Stack.Tools
{
   public static class Http
    {
       private const string useragent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; "
            + ".NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; "
            + "Tablet PC 2.0; OfficeLiveConnector.1.4; OfficeLivePatch.1.3)";

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
    }
}
