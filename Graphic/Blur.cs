using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESWCtrls.Graphic
{
    /// <summary>
    /// Diffrent Blurring classses
    /// </summary>
    public unsafe class Blur
    {
        /// <summary>
        /// Simple Gaussian Blur
        /// </summary>
        /// <param name="image">The image to blur</param>
        /// <param name="blurSize">The radius in pixels of the blur</param>
        /// <returns>Bitmap with the result of the blur</returns>
        public static Bitmap Gaussian(Bitmap image, int blurSize)
        {
            int blur = blurSize / 2;
            int width = image.Width;
            int height = image.Height;
            Rectangle bounds = new Rectangle(0, 0, width, height);

            Bitmap copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Bitmap output = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using(Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImageUnscaled(image, 0, 0);
            }


            BitmapData outData = null;
            BitmapData srcData = null;
            try
            {
                outData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                srcData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);



                byte* outRow = (byte*)outData.Scan0.ToPointer();
                Int32* outPxl;

                for(int row = 0; row < height; ++row)
                {
                    outPxl = (Int32*)outRow;
                    for(int col = 0; col < width; ++col, ++outPxl)
                    {
                        ColorList list = new ColorList();
                        int x1 = col - blur < 0 ? 0 : col - blur;
                        int x2 = col + blur > width ? width : col + blur;
                        int y1 = row - blur < 0 ? 0 : row - blur;
                        int y2 = row + blur > height ? height : row + blur;

                        byte* srcRow = (byte*)srcData.Scan0.ToPointer();
                        Int32* srcPxl;

                        srcRow += (srcData.Stride * y1);
                        for(int y = y1; y < y2; ++y)
                        {
                            srcPxl = (Int32*)srcRow;
                            srcPxl += x1;
                            for(int x = x1; x < x2; ++x, ++srcPxl)
                            {
                                list.Add(new XColor((Color32*)srcPxl));
                            }

                            srcRow += srcData.Stride;
                        }

                        ((Color32*)outPxl)->ARGB = list.average().col32.ARGB;
                    }

                    outRow += outData.Stride;
                }
            }
            finally 
            { 
                output.UnlockBits(outData);
                copy.UnlockBits(srcData);
            }

            copy.Dispose();
            return output;
        }

        /// <summary>
        /// Fills a whole image with a color while preserving the alpha channel
        /// </summary>
        /// <param name="image">The image to fill</param>
        /// <param name="color">The color to fill with</param>
        /// <returns>The result image</returns>
        public static Bitmap FillAlphaPreserve(Bitmap image, Color color)
        {
            int width = image.Width;
            int height = image.Height;
            Rectangle bounds = new Rectangle(0, 0, width, height);
            
            Bitmap copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Bitmap output = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using(Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImageUnscaled(image, 0, 0);
            }

            BitmapData outData = null;
            BitmapData srcData = null;
            try
            {
                outData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                srcData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte* outRow = (byte*)outData.Scan0.ToPointer();
                Int32* outPxl;

                byte* srcRow = (byte*)srcData.Scan0.ToPointer();
                Int32* srcPxl;

                for(int row = 0; row < height; ++row)
                {
                    outPxl = (Int32*)outRow;
                    srcPxl = (Int32*)srcRow;
                    for(int col = 0; col < width; ++col, ++outPxl, ++srcPxl)
                    {
                        XColor xc = new XColor((Color32*)srcPxl);
                        xc.rgb = color;
                        ((Color32*)outPxl)->ARGB = xc.col32.ARGB;
                    }

                    outRow += outData.Stride;
                    srcRow += srcData.Stride;
                }
            }
            finally
            {
                output.UnlockBits(outData);
                copy.UnlockBits(srcData);
            }

            copy.Dispose();
            return output;
        }
    }
}
