using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class SpriteAnimation : IUpdatable, IAnimation
{
    public const double DEFAULT_FPS = 12;
    public int SpritesCount { get; set; }
    public Vector2 SpriteOrigin { get; set; }
    public double TotalDuration => SpritesCount / Fps;
    public int CurrentOffset => (int)(PlaybackTime / (1 / Fps));
    public double PlaybackTime { get; set; } = 0;
    public int SpriteWidth { get; set; }
    public int SpriteHeight { get; set; }

    public Texture2D Texture { get; private set; }

    public double Fps { get; set; } = DEFAULT_FPS;

    public AnimationState State { get; set; } = AnimationState.Stopped;

    public event EventHandler<AnimationCompletedEventArgs> AnimationCompleted;

    public SpriteAnimation(Texture2D texture, int spriteCount, int spriteWidth, int spriteHeight)
    {
        SpritesCount = spriteCount;
        SpriteWidth = spriteWidth;
        SpriteHeight = spriteHeight;
        Texture = texture ?? throw new ArgumentNullException(nameof(texture));

        SpriteOrigin = new Vector2(spriteWidth / 4f, spriteHeight / 2f);
    }

    public void Update(GameTime gameTime)
    {
        if (State == AnimationState.Playing)
        {
            PlaybackTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (PlaybackTime > TotalDuration)
            {
                PlaybackTime = 0;
                OnAnimationCompleted();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float opacity, Color color)
    {
        var drawRect = new Rectangle(CurrentOffset * SpriteWidth, 0, SpriteWidth, SpriteHeight);
        spriteBatch.Draw(Texture, new Rectangle(position.ToPoint(), new Point(SpriteWidth, SpriteHeight)), drawRect, color * opacity, 0, SpriteOrigin, spriteEffects, 0);
    }

    public void Play()
    {
        State = AnimationState.Playing;
    }

    public void Stop()
    {
        State = AnimationState.Stopped;
        PlaybackTime = 0;
    }

    public void Pause()
    {
        State = AnimationState.Paused;
    }

    protected virtual void OnAnimationCompleted()
    {
        var handler = AnimationCompleted;
        handler?.Invoke(this, new AnimationCompletedEventArgs());
    }

}
