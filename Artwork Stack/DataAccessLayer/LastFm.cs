using System;
using System.Web;
using System.Xml;
using Artwork_Stack.Model;
using Artwork_Stack.Tools;

namespace Artwork_Stack.DataAccessLayer
{
    internal class LastFm : IServiceProvider
    {
        private const string apikey = @"893a602bf854328642e453031253928b";

        private static string GetExecutionUrl(string query)
        {
            return string.Format(
                @"http://ws.audioscrobbler.com/2.0/?method=album.search&album={0}&api_key={1}",
                HttpUtility.HtmlEncode(query),
                apikey
            );
        }

        UnifiedResponse IServiceProvider.Search(string query)
        {
            string url = GetExecutionUrl(query);
            var response = new UnifiedResponse();
            
            string xresp = Http.getText(url);
            if (string.IsNullOrEmpty(xresp))
            {
                response.Success = false;
                response.Error = "No http response from Last.fm API";
                return response;
            }

            var doc = new XmlDocument();
            doc.LoadXml(xresp);

            XmlNode error = doc.GetElementById("error");
            if (error != null)
            {
                response.Success = false;
                response.Error = string.Format(
                    "Last.fm API returns error #{0}: {1}",
                    error.Attributes["code"],
                    error.InnerXml
                );
                return response;
            }

            var nsmanager = new XmlNamespaceManager(doc.NameTable);
            nsmanager.AddNamespace("opensearch", "http://a9.com/-/spec/opensearch/1.1/");

            var countnode = doc.SelectSingleNode("/lfm/results/opensearch:totalResults", nsmanager);
            response.ResultsCount = countnode == null ? 0 : Convert.ToInt32(countnode.InnerXml);

            var results = doc.GetElementsByTagName("album");
            foreach (XmlNode result in results)
            {
                var r = new Result();
                r.Request = query;
                r.Artist  = result["artist"] == null ? string.Empty : result["artist"].InnerText;
                r.Album   = result["name"]   == null ? string.Empty : result["name"]  .InnerText;

                XmlNode urlnode = null;
                if ((urlnode = result.SelectSingleNode("descendant::image[@size='mega']")) != null)
                {
                    r.Width = r.Height = 500;
                }
                else if ((urlnode = result.SelectSingleNode("descendant::image[@size='extralarge']")) != null)
                {
                    r.Width = r.Height = 300;
                }
                else if((urlnode = result.SelectSingleNode("descendant::image[@size='large']")) != null)
                {
                    r.Width = r.Height = 174;
                }
                
                if (urlnode != null)
                {
                    r.Url = r.Thumb = urlnode.InnerXml;
                }
                response.Results.Add(r);
            }

            return response;
        }

        public bool GetFullsizeUrlViaCallback { get { return false; } }

        public Func<object, string> GetFullsizeUrlCallback
        {
            get { throw new NotImplementedException(); }
        }
    }
}
