using System.Collections.Generic;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    internal class Services
    {
        internal static Dictionary<Supported, ServiceContext> Providers = new Dictionary<Supported, ServiceContext>
        {
            { Supported.Discogs, new ServiceContext { InternalID = Supported.Discogs, DisplayedName = "Discogs.com", Provider = new Discogs() } },
            { Supported.LastFm,  new ServiceContext { InternalID = Supported.LastFm,  DisplayedName = "Last.fm",     Provider = new LastFm()  } }
        };

        internal enum Supported
        {
            LastFm,
            Discogs
        }
    }
}