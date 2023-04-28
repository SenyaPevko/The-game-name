using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public static EntityController entityController;
}
