using System;
using System.Linq;
using System.Threading.Tasks;

namespace TPL._19_Parallel.For.ParallelLoopState_ParallelLoopResult
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Frame[] frames = Renderer.MakeEmptyFrames(60, 3820, 2160);

            Action<int, ParallelLoopState> loopAction = (i, loopState) =>
            {
                if (i > 30)
                {
                    loopState.Break();
                }

                Renderer.RenderFrame(frames[i]);
            };

            Console.WriteLine($"Frames rendering has started...");

            ParallelLoopResult loopResult = Parallel.For(0, frames.Length, loopAction);

            if (loopResult.IsCompleted)
            {
                Console.WriteLine($"Frames rendering has finished succesfully.");
            }
            else
            {
                Console.WriteLine($"Frames rendering has failed on [{loopResult.LowestBreakIteration}] iteration.");
            }
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
