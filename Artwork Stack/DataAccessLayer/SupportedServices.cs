using System.Collections.Generic;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    internal class Services
    {
        internal static Dictionary<Supported, ServiceContext> Providers = new Dictionary<Supported, ServiceContext>
        {
            {
                Supported.Amazon,
                new ServiceContext
                {
                    InternalID = Supported.Amazon,
                    DisplayedName = "Amazon.com",
                    Provider = new Amazon()
                }
            }, 
            {
                Supported.LastFm,
                new ServiceContext
                {
                    InternalID = Supported.LastFm,
                    DisplayedName = "Last.fm",
                    Provider = new LastFm()
                }
            }, 
            {
                Supported.Discogs,
                new ServiceContext
                {
                    InternalID = Supported.Discogs,
                    DisplayedName = "Discogs.com",
                    Provider = new Discogs()
                }
            }
        };

        internal enum Supported
        {
            LastFm,
            Discogs,
            Amazon
        }
    }
}