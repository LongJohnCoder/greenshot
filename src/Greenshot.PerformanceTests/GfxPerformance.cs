﻿using System.Drawing;
using System.Drawing.Imaging;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using Greenshot.Gfx;
using Greenshot.Gfx.Experimental;
using Greenshot.Gfx.Quantizer;
using Greenshot.Tests.Implementation;

namespace Greenshot.PerformanceTests
{
    /// <summary>
    /// This defines the benchmarks which can be done
    /// </summary>
    [MinColumn, MaxColumn, MemoryDiagnoser]
    public class GfxPerformance
    {
        //[Benchmark]
        public void WuQuantizer()
        {
            using (var bitmap = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format24bppRgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                }
                var quantizer = new WuQuantizer(bitmap);
                using (var quantizedImage = quantizer.GetQuantizedImage())
                {
                    quantizedImage.Save(@"quantized.png", ImageFormat.Png);
                }
            }
        }

        [Benchmark]
        public void Blur()
        {
            using (var bitmap = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppArgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                }
                bitmap.ApplyBoxBlur(10);
            }
        }

        [Benchmark]
        public void BlurSpan()
        {
            using (var bitmap = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppArgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                }
                bitmap.ApplyBoxBlurSpan(10);
            }
        }

        [Benchmark]
        public void BlurOld()
        {
            using (var bitmap = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppArgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                }
                bitmap.ApplyOldBoxBlur(10);
            }
        }

        [Benchmark]
        public void Scale()
        {
            using (var bitmap = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppArgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                }
                bitmap.Scale2X().Dispose();
            }
        }
    }
}
