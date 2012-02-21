using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    public class Discogs : IServiceProvider
    {
        private const string apiurl = "http://api.discogs.com";
        
        private static string SearchRequestUrl(string query)
        {
            return string.Format("{0}/database/search?q={1}", apiurl, HttpUtility.UrlEncode(query));
        }

        private static string ReleasesRequestUrl(string id)
        {
            return string.Format("{0}/releases/{1}", apiurl, id);
        }

        public UnifiedResponse Search(string query)
        {
            var output = new UnifiedResponse();

            var json = new JavaScriptSerializer();
            var respstr = Tools.Http.getText(SearchRequestUrl(query));
            dynamic response = json.DeserializeObject(respstr);

            dynamic results = response["results"];
            if (results != null)
            {
                int i = 0;
                int cursor = 0;
                while (cursor < results.Length && i < Math.Min(results.Length, 20))
                {
                    var entry = results[i];
                    var r = new Result();
                    var title = Regex.Split((string)(entry["title"] ?? string.Empty), " - ");
                    r.Artist = title.Length > 0 ? title[0] : string.Empty;
                    r.Album = title.Length > 1 ? title[1] : string.Empty;
                    r.Request = query;
                    r.Thumb = entry["thumb"] ?? string.Empty;
                    r.AdditionalInfo = (entry["id"] ?? string.Empty).ToString();

                    if (!string.IsNullOrEmpty(r.Thumb) && !string.IsNullOrEmpty((string)r.AdditionalInfo))
                    {
                        output.Results.Add(r);
                        i++;
                    }

                    cursor++;
                }
            }
            output.ResultsCount = output.Results.Count;
            return output;
        }

        public bool GetFullsizeUrlViaCallback { get { return true; } }

        public Func<object, string> GetFullsizeUrlCallback { get { return GetFullsize; } }
        private string GetFullsize(object id)
        {
            string url = ReleasesRequestUrl(id.ToString());
            var resp = Tools.Http.getText(url);
            var json = new JavaScriptSerializer();
            dynamic release = json.DeserializeObject(resp);

            dynamic images = null;
            try
            {
                images = release["images"];
            }
            catch(Exception)
            {
                return string.Empty;
            }

            if (images != null && images.Length > 0)
            {
                url = images[0]["uri"];
            }

            return url;
        }
    }
}
