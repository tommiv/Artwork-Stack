using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    public class Discogs : IServiceProvider
    {
        private const string apiurl = "http://api.discogs.com";

        private static string SearchRequestUrl(string query)
        {
            return string.Format("{0}/search?q={1}", apiurl, HttpUtility.UrlEncode(query));
        }

        private static string ReleasesRequestUrl(string id)
        {
            return string.Format("{0}/{1}", apiurl, id);
        }

        private string request;

        public UnifiedResponse Search(string query)
        {
            request = query;

            var doc = Tools.Http.getXmlDoc(SearchRequestUrl(query));
            var output = new UnifiedResponse();
            output.Success = true;
            output.Results = doc.Descendants("result").Select(SelectResult).ToList();
            output.ResultsCount = output.Results.Count;
            return output;
        }

        private Result SelectResult(XElement input)
        {
            string thumb = input.Element("thumb") != null ? input.Element("thumb").Value : string.Empty;
            string title = input.Element("title") != null ? input.Element("title").Value : string.Empty;
            
            string uri = input.Element("uri")   != null ? input.Element("uri").Value   : string.Empty;
            string id = string.Join("/", uri.Split('/').Reverse().Take(2).Reverse().ToArray());
            
            var albuminfo = Regex.Split(title, " - ");

            var res = new Result();
            res.Thumb = thumb;
            res.AdditionalInfo = id;
            res.Artist = albuminfo.Length > 0 ? albuminfo[0] : string.Empty;
            res.Album  = albuminfo.Length > 1 ? albuminfo[1] : string.Empty;
            res.Request = request;
            return res;
        }

        public bool GetFullsizeUrlViaCallback { get { return true; } }

        public Func<object, string> GetFullsizeUrlCallback { get { return GetFullsize; } }
        private string GetFullsize(object id)
        {
            var doc = Tools.Http.getXmlDoc(ReleasesRequestUrl((string)id));
            var images = doc.Descendants("image").ToList();
            if (!images.Any())
            {
                return string.Empty;
            }
            var primary = images.FirstOrDefault(r => r.Attribute("type").Value == "primary");
            return primary != null ? primary.Attribute("uri").Value : images[0].Attribute("uri").Value;
        }
    }
}
