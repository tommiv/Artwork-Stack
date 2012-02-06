using System.Collections.Generic;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    internal class Services
    {
        internal static Dictionary<Supported, ServiceContext> Providers = new Dictionary<Supported, ServiceContext>
        {
            { Supported.LastFm, new ServiceContext { InternalID = Supported.LastFm, DisplayedName = "Last.fm", Provider = new LastFm() } }
        };

        internal enum Supported
        {
            LastFm
        }
    }
}
