using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.WinForms
{
    public class Line
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public float Point1Z { get; set; }
        public float Point2Z { get; set; }

        public Line(PointF p1, PointF p2, float p1Z, float p2Z)
        {
            Point1 = new Point((int)p1.X, (int)p1.Y);
            Point2 = new Point((int)p2.X, (int)p2.Y);
            Point1Z = p1Z;
            Point2Z = p2Z;
        }

        public void Draw(ref Bitmap b)
        {
            int w = Point2.X - Point1.X;
            int h = Point2.Y - Point1.Y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            int x = Point1.X;
            int y = Point1.Y;
            for (int i = 0; i <= longest; i++)
            {
                if (b.Width > x && b.Height > y && x >= 0 && y >= 0)
                    b.SetPixel(x, y, Color.Black);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
    }
}