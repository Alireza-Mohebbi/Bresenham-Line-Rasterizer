using System;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter start point of the line:");
        Console.Write("X1: ");
        int x1 = int.Parse(Console.ReadLine());
        Console.Write("Y1: ");
        int y1 = int.Parse(Console.ReadLine());

        Console.WriteLine("\nEnter end point of the line:");
        Console.Write("X2: ");
        int x2 = int.Parse(Console.ReadLine());
        Console.Write("Y2: ");
        int y2 = int.Parse(Console.ReadLine());

        BresenhamLineRasterizer bresenhamLineRasterizer = new BresenhamLineRasterizer();
        List<Point2D> myLine = bresenhamLineRasterizer.GetRasterizedLine(new Point2D(x1, y1), new Point2D(x2, y2));

        PrintResult(myLine);

        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }

    public static void PrintResult(List<Point2D> rasterizedLine)
    {
        if (rasterizedLine.Count != 0)
        {
            Console.WriteLine("\nPixel coordinates of the rasterized line:");
            foreach (Point2D pixel in rasterizedLine)
            {
                Console.WriteLine("({0}, {1})", pixel.X, pixel.Y);
            }
        }
    }
}