using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artwork_Stack.Tools
{
    public static class Text
    {
        public static string Correct(this string input)
        {
            var buf = Encoding.GetEncoding("iso-8859-1").GetBytes(input);
            return Encoding.GetEncoding(1251).GetString(buf);
        }
    }
}
