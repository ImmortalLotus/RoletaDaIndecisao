using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roleta.Helper
{
    internal class ArcHelper
    {
        public static Geometry geraArco(int StartAngle, int SweepAngle)
        {
            var angle1 = DegreesToRad(StartAngle);
            var angle2 = angle1 + DegreesToRad(SweepAngle);

            var startAngle = Math.Min(angle1, angle2);
            var sweepAngle = Math.Max(angle1, angle2);

            var normStart = RadToNormRad(startAngle);
            var normEnd = RadToNormRad(sweepAngle);

            var rect = new Rect(0, 0, 600, 600);

            if ((normStart == normEnd) && (startAngle != sweepAngle)) // Complete ring.
            {
                return new EllipseGeometry(rect.Deflate(1 / 2));
            }
            else if (SweepAngle == 0)
            {
                return new StreamGeometry();
            }
            else // Partial arc.
            {
                var deflatedRect = rect;

                var centerX = 0;
                var centerY = 0;

                var radiusX = 300;
                var radiusY = 300;

                var angleGap = RadToNormRad(sweepAngle - startAngle);

                var startPoint = GetRingPoint(radiusX, radiusY, centerX, centerY, startAngle);
                var endPoint = GetRingPoint(radiusX, radiusY, centerX, centerY, sweepAngle);

                var arcGeometry = new StreamGeometry();

                using (var ctx = arcGeometry.Open())
                {

                    ctx.BeginFigure(startPoint, false);


                    ctx.ArcTo(endPoint, new Size(radiusX, radiusY), 0, angleGap >= Math.PI,
                        SweepDirection.Clockwise);
                    ctx.LineTo(new Point(0, 0));
                    ctx.EndFigure(false);
                }

                return arcGeometry;
            }
        }


        private static double DegreesToRad(double inAngle) =>
        inAngle * Math.PI / 180;

        private static double RadToNormRad(double inAngle) => ((inAngle % (Math.PI * 2)) + (Math.PI * 2)) % (Math.PI * 2);

        private static Point GetRingPoint(double radiusX, double radiusY, double centerX, double centerY, double angle) =>
            new Point((radiusX * Math.Cos(angle)) + centerX, (radiusY * Math.Sin(angle)) + centerY);
    }
}
