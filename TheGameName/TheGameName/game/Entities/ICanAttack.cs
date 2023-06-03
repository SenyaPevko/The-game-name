using Microsoft.Xna.Framework;

namespace TheGameName;

public interface ICanAttack
{
    private const int MAGAZINE_SIZE = 10;
    private const double FIRE_RATE = 70;

    public int AttackCounter { get;}

    public double AttackDelayTimer { get;}

    public bool Attack(GameTime gameTime);

    public void Reload(GameTime gameTime);
}
