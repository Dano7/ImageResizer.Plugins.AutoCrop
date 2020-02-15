﻿using System;
using System.Drawing;

namespace ImageResizer.Plugins.AutoCrop.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle Aspect(this Rectangle rectangle, int width, int height)
        {
            return Aspect(rectangle, width / (float)height, width, height);
        }

        public static Rectangle Aspect(this Rectangle rectangle, float aspect, int width, int height)
        {
            if (rectangle.Equals(Rectangle.Empty)) return rectangle;
            if (aspect == 0) return rectangle;

            var ta = rectangle.Width / (float)rectangle.Height;

            if (Math.Abs(aspect - ta) < 0.01f)
                return rectangle;

            if (aspect > ta)
            {
                var iw = (int)Math.Ceiling(rectangle.Height * aspect);
                var p = (int)Math.Ceiling((iw - rectangle.Width) * 0.5f);
                return Expand(rectangle, p, 0, width, height);
            }
            else
            {
                var ih = (int)Math.Ceiling(rectangle.Width / aspect);
                var p = (int)Math.Ceiling((ih - rectangle.Height) * 0.5f);
                return Expand(rectangle, 0, p, width, height);
            }
        }

        public static Rectangle Aspect(this Rectangle rectangle, float aspect)
        {
            if (rectangle.Equals(Rectangle.Empty)) return rectangle;
            if (aspect == 0) return rectangle;

            var ta = rectangle.Width / (float)rectangle.Height;

            if (Math.Abs(aspect - ta) < 0.01f)
                return rectangle;

            if (aspect > ta)
            {
                var iw = (int)Math.Ceiling(rectangle.Height * aspect);
                var p = (int)Math.Ceiling((iw - rectangle.Width) * 0.5f);
                return Expand(rectangle, p, 0);
            }
            else
            {
                var ih = (int)Math.Ceiling(rectangle.Width / aspect);
                var p = (int)Math.Ceiling((ih - rectangle.Height) * 0.5f);
                return Expand(rectangle, 0, p);
            }
        }

        public static Rectangle Constrain(this Rectangle rectangle, Rectangle other)
        {
            var xn = Math.Max(rectangle.Left, other.Left);
            var yn = Math.Max(rectangle.Top, other.Top);
            var xm = Math.Min(rectangle.Right, other.Right);
            var ym = Math.Min(rectangle.Bottom, other.Bottom);

            if (rectangle.X + rectangle.Width > other.X + other.Width)
                xm -= (rectangle.X + rectangle.Width) - (other.X + other.Width);

            if (rectangle.Y + rectangle.Height > other.Y + other.Height)
                ym -= (rectangle.Y + rectangle.Height) - (other.Y + other.Height);

            return new Rectangle(xn, yn, xm, ym);
        }

        public static Rectangle Scale(this Rectangle rectangle, double scale)
        {
            if (scale == 1)
                return rectangle;

            var x = (int)Math.Round(rectangle.X * scale);
            var y = (int)Math.Round(rectangle.Y * scale);
            var w = (int)Math.Round(rectangle.Width * scale);
            var h = (int)Math.Round(rectangle.Height * scale);

            return new Rectangle(x, y, w, h);
        }

        public static Rectangle Translate(this Rectangle rectangle, Point point)
        {
            return new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, rectangle.Width, rectangle.Height);
        }

        

        public static Rectangle Expand(this Rectangle rectangle, int paddingX, int paddingY)
        {
            if (paddingX == 0 && paddingY == 0) return rectangle;

            return new Rectangle(rectangle.X - paddingX, 
                                 rectangle.Y - paddingY, 
                                 rectangle.Width + paddingX * 2, 
                                 rectangle.Height + paddingY * 2);
        }

        public static Rectangle Contract(this Rectangle rectangle, double percent)
        {
            return Contract(rectangle, percent, percent);
        }

        public static Rectangle Contract(this Rectangle rectangle, double percentX, double percentY)
        {
            if (percentX == 0 && percentY == 0)
                return rectangle;

            if (percentX > 100) 
                percentX = 100;

            if (percentY > 100)
                percentY = 100;
            
            var rx = percentX * 0.01 * 0.5;
            var ry = percentY * 0.01 * 0.5;

            var xn = rectangle.Left + (int)(rectangle.Width * rx);
            var xm = rectangle.Right - (int)(rectangle.Width * rx);
            
            var yn = rectangle.Top + (int)(rectangle.Height * ry);
            var ym = rectangle.Bottom - (int)(rectangle.Height * ry);

            return new Rectangle(xn, yn, xm - xn, ym - yn);
        }

        public static Rectangle Expand(this Rectangle rectangle, int paddingX, int paddingY, int maxWidth, int maxHeight)
        {
            if (paddingX == 0 && paddingY == 0)
                return rectangle;

            var xnc = 0;
            var xn = rectangle.X - paddingX;
            if (xn < 0)
            {
                xnc = -xn;
            }

            var xmc = 0;
            var xm = rectangle.Right + paddingX;
            if (xm > maxWidth)
            {
                xmc = xm - maxWidth;
            }

            var c = Math.Max(xnc, xmc);

            xn += c;
            xm -= c;

            var ync = 0;
            var yn = rectangle.Y - paddingY;
            if (yn < 0)
            {
                ync = -yn;
            }

            var ymc = 0;
            var ym = rectangle.Bottom + paddingY;
            if (ym > maxHeight)
            {
                ymc = ym - maxHeight;
            }

            c = Math.Max(ync, ymc);

            yn += c;
            ym -= c;

            return new Rectangle(xn, yn, xm - xn, ym - yn);
        }
    }
}
