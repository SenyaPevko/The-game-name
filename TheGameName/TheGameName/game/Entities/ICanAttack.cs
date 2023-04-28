using Microsoft.Xna.Framework;
using TheGameName;

public interface ICanAttack
{
    private const int MAGAZINE_SIZE = 10;
    private const double FIRE_RATE = 70;

    public int AttackCounter { get; set; }

    public double AttackDelayTimer { get; set; }

    public bool Attack(GameTime gameTime);

    public void Reload(GameTime gameTime);
}
