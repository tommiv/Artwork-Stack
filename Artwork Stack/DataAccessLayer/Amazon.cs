using System;
using System.Text;
using System.Web;
using System.Xml;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    public class Amazon : IServiceProvider
    {
        // using signing service helps to hide secret key and simplifies request
        private const string ApiFrontendUrl = @"http://sowacs.appspot.com/AWS/webservices.amazon.com/onca/xml";

        private static string BuildSearchRequestUrl(string query)
        {
            return string.Format(
                "{0}?Service=AWSECommerceService&Operation=ItemSearch" +
                "&AWSAccessKeyId=AKIAIEUL7677YZ4IQZRQ&SearchIndex=Music&Keywords={1}" +
                "&ResponseGroup=Medium&AssociateTag=artwor02-20",
                ApiFrontendUrl,
                HttpUtility.UrlEncode(query, Encoding.UTF8)
            );
        }

        public UnifiedResponse Search(string query)
        {
            var xmlstring = Tools.Http.getText(BuildSearchRequestUrl(query))
                .Replace("xmlns=\"http://webservices.amazon.com/AWSECommerceService/2005-10-05\"", ""); // hack to avoid namespace manager
            var response = new XmlDocument();

            var output = new UnifiedResponse();
            try
            {
                response.LoadXml(xmlstring);
            }
            catch(Exception e)
            {
                output.Success = false;
                output.Error = e.Message;
            }

            var root = response.SelectSingleNode("/ItemSearchResponse");

            var valid = root.SelectSingleNode("Items/Request/IsValid");
            if (valid != null && valid.InnerXml == "True")
            {
                output.Success = true;
                var items = root.SelectNodes("//Item");

                foreach (XmlNode item in items)
                {
                    var imagenode = item.SelectSingleNode("LargeImage");
                    if (imagenode == null)
                    {
                        continue;
                    }

                    var r = new Result();
                    try
                    {
                        r.Artist = item.SelectSingleNode("ItemAttributes/Creator").InnerXml;
                        r.Album = item.SelectSingleNode("ItemAttributes/Title").InnerXml;
                        r.Url = imagenode.SelectSingleNode("URL").InnerXml;
                        r.Width = Convert.ToInt32(imagenode.SelectSingleNode("Width").InnerXml);
                        r.Height = Convert.ToInt32(imagenode.SelectSingleNode("Height").InnerXml);
                        var thumbnode = item.SelectSingleNode("MediumImage");
                        r.Thumb = thumbnode == null ? r.Url : thumbnode.SelectSingleNode("URL").InnerXml;
                    }
                    catch(Exception)
                    {
                        continue;
                    }

                    output.Results.Add(r);
                }

                output.ResultsCount = output.Results.Count;
            }

            return output;
        }

        public bool GetFullsizeUrlViaCallback { get { return false; } }

        public Func<object, string> GetFullsizeUrlCallback { get { return (dummy) => string.Empty; } }
    }
}
