using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TPL._18_Parallel.For
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new();

            Frame[] frames = Renderer.MakeEmptyFrames(60, 3820, 2160);

            Console.WriteLine($"Frames rendering has started sequentially...");

            stopwatch.Start();

            for (int i = 0; i < frames.Length; i++)
            {
                Renderer.RenderFrame(frames[i]);
            }

            Console.WriteLine($"Frames rendering has finished sequentially after {stopwatch.ElapsedMilliseconds} milliseconds.{Environment.NewLine}");

            stopwatch.Reset();

            Console.WriteLine($"Frames rendering has started parallel...");

            stopwatch.Start();

            Parallel.For(0, frames.Length, (i) => Renderer.RenderFrame(frames[i]));

            Console.WriteLine($"Frames rendering has finished parallel after {stopwatch.ElapsedMilliseconds} milliseconds.");

            stopwatch.Stop();
        }
    }

    internal struct Frame
    {
        public int Width { get; }

        public int Height { get; }

        public Pixel[] Pixels { get; }

        public Frame(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Pixel[Width * Height];
        }
    }

    internal struct Pixel
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;
    }

    internal static class Renderer
    {
        internal static void RenderFrame(Frame frame)
        {
            Pixel[] framePixels = frame.Pixels;

            for (int i = 0; i < framePixels.Length; i++)
            {
                RenderPixel(framePixels[i], i);
            }
        }

        internal static void RenderPixel(Pixel pixel, int index)
        {
            byte coefficient = (byte)(index % byte.MaxValue);

            pixel.R = (byte)(byte.MaxValue - coefficient);
            pixel.G = (byte)(pixel.R - coefficient);
            pixel.B = (byte)(pixel.G - coefficient);
            pixel.A = (byte)(pixel.B - coefficient);
        }

        internal static Frame[] MakeEmptyFrames(int number, int resWidth, int resHeight) =>
            Enumerable.Range(0, number)
            .Select(i => new Frame(resWidth, resHeight))
            .ToArray();
    }
}
