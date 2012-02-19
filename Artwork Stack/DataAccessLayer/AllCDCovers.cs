using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    public class AllCDCovers : IServiceProvider
    {
        private string BuildQueryString(string query)
        {
            return string.Format(
                "http://www.allcdcovers.com/api/search/!username!/{0}/{1}/music",
                CalculateHash("apisecret" + query),
                HttpUtility.UrlEncode(query, Encoding.UTF8)
            );
        }

        private string CalculateHash(string data)
        {
            var provider = MD5.Create();
            var hashbuffer = provider.ComputeHash(Encoding.Default.GetBytes(data));
            var sb = new StringBuilder();
            foreach (byte t in hashbuffer)
            {
                sb.AppendFormat("{0:x02}", t);
            }
            return sb.ToString();
        }

        public UnifiedResponse Search(string query)
        {
            string xmlstring = Tools.Http.getText(BuildQueryString(query));
            var doc = new XmlDocument();
            var output = new UnifiedResponse();
            try
            {
                doc.LoadXml(xmlstring);
            }
            catch (Exception e)
            {
                output.Success = false;
                output.Error = e.Message;
                return output;
            }

            return output;
        }

        public bool GetFullsizeUrlViaCallback { get { return false; } }

        public Func<object, string> GetFullsizeUrlCallback { get { return o => string.Empty; } }
    }
}
