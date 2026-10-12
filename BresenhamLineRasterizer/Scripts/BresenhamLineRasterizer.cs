using System;
using System.Collections.Generic;
using System.Linq;

public class BresenhamLineRasterizer
{
    private Point2D startPoint;
    private Point2D endPoint;
    private int lineWidth;
    private int lineHeight;
    private float lineSlope;
    private int nextPixel_Y_Decider;
    private List<Point2D> linePixels = new List<Point2D>();

    public List<Point2D> GetRasterizedLine(Point2D startPoint, Point2D endPoint)
    {
        linePixels.Clear();
        InitializeVariables(startPoint, endPoint);
        if (IsLineValid())
        {
            PrepareLine();
            Rasterize();
            OrganizeResult();
        }
        return linePixels;
    }

    private void InitializeVariables(Point2D startPoint, Point2D endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        lineWidth = endPoint.X - startPoint.X;
        lineHeight = endPoint.Y - startPoint.Y;
        lineSlope = lineWidth == 0 ? 0 : (float)lineHeight / lineWidth;
        nextPixel_Y_Decider = 0;
    }

    private bool IsLineValid()
    {
        if (startPoint.X <= endPoint.X)
        {
            if (startPoint.X >= 0 && endPoint.X >= 0)
            {
                if (startPoint.Y >= 0 && endPoint.Y >= 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("\nError: (Y.start) and (Y.end) can't be negative.");
                }
            }
            else
            {
                Console.WriteLine("\nError: (X.start) and (X.end) can't be negative.");
            }
        }
        else
        {
            Console.WriteLine("\nError: (X.start <= X.end) must hold.");
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

        // Update data
        lineWidth = endPoint.X - startPoint.X;
        lineHeight = endPoint.Y - startPoint.Y;
    }

    private void InvertLineSlopeToPositive()
    {
        endPoint = new Point2D(endPoint.X, endPoint.Y + 2 * Math.Abs(lineHeight));
    }

    private void SwapLine_X_And_Y_Coordinates()
    {
        startPoint = new Point2D(startPoint.Y, startPoint.X);
        endPoint = new Point2D(endPoint.Y, endPoint.X);
    }

    private void Rasterize()
    {
        int x = startPoint.X;
        int y = startPoint.Y;

        while (x <= endPoint.X)
        {
            AddPixel(x, y);

            x = PrepareNextPixel_X(x);
            y = PrepareNextPixel_Y(y);
        }
    }

    private void AddPixel(int x, int y)
    {
        linePixels.Add(new Point2D(x, y));
    }

    private int PrepareNextPixel_X(int x)
    {
        return ++x;
    }

    private int PrepareNextPixel_Y(int y)
    {
        if (ShouldPixelSiftUp())
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

    private bool ShouldPixelSiftUp()
    {
        return 2 * lineHeight - lineWidth + nextPixel_Y_Decider > 0;
    }

    private void OrganizeResult()
    {
        if (Math.Abs(lineSlope) > 1)
        {
            SwapLinePixels_X_And_Y_Coordinates();
        }
        if (lineSlope < 0)
        {
            InvertSlopeOfRasterizedLineToNegative();
        }
    }

    private void SwapLinePixels_X_And_Y_Coordinates()
    {
        for (int i = 0; i < linePixels.Count(); i++)
        {
            Point2D pixel = linePixels[i];
            linePixels[i] = new Point2D(pixel.Y, pixel.X);
        }
    }

    private void InvertSlopeOfRasterizedLineToNegative()
    {
        for (int i = 0; i < linePixels.Count(); i++)
        {
            Point2D pixel = linePixels[i];
            if (Math.Abs(lineSlope) > 1)
            {
                linePixels[i] = new Point2D(pixel.X, pixel.Y - 2 * (pixel.Y - startPoint.X));
            }
            else
            {
                linePixels[i] = new Point2D(pixel.X, pixel.Y - 2 * (pixel.Y - startPoint.Y));
            }
        }
    }
}