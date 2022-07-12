using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSkiaClockDemo
{
    /// <summary>
    /// 一个简单版的时钟
    /// </summary>
    public class DrawClock
    {
        public SKPoint centerPoint;
        public int Radius = 0;
        public int HAND_TRUNCATION;
        public int HOUR_HAND_TRUNCATION;
        public int HAND_RADIUS;
        public int TIPS;
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render(SKCanvas canvas, SKTypeface Font, int Width, int Height)
        {
            centerPoint = new SKPoint(Width / 2, Height / 2);
            this.Radius = (int)(centerPoint.Y - 50);
            HAND_TRUNCATION = Width / 25;
            HOUR_HAND_TRUNCATION = Width / 10;
            HAND_RADIUS = this.Radius + 15;
            TIPS = this.Radius - 40;

            canvas.Clear(SKColors.SkyBlue);
            DrawCircle(canvas, Font);
            DrawCenter(canvas, Font);
            DrawHands(canvas, Font);
            DrawTimeNumber(canvas, Font);
            DrawTips(canvas, Font);


            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 20
            };
            using var paint2 = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 24
            };
            string msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff:ffffff}";
            string tishi = " WPF SkiaSharp 自绘时钟 基础代码";
            string by = $"by 蓝创精英团队";

            canvas.DrawText(msg, 0, 30, paint);
            canvas.DrawText(tishi, 450, 30, paint);
            canvas.DrawText(by, 600, 400, paint2);
        }
        /// <summary>
        /// 画一个圆
        /// </summary>
        public void DrawCircle(SKCanvas canvas, SKTypeface Font)
        {
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 2
            };
            canvas.DrawCircle(centerPoint.X, centerPoint.Y, Radius, paint);
        }
        /// <summary>
        /// 时钟的核心
        /// </summary>
        public void DrawCenter(SKCanvas canvas, SKTypeface Font)
        {
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                StrokeWidth = 2
            };
            canvas.DrawCircle(centerPoint.X, centerPoint.Y, 5, paint);
        }

        private void DrawHand(SKCanvas canvas, SKTypeface Font, int times, bool isHour = false)
        {
            var angle = Math.PI * 2 * (times / (double)60) - Math.PI / 2;
            var handRadius = isHour ? this.Radius - HAND_TRUNCATION - HOUR_HAND_TRUNCATION : this.Radius - HAND_TRUNCATION;
            using var paint = new SKPaint
            {
                Color = (DateTimeOffset.Now.Second % 4 <= 1) ? SKColors.Red : SKColors.Green,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 2,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
            var path = new SKPath();
            path.MoveTo(centerPoint);
            path.LineTo((float)(centerPoint.X + Math.Cos(angle) * handRadius), (float)(centerPoint.Y + Math.Sin(angle) * handRadius));
            path.Close();
            canvas.DrawPath(path, paint);
        }
        /// <summary>
        /// 画时针
        /// </summary>
        public void DrawHands(SKCanvas canvas, SKTypeface Font)
        {
            var time = DateTime.Now;
            var hour = time.Hour > 12 ? time.Hour - 12 : time.Hour;
            DrawHand(canvas, Font, hour * 5 + time.Minute / 60 * 5, true);
            DrawHand(canvas, Font, time.Minute, false);
            DrawHand(canvas, Font, time.Second, false);
        }
        /// <summary>
        /// 画时间点
        /// </summary>
        public void DrawTimeNumber(SKCanvas canvas, SKTypeface Font)
        {
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 24
            };
            for (int i = 1; i <= 12; i++)
            {
                var angle = Math.PI / 6 * (i - 3);
                var number = i.ToString();
                var numberTextWidth = paint.MeasureText(number);
                canvas.DrawText(number, (float)(centerPoint.X + Math.Cos(angle) * HAND_RADIUS - numberTextWidth / 2), (float)(centerPoint.Y + Math.Sin(angle) * HAND_RADIUS + 24 / 3), paint);
            }
        }
        /// <summary>
        /// 画提示信息
        /// </summary>
        public void DrawTips(SKCanvas canvas, SKTypeface Font)
        {
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 20
            };
            var now = DateTime.Now;
            //年月日
            var Date = $"{now.Year}/{now.Month}/{now.Day}";
            var DateTextWidth = paint.MeasureText(Date);
            var angle = Math.PI / 6 * (6 - 3);
            canvas.DrawText(Date, (float)(centerPoint.X + Math.Cos(angle) * TIPS - DateTextWidth / 2), (float)(centerPoint.Y + Math.Sin(angle) * TIPS), paint);
            //PM AM
            var amOrPm = now.Hour > 12 ? "PM" : "AM";
            var amOrPmTextWidth = paint.MeasureText(amOrPm);
            var angle2 = Math.PI / 6 * (12 - 3);
            canvas.DrawText(amOrPm, (float)(centerPoint.X + Math.Cos(angle2) * TIPS - amOrPmTextWidth / 2), (float)(centerPoint.Y + Math.Sin(angle2) * TIPS), paint);
        }
    }
}
