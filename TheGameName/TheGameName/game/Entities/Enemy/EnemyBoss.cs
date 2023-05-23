using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class EnemyBoss : Enemy
{
    public EnemyBoss(Vector2 position, Texture2D texture, Player player, double health) : base(position, texture, player, health)
    {
    }
}