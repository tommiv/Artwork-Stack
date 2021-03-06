﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Artwork_Stack.Model;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;

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
            return string.Format("{0}/{1}", apiurl, id);
        }

        private WebClient getClient()
        {
            var wcl = new WebClient();
            wcl.Headers.Add("User-Agent", "ArtworkStack/0.1b +https://github.com/tommiv/Artwork-Stack");
            wcl.Headers.Add("Accept", "application/json");
            return wcl;
        }

        public UnifiedResponse Search(string query)
        {
            var wcl = this.getClient();
            string response = wcl.DownloadString(SearchRequestUrl(query));
            var output = new UnifiedResponse();
            if (string.IsNullOrWhiteSpace(response))
            {
                output.Success = false;
                output.Error = "Cannot get server response";
                return output;
            }
            else
            {
                try
                {
                    dynamic searchres = new JavaScriptSerializer().Deserialize<dynamic>(response);
                    output.Success = true;

                    output.Results = new List<Result>();
                    var items = (dynamic[])searchres["results"];
                    foreach (var item in items)
                    {
                        if (item["type"] != "release" && item["type"] != "master") continue;
                        var release = new Result();
                        release.Thumb          = ResUrl(item["thumb"]);
                        release.AdditionalInfo = item["resource_url"];
                        release.Artist = release.Album = item["title"];
                        release.Request        = query;
                        output.Results.Add(release);
                    }

                    output.ResultsCount = output.Results.Count;
                    return output;
                }
                catch (Exception e)
                {
                    output.Success = false;
                    output.Error = e.Message;
                    return output;
                }
            }
        }

        private static string ResUrl(string url)
        {
            return url.Replace("api.discogs", "s.pixogs");
        }

        public bool GetFullsizeUrlViaCallback { get { return true; } }

        public Func<object, string> GetFullsizeUrlCallback { get { return GetFullsize; } }
        private string GetFullsize(object url)
        {
            try
            {
                string plain = this.getClient().DownloadString((string)url);
                if (string.IsNullOrWhiteSpace(plain))
                {
                    return null;
                }
                else
                {
                    string candidate = null;
                    int size = 0;
                    var resp = new JavaScriptSerializer().Deserialize<dynamic>(plain);
                    foreach (dynamic img in resp["images"])
                    {
                        if (img["type"] == "primary") return ResUrl(img["uri"]);
                        int widesize = Math.Min(img["width"], img["height"]);
                        if (widesize > size)
                        {
                            size = widesize;
                            candidate = img["uri"];
                        }
                    }
                    return ResUrl(candidate);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
