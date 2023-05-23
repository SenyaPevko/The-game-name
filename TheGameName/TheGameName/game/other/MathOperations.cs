using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public static class MathOperations
{
    public const float EPSIOLON = 0.0001f;

    public static bool IsRoughlyZero(this float val) => Math.Abs(val) < EPSIOLON;

    public static int RoundToInt(float val) => (int)Math.Round(val);

    public static float CalculateHypotenuse(double firstSide, double secondSide)
    {
        return (float)Math.Sqrt(Math.Pow(firstSide, 2) + Math.Pow(secondSide, 2));
    }

    public static Vector2 GetDirectionToPoint(Point p1, Point p2)
    {
        float diffX = p2.X - p1.X;
        float diffY = p2.Y - p1.Y;
        return new Vector2(diffX, diffY);
    }

    public static float GetAngleBetweenPoints(Point p1, Point p2)
    {
        // get the difference x and y from the points
        float deltaX = p2.X - p1.X;
        float deltaY = p2.Y - p1.Y;
        // calculate the angle
        float res = (float)(Math.Atan2(deltaY, deltaX));
        return res;
    }
}