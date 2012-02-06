using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artwork_Stack.Model;

namespace Artwork_Stack.DataAccessLayer
{
    interface IServiceProvider
    {
        UnifiedResponse Search(string query);
    }
}
