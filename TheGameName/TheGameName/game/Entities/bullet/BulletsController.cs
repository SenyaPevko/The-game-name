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
    private HashSet<Bullet> bulletsFired;
    private Texture2D bulletTexture;

    public BulletsContoller(Texture2D bulletTexture)
    {
        this.bulletTexture = bulletTexture;
        bulletsFired = new HashSet<Bullet>();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var bullet in bulletsFired)
        {
            if (!bullet.IsAlive)
            {
                bulletsFired.Remove(bullet);
                continue;
            }
            bullet.Update(gameTime);
        }
    }

    public void AddBulletToFired(IGameEntity sender, Vector2 target)
    {
        var bullet = new Bullet(bulletTexture, sender, target);
        bulletsFired.Add(bullet);
        TheGameName.EntityController.AddEntity(bullet);
    }
}