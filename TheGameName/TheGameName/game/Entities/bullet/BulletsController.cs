using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheGameName;

public class BulletsContoller : IUpdatable
{
    private HashSet<Bullet> firedBullets;
    private Texture2D bulletTexture;

    public BulletsContoller(Texture2D bulletTexture)
    {
        this.bulletTexture = bulletTexture;
        firedBullets = new HashSet<Bullet>();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var bullet in firedBullets)
        {
            if (!bullet.IsAlive)
            {
                firedBullets.Remove(bullet);
                continue;
            }
            bullet.Update(gameTime);
        }
    }

    public void AddBulletToFired(IGameEntity sender, Vector2 target)
    {
        var bullet = new Bullet(bulletTexture, sender, target);
        firedBullets.Add(bullet);
        TheGameName.EntityController.AddEntity(bullet);
    }

    public void Restart()
    {
        foreach(var bullet in firedBullets)
        {
            TheGameName.EntityController.RemoveEntity(bullet);
        }
        firedBullets.Clear();
    }
}