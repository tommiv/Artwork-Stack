using Artwork_Stack.DataAccessLayer;

namespace Artwork_Stack.Model
{
    internal class ServiceContext
    {
        public Services.Supported InternalID { get; set; }
        public string DisplayedName          { get; set; }
        public IServiceProvider Provider     { get; set; }
    }
}
