using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class Globals
{
    public static int screenHeight = 900;
    public static int screenWidth = 1600;

    public static ContentManager content;
    public static SpriteBatch spriteBatch;

    public static Cursor cursor;
    public static BulletsContoller bulletsContoller;


    public static float GetDistance(Vector2 pos, Vector2 target)
    {
        return (float)Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
    }


    public static float RotateTowards(Vector2 Pos, Vector2 focus)
    {

        float h, sineTheta, angle;
        if (Pos.Y - focus.Y != 0)
        {
            h = (float)Math.Sqrt(Math.Pow(Pos.X - focus.X, 2) + Math.Pow(Pos.Y - focus.Y, 2));
            sineTheta = (float)(Math.Abs(Pos.Y - focus.Y) / h); //* ((item.Pos.Y-focus.Y)/(Math.Abs(item.Pos.Y-focus.Y))));
        }
        else
        {
            h = Pos.X - focus.X;
            sineTheta = 0;
        }

        angle = (float)Math.Asin(sineTheta);

        // Drawing diagonial lines here.
        //Quadrant 2
        if (Pos.X - focus.X > 0 && Pos.Y - focus.Y > 0)
        {
            angle = (float)(Math.PI * 3 / 2 + angle);
        }
        //Quadrant 3
        else if (Pos.X - focus.X > 0 && Pos.Y - focus.Y < 0)
        {
            angle = (float)(Math.PI * 3 / 2 - angle);
        }
        //Quadrant 1
        else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y > 0)
        {
            angle = (float)(Math.PI / 2 - angle);
        }
        else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y < 0)
        {
            angle = (float)(Math.PI / 2 + angle);
        }
        else if (Pos.X - focus.X > 0 && Pos.Y - focus.Y == 0)
        {
            angle = (float)Math.PI * 3 / 2;
        }
        else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y == 0)
        {
            angle = (float)Math.PI / 2;
        }
        else if (Pos.X - focus.X == 0 && Pos.Y - focus.Y > 0)
        {
            angle = (float)0;
        }
        else if (Pos.X - focus.X == 0 && Pos.Y - focus.Y < 0)
        {
            angle = (float)Math.PI;
        }

        return angle;
    }

    public static double CalculateAngle(Vector2 pos, Vector2 focus)
    {
        var x1 = pos.X;
        var y1 = pos.Y;
        var x2 = focus.X;
        var y2 = focus.Y;
        var angle = Math.Acos((x1*x2 + y1*y2)/ (Math.Sqrt(x1*x1 + y1*y1) * Math.Sqrt(x2 * x2 + y2 * y2)));
        return angle;
    }
}
