using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artwork_Stack
{
    public static class Model
    {
        public struct grid
        {
            public const int i = 4;
            public const int j = 4;
            public const int w = 100;
            public const int h = 130;
            public const int pw = 4; // Space between cells
            public const int ph = 4;
            public const int lm = 12; // Left margin
            public const int tm = 12;
        }

        public struct gImgAPIWorkerParams
        {
            public string query;
            public int rsz; // results per page
            public int i;   // thread position
            public gImgAPIWorkerParams(string _query, int _rsz, int _i)
            {
                query = _query; rsz = _rsz; i = _i;
            }
        }

        public struct getIMGWorkerParams
        {
            public string tburl;
            public string url;
            public int ix; // thread position
            public int iy;
            public int w;
            public int h;
            public string caption;
            public getIMGWorkerParams(string _tburl, string _url, int _ix, int _iy, int _w, int _h, string _caption)
            {
                tburl = _tburl; url = _url; ix = _ix; iy = _iy; w = _w; h = _h; caption = _caption;
            }
        }
    }
}
