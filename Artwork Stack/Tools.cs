using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Artwork_Stack
{
    public static class Tools
    {
        public static Image resizeImage(Image imgToResize, Size size)
        {
            float nPercentW = (size.Width / (float)imgToResize.Width);
            float nPercentH = (size.Height / (float)imgToResize.Height);
            float nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            int destWidth = (int)(imgToResize.Width * nPercent);
            int destHeight = (int)(imgToResize.Height * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
    }
}
