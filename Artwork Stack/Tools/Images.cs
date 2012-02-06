using System;
using System.Drawing;

namespace Artwork_Stack.Tools
{
    public static class Images
    {
        public static Image ResizeImage(Image imgToResize, Size size)
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

        public static Image CropImage(Image img)
        {
            bool IsPortrait = img.Width < img.Height;
            int cropped = Math.Abs(img.Width - img.Height);
            var cropArea = new Rectangle(
                IsPortrait?         0           : cropped/2,
                IsPortrait? cropped/2           : 0,
                IsPortrait? img.Width           : img.Width - cropped,
                IsPortrait? img.Width - cropped : img.Height
            );
            var input  = new Bitmap(img);
            var output = input.Clone(cropArea, input.PixelFormat);
            return output;
        }
    }
}
