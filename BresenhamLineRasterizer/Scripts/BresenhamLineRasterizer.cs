using System;
using System.Collections.Generic;
using System.Linq;

public class BresenhamLineRasterizer
{
    private Point2D point1;
    private Point2D point2;
    private int lineWidth;
    private int lineHeight;
    private float lineSlope;
    private int nextPixel_Y_Decider;
    private List<Point2D> drawnPixels = new List<Point2D>();

    public List<Point2D> RasterizeLine(Point2D point1, Point2D point2)
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

    private void InitializeVariables(Point2D point1, Point2D point2)
    {
        this.point1 = point1;
        this.point2 = point2;
        lineWidth = point2.X - point1.X;
        lineHeight = point2.Y - point1.Y;
        lineSlope = lineWidth == 0 ? 0 : (float)lineHeight / lineWidth;
        nextPixel_Y_Decider = 0;
    }

    private bool IsLineValid()
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

    private void PrepareLine()
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

    private void InvertLineSlopeToPositive()
    {
        point2 = new Point2D(point2.X, point2.Y + 2 * Math.Abs(lineHeight));
    }

    private void SwapLine_X_And_Y_Coordinates()
    {
        point1 = new Point2D(point1.Y, point1.X);
        point2 = new Point2D(point2.Y, point2.X);
    }

    private void UpdateLinelineWidthAndHeight()
    {
        lineWidth = point2.X - point1.X;
        lineHeight = point2.Y - point1.Y;
    }

    private void Rasterize()
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

    private void DrawPixel(int x, int y)
    {
        drawnPixels.Add(new Point2D(x, y));
    }

    private int PrepareNextPixel_X(int x)
    {
        return ++x;
    }

    private int PrepareNextPixel_Y(int y)
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

    private bool Pixel_Y_ShouldBeShiftedUp()
    {
        return 2 * lineHeight - lineWidth + nextPixel_Y_Decider > 0;
    }

    private void OrganizeResult()
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

    private void SwapDrawnPixels_X_And_Y_Coordinates()
    {
        for (int i = 0; i < drawnPixels.Count(); i++)
        {
            Point2D pixel = drawnPixels[i];
            drawnPixels[i] = new Point2D(pixel.Y, pixel.X);
        }
    }

    private void InvertSlopeOfRasterizedLineToNegative()
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
}