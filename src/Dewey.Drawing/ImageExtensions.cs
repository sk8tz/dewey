﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Dewey.Drawing
{
    public static class ImageExtensions
    {
        public static byte[] GetImageBytes(this Image value)
        {
            if (value == null) {
                throw new ArgumentException(nameof(value));
            }

            return (byte[])(new ImageConverter()).ConvertTo(value, typeof(byte[]));
        }

        public static Image Resize(this byte[] value, int maxWidth, int maxHeight)
        {
            if (value == null) {
                throw new ArgumentException(nameof(value));
            }

            var image = GetImage(value);

            return Resize(image, maxWidth, maxHeight);
        }

        public static Image GetImage(this byte[] value)
        {
            if (value == null) {
                throw new ArgumentException(nameof(value));
            }

            using (var ms = new MemoryStream(value)) {
                return Image.FromStream(ms);
            }
        }

        public static Image Resize(this Image value, int maxWidth, int maxHeight)
        {
            if (value == null) {
                throw new ArgumentException(nameof(value));
            }
            
            var wScale = (float)maxWidth / value.Width;
            var hScale = (float)maxHeight / value.Height;

            var scale = Math.Min(wScale, hScale);

            var newWidth = (int)(scale * value.Width + 0.5f);
            var newHeight = (int)(scale * value.Height + 0.5f);

            var result = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

            var gfx = Graphics.FromImage(result);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(value, 0, 0, newWidth, newHeight);

            return result;
        }

        public static double GetScaleFactor(this Image value, double maxWidth, double maxHeight)
        {
            if (value == null) {
                throw new ArgumentException(nameof(value));
            }

            if (value.Width > maxWidth && value.Height > maxHeight) {
                if (value.Width > value.Height) {
                    return maxWidth / value.Width;
                }

                return maxHeight / value.Height;
            }

            if (value.Width > maxWidth) {
                return maxWidth / value.Width;
            }

            if (value.Height > maxHeight) {
                return maxHeight / value.Height;
            }

            return 1.0;
        }
    }
}
