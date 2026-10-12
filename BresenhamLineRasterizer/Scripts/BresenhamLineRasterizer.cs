using System;
using System.Collections.Generic;
using System.Linq;

public static class BresenhamLineRasterizer
{
    private static Point2D point1;
    private static Point2D point2;
    private static int lineWidth;
    private static int lineHeight;
    private static float lineSlope;
    private static int nextPixel_Y_Decider;
    private static List<Point2D> drawnPixels = new List<Point2D>();

    public static List<Point2D> RasterizeLine(Point2D point1, Point2D point2)
    {
        drawnPixels.Clear();
        InitializeVariables(point1, point2);
        if (IsLineValid())
        {
            PrepareLine();
            Rasterize();
            OrganizeResult();
        }
        return drawnPixels;
    }

    private static void InitializeVariables(Point2D point1, Point2D point2)
    {
        BresenhamLineRasterizer.point1 = point1;
        BresenhamLineRasterizer.point2 = point2;
        lineWidth = point2.X - point1.X;
        lineHeight = point2.Y - point1.Y;
        lineSlope = lineWidth == 0 ? 0 : (float)lineHeight / lineWidth;
        nextPixel_Y_Decider = 0;
    }

    private static bool IsLineValid()
    {
        if (point1.X <= point2.X)
        {
            if (point1.X >= 0 && point2.X >= 0)
            {
                if (point1.Y >= 0 && point2.Y >= 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: (Y.start) and (Y.end) of the line can't be negative.");
                }
            }
            else
            {
                Console.WriteLine("Error: (X.start) and (X.end) of the line can't be negative.");
            }
        }
        else
        {
            Console.WriteLine("Error: (X.start <= X.end) for the line must hold.");
        }
        return false;
    }

    private static void PrepareLine()
    {
        if (lineSlope < 0)
        {
            InvertLineSlopeToPositive();
        }
        if (Math.Abs(lineSlope) > 1)
        {
            SwapLine_X_And_Y_Coordinates();
        }
        UpdateLinelineWidthAndHeight();
    }

    private static void InvertLineSlopeToPositive()
    {
        point2 = new Point2D(point2.X, point2.Y + 2 * Math.Abs(lineHeight));
    }

    private static void SwapLine_X_And_Y_Coordinates()
    {
        point1 = new Point2D(point1.Y, point1.X);
        point2 = new Point2D(point2.Y, point2.X);
    }

    private static void UpdateLinelineWidthAndHeight()
    {
        lineWidth = point2.X - point1.X;
        lineHeight = point2.Y - point1.Y;
    }

    private static void Rasterize()
    {
        int x = point1.X;
        int y = point1.Y;
        while (x <= point2.X)
        {
            DrawPixel(x, y);
            x = PrepareNextPixel_X(x);
            y = PrepareNextPixel_Y(y);
        }
    }

    private static void DrawPixel(int x, int y)
    {
        drawnPixels.Add(new Point2D(x, y));
    }

    private static int PrepareNextPixel_X(int x)
    {
        return ++x;
    }

    private static int PrepareNextPixel_Y(int y)
    {
        if (Pixel_Y_ShouldBeShiftedUp())
        {
            y++;
            nextPixel_Y_Decider += 2 * (lineHeight - lineWidth);
        }
        else
        {
            nextPixel_Y_Decider += 2 * lineHeight;
        }
        return y;
    }

    private static bool Pixel_Y_ShouldBeShiftedUp()
    {
        return 2 * lineHeight - lineWidth + nextPixel_Y_Decider > 0;
    }

    private static void OrganizeResult()
    {
        if (Math.Abs(lineSlope) > 1)
        {
            SwapDrawnPixels_X_And_Y_Coordinates();
        }
        if (lineSlope < 0)
        {
            InvertSlopeOfRasterizedLineToNegative();
        }
    }

    private static void SwapDrawnPixels_X_And_Y_Coordinates()
    {
        for (int i = 0; i < drawnPixels.Count(); i++)
        {
            Point2D pixel = drawnPixels[i];
            drawnPixels[i] = new Point2D(pixel.Y, pixel.X);
        }
    }

    private static void InvertSlopeOfRasterizedLineToNegative()
    {
        for (int i = 0; i < drawnPixels.Count(); i++)
        {
            Point2D pixel = drawnPixels[i];
            if (Math.Abs(lineSlope) > 1)
            {
                drawnPixels[i] = new Point2D(pixel.X, pixel.Y - 2 * (pixel.Y - point1.X));
            }
            else
            {
                drawnPixels[i] = new Point2D(pixel.X, pixel.Y - 2 * (pixel.Y - point1.Y));
            }
        }
    }

    public static void PrintPixelCoordinatesOfRasterizedLine(List<Point2D> rasterizedLine)
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