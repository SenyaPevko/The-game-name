using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class BulletsContoller : IUpdatable
{
    private List<Bullet> bulletsFired = new List<Bullet> { };
    private Texture2D bulletTexture;

    public BulletsContoller(Texture2D bulletTexture)
    {
        this.bulletTexture = bulletTexture;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var bullet in bulletsFired)
        {
            bullet.Update(gameTime);
        }
        for(var i = 0; i < bulletsFired.Count; i++)
        {
            RemoveBulletReachedScreenEnd(bulletsFired[i], i);
        }
    }

    public void RemoveBulletReachedScreenEnd(Bullet bullet, int index)
    {
        if (bullet.Position.X > Globals.screenWidth - 50 || bullet.Position.Y > Globals.screenHeight - 50 || bullet.Position.X < 50 || bullet.Position.Y < 50)
        {
            if (bulletsFired.Count > index)
                bulletsFired.RemoveAt(index);
        }
    }

    public void AddBulletToFired(Player player)
    {
        var bullet = new Bullet(bulletTexture, player);
        bulletsFired.Add(bullet);
    }

    public void Draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        foreach (var bullet in bulletsFired)
        {
            bullet.Draw(spritebatch, gameTime);
        }
    }
}